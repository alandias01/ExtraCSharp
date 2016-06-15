using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFUtils;

namespace Data.Slate
{
    public class HedgeMarkTotalViewDA : IDataProvider<HedgeMarkTotalView_AlanPledger>
    {
        SlateEntities se = new SlateEntities();

        public void Load(out HedgeMarkTotalView_AlanPledger obj, int Id)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<HedgeMarkTotalView_AlanPledger> collection)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<HedgeMarkTotalView_AlanPledger> collection, DateTime dateOfData)
        {
            //new SlateEntities().HedgeMarkTotalView_AlanPledger.Where(x => x.ActivityDate == dateOfData).ToList().ForEach(collection.Add);
            se.HedgeMarkTotalView_AlanPledger.Where(x => x.ActivityDate == dateOfData).ToList().ForEach(collection.Add);
        }

        public void Insert(HedgeMarkTotalView_AlanPledger obj)
        {
            throw new NotImplementedException();
        }

        public void Update(HedgeMarkTotalView_AlanPledger obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(HedgeMarkTotalView_AlanPledger obj)
        {
            throw new NotImplementedException();
        }
    }
}
