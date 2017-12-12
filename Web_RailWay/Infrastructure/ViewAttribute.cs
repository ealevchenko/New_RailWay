using EFAccess.Abstract;
using EFAccess.Concrete;
using EFAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Infrastructure
{
    public class ViewAttribute : AuthorizeAttribute, IActionFilter
    {
        private IWebAccess ia;
        private string[] allowedUsers = new string[] { };
        private string[] allowedRoles = new string[] { };

        public ViewAttribute(IWebAccess ia)
        {
            this.ia = ia;
        }

        public ViewAttribute()
        {
            this.ia = new EFWebAcces();
        }

        private bool User(HttpContextBase httpContext)
        {
            if (allowedUsers.Length > 0)
            {
                if (allowedUsers.Contains(httpContext.User.Identity.Name))
                {
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
                        return true;
                    }
                }
            }
            return false;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Trim();
            string action = filterContext.ActionDescriptor.ActionName.Trim();
            string user = filterContext.HttpContext.User.Identity.Name.Trim();
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
                if (!(us | rl))
                {
                    filterContext.Result = new EmptyResult();
                }
            }
        }
    }
}