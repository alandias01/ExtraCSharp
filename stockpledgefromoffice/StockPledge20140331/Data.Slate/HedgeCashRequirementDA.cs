using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFUtils;

namespace Data.Slate
{
    public class HedgeCashRequirementDA : IDataProvider<hedgecashrequirementView_AlanPledger>
    {
        SlateEntities se = new SlateEntities();

        public void Load(out hedgecashrequirementView_AlanPledger obj, int Id)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<hedgecashrequirementView_AlanPledger> collection)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<hedgecashrequirementView_AlanPledger> collection, DateTime dateOfData)
        {
            se.hedgecashrequirementView_AlanPledger.Where(x => x.DateOfData == dateOfData).ToList().ForEach(collection.Add);
        }

        public void Insert(hedgecashrequirementView_AlanPledger obj)
        {
            try
            {
                se.hedgecashrequirementView_AlanPledger.AddObject(obj);
                se.SaveChanges();
            }
            catch (Exception)
            {
                se.Detach(obj);
                throw;
            }
        }

        public void Update(hedgecashrequirementView_AlanPledger obj)
        {
            try
            {
                se.SaveChanges();
            }
            catch (Exception)
            {
                se.Detach(obj);
                throw;
            }
        }

        public void Delete(hedgecashrequirementView_AlanPledger obj)
        {
            try
            {
                se.hedgecashrequirementView_AlanPledger.DeleteObject(obj);
                se.SaveChanges();
            }
            catch (Exception)
            {
                se.Detach(obj);
                throw;
            }
        }
    }
}
