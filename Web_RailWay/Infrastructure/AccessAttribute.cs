using EFAccess.Abstract;
using EFAccess.Concrete;
using EFAccess.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Infrastructure
{
    public class AccessAttribute : FilterAttribute, IActionFilter
    {
        public bool LogVisit = false;
        private string[] allowedUsers = new string[] { };
        private string[] allowedRoles = new string[] { };
        private string RulesAccess = null;
        private bool? access = false;
        private IWebAccess ia;
        private string controller = null;
        private string action = null;
        private string user = null;
        private bool logPartialView = false;
        private bool logRedirectToRoute = false;

        public AccessAttribute(IWebAccess ia)
        {
            this.ia = ia;
        }

        public AccessAttribute()
        {
            this.ia = new EFWebAcces();
        }

        private bool User(HttpContextBase httpContext)
        {
            if (allowedUsers.Length > 0)
            {
                if (allowedUsers.Contains(httpContext.User.Identity.Name))
                {
                    this.RulesAccess = httpContext.User.Identity.Name;
                    this.access = true;
                    return true;
                }
            }
            return false;
        }

        private bool Role(HttpContextBase httpContext)
        {
            if (allowedRoles.Length > 0)
            {
                for (int i = 0; i < allowedRoles.Length; i++)
                {
                    if (httpContext.User.IsInRole(allowedRoles[i]))
                    {
                        this.RulesAccess = allowedRoles[i];
                        this.access = true;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Выполнен
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is PartialViewResult) return;
            if (filterContext.Result is RedirectToRouteResult) return;
            // если не локал хост
            if (!filterContext.HttpContext.Request.IsLocal & LogVisit)
                filterContext.WriteVisit(this.RulesAccess, this.access);
            return;
        }
        /// <summary>
        /// Перед выполненим
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Trim();
            this.action = filterContext.ActionDescriptor.ActionName.Trim();
            this.user = filterContext.HttpContext.User.Identity.Name.Trim();
            this.RulesAccess = null;
            this.access = false;
            allowedUsers = new string[] { };
            allowedRoles = new string[] { };
            WebAccess ac = this.ia.GetWebAccess(controller, action);
            if (ac != null)
            {

                if (!String.IsNullOrEmpty(ac.users))
                {
                    allowedUsers = ac.users.Split(new char[] { ';' });
                    for (int i = 0; i < allowedUsers.Length; i++)
                    {
                        allowedUsers[i] = allowedUsers[i].Trim();
                    }
                }
                if (!String.IsNullOrEmpty(ac.roles))
                {
                    allowedRoles = ac.roles.Split(new char[] { ';' });
                    for (int i = 0; i < allowedRoles.Length; i++)
                    {
                        allowedRoles[i] = allowedRoles[i].Trim();
                    }
                }
                bool us = User(filterContext.HttpContext);
                bool rl = Role(filterContext.HttpContext);
                //bool re = us && rl;
                if (!(us | rl))
                {
                    this.RulesAccess = null;
                    this.access = false;
                    // если не локал хост
                    if (!filterContext.HttpContext.Request.IsLocal & LogVisit)
                        filterContext.WriteVisit(this.RulesAccess, this.access);

                    string message = filterContext.HttpContext.User.Identity.Name + ";" + filterContext.ActionDescriptor.ActionName;
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "AccessDenied",
                        ViewData = new ViewDataDictionary(filterContext.Controller.ViewData)
                        {
                            Model = message, // set the model
                        }
                    };
                }
            }
            else
            {
                this.RulesAccess = "General access";
                this.access = null;
            }
        }
    }
}