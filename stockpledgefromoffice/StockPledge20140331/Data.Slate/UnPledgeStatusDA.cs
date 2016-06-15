using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFUtils;

namespace Data.Slate
{
    public class UnPledgeableDA : IDataProvider<UnPledgeableView_AlanPledger>
    {
        SlateEntities se = new SlateEntities();

        public void Load(out UnPledgeableView_AlanPledger obj, int Id)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<UnPledgeableView_AlanPledger> collection)
        {
            se.UnPledgeableView_AlanPledger.ToList().ForEach(collection.Add);
        }

        public void Load(ICollection<UnPledgeableView_AlanPledger> collection, DateTime dateOfData)
        {
            throw new NotImplementedException();
        }

        public void Insert(UnPledgeableView_AlanPledger obj)
        {
            throw new NotImplementedException();
        }

        public void Update(UnPledgeableView_AlanPledger obj)
        {
            throw new NotImplementedException();
            /*
            try
            {
                se.SaveChanges();
            }
            catch (Exception)
            {
                se.Detach(obj);
                throw;
            }
            */
        }

        public void Delete(UnPledgeableView_AlanPledger obj)
        {
            throw new NotImplementedException();
        }
    }
}
