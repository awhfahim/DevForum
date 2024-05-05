using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflow.Web.Areas.Discussion.Models.TagModels;

namespace StackOverflow.Web.Areas.Discussion.Controllers
{
    [Authorize]
    [Area("Discussion")]
    public class TagController : Controller  
    {
        private readonly ILogger<TagController> _logger;
        private readonly ILifetimeScope _scope;

        public TagController(ILogger<TagController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }
        public async Task<IActionResult> Index()
        {
            var model = new TagListModel();
            model.Resolve(_scope);
            await model.GetAllTagsASync();
            return View(model);
        }
        
        public async Task<IActionResult> GetQuestionsByTagId(Guid tagId, string tagName)
        {
            var model = new TagListModel();
            model.TagName = tagName;
            model.Resolve(_scope);
            await model.GetQuestionByTagIdAsync(tagId);
            return View(model);
        }
    }
}