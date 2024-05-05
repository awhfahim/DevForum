using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using StackOverflow.Application;
using StackOverflow.Application.Contracts.Features.AccountManagementServices;
using StackOverflow.Application.Contracts.Features.AwsManagementServices;
using StackOverflow.Application.Contracts.Properties;
using StackOverflow.Infrastructure.Membership;

namespace StackOverflow.Web.Models.AuthModels
{
    public class RegisterModel
    {
	    private UserManager<ApplicationUser> _userManager;
	    private SignInManager<ApplicationUser> _signInManager;
	    private IApplicationUnitOfWork _unitOfWork;
	    private IMemberManagementService _memberManagementService;
	    private IEmailQueueService _emailQueueService;

		[Required, EmailAddress, Display(Name = "Email")]
		public string Email { get; set; }

		[Required, Display(Name = "Password"), DataType(DataType.Password)]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		public string Password { get; set; }

		public string? ReturnUrl { get; set; }

		public RegisterModel()
		{
		}
		public RegisterModel(ApplicationUserManager userManager, SignInManager<ApplicationUser> signInManager,
			IMemberManagementService memberManagementService, IApplicationUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_memberManagementService = memberManagementService;
			_unitOfWork = unitOfWork;
		}


		internal async Task<(IEnumerable<IdentityError>? errors, string? redirectLocation)> RegisterAsync()
		{
			ReturnUrl ??= "~/";

			await using var transaction = _unitOfWork.BeginTransaction();
			try
			{
				var user = new ApplicationUser { UserName = Email, Email = Email };
				await _userManager.AddClaimAsync(user, new Claim("CreateQuestion", "true"));
				await _userManager.AddClaimAsync(user, new Claim("RetrieveQuestion", "true"));
				var result = await _userManager.CreateAsync(user, Password);
				if (!result.Succeeded) return result.Succeeded ? (null, ReturnUrl) : (result.Errors, null);
				await _memberManagementService.ConnectMemberTableAsync(user.Id, Email);
				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
				var callbackUrl = $"https://localhost/Auth/ConfirmEmail?email={user.Email}&code={code}";
					
				var emailMessage = new EmailMessage
				(
					Email,
					"Confirm your email",
					$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
				);

				await _emailQueueService.EnqueueEmailAsync(emailMessage);
					
				if(_userManager.Options.SignIn.RequireConfirmedEmail)
				{
					await transaction.CommitAsync();
					return result.Succeeded ? (null, "https://localhost/Auth/RegisterConfirmation?email=" + Email) 
						: (result.Errors, null);
				}

				await _signInManager.SignInAsync(user, isPersistent: false);
				await transaction.CommitAsync();
				return result.Succeeded ? (null, ReturnUrl) : (result.Errors, null);
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				return (null, ReturnUrl);
			}
		}

		internal void Resolve(ILifetimeScope scope)
		{
			_userManager = scope.Resolve<UserManager<ApplicationUser>>();
			_signInManager = scope.Resolve<SignInManager<ApplicationUser>>();
			_unitOfWork = scope.Resolve<IApplicationUnitOfWork>();
			_memberManagementService = scope.Resolve<IMemberManagementService>();
			_emailQueueService = scope.Resolve<IEmailQueueService>();
		}
	}
}
