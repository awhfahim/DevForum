using System.Net;
using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackOverflow.Infrastructure.Membership;
using StackOverflow.Web.Models.AccountModels;
using StackOverflow.Application.Contracts.Features.AwsManagementServices;

namespace StackOverflow.Web.Controllers
{
	[Authorize]
    public class AccountController : Controller
    {
		private readonly ILogger<AccountController> _logger;
		private readonly ILifetimeScope _scope;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IImageManagementService _imageManagementService;

		public AccountController(ILogger<AccountController> logger, ILifetimeScope scope,
            UserManager<ApplicationUser> userManager, IImageManagementService imageManagementService)
        {
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_scope = scope ?? throw new ArgumentNullException(nameof(scope));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
			_imageManagementService = imageManagementService;
        }

		//public IActionResult ForgotPassword() { return View(); }
		//public IActionResult ResetPassword() { return View(); }
		//public IActionResult AccessDenied() { return View(); }

		public async Task<IActionResult> UserProfile()
		{
			var model = new ProfileModel();
			model.Resolve(_scope);
			await model.GetProfileAsync(_userManager.GetUserId(User));
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> UserProfile(ProfileModel model)
		{
			if(!ModelState.IsValid) return View(model);
			
			model.Resolve(_scope);
			var userId =  _userManager.GetUserId(User);
			await model.UpdateProfileAsync(Guid.Parse(userId!));
			return RedirectToAction("Index", "Home");
		}
		
		[HttpPost, ValidateAntiForgeryToken] //Authorize(Policy = "RequireProfileImage")]
		public async Task UploadProfilePicture(IFormFile profilePicture)
		{
			if (profilePicture.Length > 0)
			{
				try
				{
					const string bucketName = "fahim-imagestore-bucket";
					await _imageManagementService.CreateBucketIfNotExistsAsync(bucketName);
					var userId = _userManager.GetUserId(User);
					var key = $"profile-pictures/{userId}.jpg";
					var result = await _imageManagementService.UploadImageAsync(profilePicture, bucketName, key);
					if (result is HttpStatusCode.OK)
					{
						_logger.Log(LogLevel.Information, "Image uploaded to S3");
						var user = await _userManager.GetUserAsync(User);
						user!.ProfileImageKey = key;
						await _userManager.UpdateAsync(user);
					}
					else
					{
						throw new Exception($"Error uploading image to S3. HttpStatusCode is {result}");
					}
				}
				catch (Exception e)
				{
					_logger.Log(LogLevel.Error, "Error uploading image to S3");
				}
				
				
			}
		}
		
		//[HttpGet, Authorize(Policy = "RequireProfileImage")]
		public async Task<string?> GetProfilePictureUrl()
		{
			var userId = _userManager.GetUserId(User);
			var id = Guid.Parse(userId!);
			var user = await _userManager.GetUserAsync(User);

			var key = user?.ProfileImageKey?.ToString();

			if (key == null) return null;
			var result = await _imageManagementService.GetPresignedUrlAsync("fahim-imagestore-bucket", 
				key, DateTime.Now.AddMinutes(30));
			if (result.ErrorMessage != null)
			{
				_logger.Log(LogLevel.Error, "Error generating presigned URL");
				throw new Exception("Error generating presigned URL");
			}
			return result.Url;
		}
    }
}
