using EFAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFAccess.Abstract
{
    public interface IWebAccess
    {
        IQueryable<WebAccess> WebAccess { get; }
        IQueryable<WebAccess> GetWebAccess();
        WebAccess GetWebAccess(int id);
        WebAccess GetWebAccess(string controller, string action);
        int SaveWebAccess(WebAccess WebAccess);
        WebAccess DeleteWebAccess(int id);
    }
}
