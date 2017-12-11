using EFAccess.Abstract;
using EFAccess.Entities;
using MessageLog;
using libClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFAccess.Concrete
{
    public class EFWebAcces : IWebAccess
    {
        protected EFDbContext context = new EFDbContext();
        private eventID eventID = eventID.EFAccess_EFWebAcces;



        public IQueryable<WebAccess> WebAccess
        {
            get { return context.WebAccess; }
        }

        public IQueryable<WebAccess> GetWebAccess()
        {
            try
            {
                return WebAccess;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWebAccess()"), eventID);
                return null;
            }
        }

        public WebAccess GetWebAccess(int id)
        {
            try
            {
                return GetWebAccess().Where(a => a.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWebAccess(id={0})", id), eventID);
                return null;
            }
        }

        public WebAccess GetWebAccess(string controller, string action)
        {
            try
            {
                return GetWebAccess().Where(a => a.controller.ToLower() == controller.ToLower() & a.action.ToLower() == action.ToLower()).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWebAccess(controller={0}, action={1})", controller, action), eventID);
                return null;
            }
        }

        public int SaveWebAccess(WebAccess WebAccess)
        {
            WebAccess dbEntry;
            try
            {
                if (WebAccess.id == 0)
                {
                    dbEntry = new WebAccess()
                    {
                        id = 0,
                        description = WebAccess.description,
                        action = WebAccess.action,
                        controller = WebAccess.controller,
                        roles = WebAccess.roles,
                        users = WebAccess.users
                    };
                    context.WebAccess.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.WebAccess.Find(WebAccess.id);
                    if (dbEntry != null)
                    {
                        dbEntry.description = WebAccess.description;
                        dbEntry.action = WebAccess.action;
                        dbEntry.controller = WebAccess.controller;
                        dbEntry.roles = WebAccess.roles;
                        dbEntry.users = WebAccess.users;
                    }
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("WebAccess(WebAccess={0})", WebAccess.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public WebAccess DeleteWebAccess(int id)
        {
            WebAccess dbEntry = context.WebAccess.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.WebAccess.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteWebAccess(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
    }
}
