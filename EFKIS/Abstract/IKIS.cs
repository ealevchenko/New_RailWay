using EFKIS.Concrete;
using EFKIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFKIS.Abstract
{
    public interface IKIS
    {
        IQueryable<KometaVagonSob> KometaVagonSob { get; }
        IQueryable<KometaVagonSob> GetVagonsSob();
        IQueryable<KometaVagonSob> GetVagonsSob(int num);
        KometaVagonSob GetVagonsSob(int num, DateTime dt);
        IQueryable<KometaVagonSob> GetChangeVagonsSob(DateTime dt, int day_period);
        IQueryable<KometaVagonSob> GetChangeVagonsSob(int day_period);
        List<rwCar> GetVagons();
    }
}
