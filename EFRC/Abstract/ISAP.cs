using EFRC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRC.Abstract
{
    public interface ISAP
    {
        IQueryable<SAPIncSupply> SAPIncSupply { get; }
        IQueryable<SAPIncSupply> GetSAPIncSupply();
        SAPIncSupply GetSAPIncSupply(int id);
        int SaveSAPIncSupply(SAPIncSupply SAPIncSupply);
        SAPIncSupply DeleteSAPIncSupply(int id);

        IQueryable<SAPIncSupply> GetSAPIncSupplyOfSostav(int idsostav);
        SAPIncSupply GetSAPIncSupply(int idsostav, int num);
        List<int> GetSAPIncSupplyToNumWagons(int idsostav);
        int UpdateSAPIncSupplyIDSostav(int new_idsostav, int old_idsostav);
        int UpdateSAPIncSupplyPosition(int idsostav, int numvag, int position);
        int DeleteSAPIncSupplySostav(int idsostav);
        int DeleteSAPIncSupply(int idsostav, int numvag);
        int CountSAPIncSupply(int idsostav);
    }
}
