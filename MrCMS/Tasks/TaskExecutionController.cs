﻿using System.Web.Mvc;
using MrCMS.Settings;
using MrCMS.Website.Controllers;

namespace MrCMS.Tasks
{
    public class TaskExecutionController : MrCMSUIController
    {
        private readonly SiteSettings _siteSettings;
        private readonly ITaskRunner _taskRunner;

        public TaskExecutionController(SiteSettings siteSettings, ITaskRunner taskRunner)
        {
            _siteSettings = siteSettings;
            _taskRunner = taskRunner;
        }

        // Basic admin-configurable security
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsLocal)
                return;
            var item = filterContext.HttpContext.Request[_siteSettings.TaskExecutorKey];
            if (string.IsNullOrWhiteSpace(item) || item != _siteSettings.TaskExecutorPassword)
                filterContext.Result = new EmptyResult();
        }

        public ContentResult Execute()
        {
            var result = _taskRunner.ExecutePendingTasks();
            return new ContentResult { Content = "Executed", ContentType = "text/plain" };
        }
    }
}