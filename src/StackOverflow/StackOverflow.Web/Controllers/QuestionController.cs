using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackOverflow.Application.Contracts.Features;
using StackOverflow.Domain.consts;
using StackOverflow.Infrastructure.Extensions;
using StackOverflow.Infrastructure.Membership;
using StackOverflow.Web.ActionFilters;
using StackOverflow.Web.Models;
using StackOverflow.Web.Models.QuestionModels;

namespace StackOverflow.Web.Controllers;

[Authorize]
public class QuestionController : Controller
{
    private readonly ILifetimeScope _scope;
    private readonly ILogger<QuestionController> _logger;
    private readonly IRecaptchaService _recaptchaService;
    private readonly UserManager<ApplicationUser> _userManager;
    public QuestionController(ILogger<QuestionController> logger, ILifetimeScope scope, 
        IRecaptchaService recaptchaService, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _scope = scope;
        _recaptchaService = recaptchaService;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(Policy = "QuestionCreatePolicy")]
    public IActionResult Ask()
    {
        return View();
    }
    
    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Policy = "QuestionCreatePolicy")]
    [ServiceFilter(typeof(GetApplicationUserIdActionFilter))]
    public async Task<IActionResult> Ask(AskViewModel question)
    {
        var recaptchaResponse = Request.Form["g-recaptcha-response"];
        if (string.IsNullOrEmpty(recaptchaResponse))
        {
            ModelState.AddModelError(string.Empty, "Please verify you are not a robot.");
            return View(question);
        }

        var isCaptchaValid = await _recaptchaService.IsCaptchaValidAsync(recaptchaResponse!);
        if (!isCaptchaValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid captcha. Please try again.");
            return View(question);
        }

        if (!ModelState.IsValid) return View(question);
        
        question.Resolve(_scope);
        await question.CreateQuestionAsync();
        _logger.LogInformation("Question created successfully");
        return RedirectToAction("RetrieveAllQuestion");
    }
    
    [Authorize(Policy = "QuestionRetrievePolicy")]
    public async Task<IActionResult> RetrieveAllQuestion(int pageNumber = 1, int pageSize = 10, string sortOption = null)
    {
        var model = new QuestionRetrievalViewModel();
        model.Resolve(_scope);
        var data = await model.GetQuestionsAsync(pageNumber, pageSize, sortOption);
        ViewBag.PageNumber = pageNumber;
        int totalPageNumber = (int)Math.Ceiling((double)model.TotalQuestions / pageSize);
        if(totalPageNumber > 0)
        {
            ViewBag.TotalPage = totalPageNumber;
        }
        else
        {
            ViewBag.TotalPage = 1;
        }
        ViewBag.PageSize = pageSize;
        ViewBag.SortOption = sortOption;
        
        // Check if the request is an AJAX request
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_QuestionListPartial", data);
        }
        return View(data);
    }
    
    public async Task<IActionResult> Question(Guid id)
    {
        var model = new QuestionRetrievalViewModel();
        model.Resolve(_scope);
        await model.GetQuestionAsync(id);
        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> AddComment()
    {
        var userId = Guid.Parse(_userManager.GetUserId(User)!);
        var model = new QuestionCommentViewModel();
        model.Resolve(_scope);
        var canComment = await model.CheckUserReputationAsync(userId);
        
        if (!canComment)
        {
            TempData.Put("CommentResponseMessage", new ResponseModel()
            {
                Message = Messages.AddCommentNotAllowed
            });
        }
        return Json(canComment);
    }
    
    [HttpPost, ValidateAntiForgeryToken]
    [ServiceFilter(typeof(GetApplicationUserIdActionFilter))]
    public async Task<IActionResult> AddComment(QuestionCommentViewModel model)
    {
        if (ModelState.IsValid)
        {
            model.Resolve(_scope);
            await model.AddCommentAsync();
            return RedirectToAction("Question", new { id = model.QuestionId });
        }
        return RedirectToAction("Question", new { id = model.QuestionId });
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = new QuestionRetrievalViewModel();
        model.Resolve(_scope);
        await model.GetQuestionAsync(id);
        return View(model);
    }
    
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(QuestionRetrievalViewModel model)
    {
        if (ModelState.IsValid)
        {
            model.Resolve(_scope);
            await model.EditQuestionAsync();
            return RedirectToAction("Question", new { id = model.Id });
        }
        return RedirectToAction("Question", new { id = model.Id });
    }
    
    [ServiceFilter(typeof(GetApplicationUserIdActionFilter))]
    public async Task<IActionResult> AddVote(QuestionVoteViewModel model)
    {
        if (!ModelState.IsValid) return RedirectToAction("Question", new { id =model.Id });
        
        model.Resolve(_scope);
        var resultMessage = await model.AddVoteAsync();
        
        if(resultMessage is Messages.UpvoteNotAllowed or Messages.DownvoteNotAllowed)
        {
            TempData.Put("ResponseMessage", new ResponseModel
            {
                Message = resultMessage,
            });
        }
        
        return RedirectToAction("Question", new { id =model.Id });
     }
}