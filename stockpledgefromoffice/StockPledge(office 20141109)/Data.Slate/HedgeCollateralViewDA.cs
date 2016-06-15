using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFUtils;

namespace Data.Slate
{
    public class HedgeCollateralViewDA : IDataProvider<HedgeCollateralView_AlanPledger>
    {
        SlateEntities se = new SlateEntities();

        public void Load(out HedgeCollateralView_AlanPledger obj, int Id)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<HedgeCollateralView_AlanPledger> collection)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<HedgeCollateralView_AlanPledger> collection, DateTime dateOfData)
        {
            se.HedgeCollateralView_AlanPledger.Where(x => x.ActivityDate == dateOfData).ToList().ForEach(collection.Add);
        }

        public void Insert(HedgeCollateralView_AlanPledger obj)
        {
            throw new NotImplementedException();
        }

        public void Update(HedgeCollateralView_AlanPledger obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(HedgeCollateralView_AlanPledger obj)
        {
            throw new NotImplementedException();
        }
    }
}
