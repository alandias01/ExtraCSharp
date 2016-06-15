using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFUtils;

namespace Data.Slate
{
    public class PledgeStatusDA : IDataProvider<PledgeStatus>
    {
        SlateEntities se = new SlateEntities();

        public void Load(out PledgeStatus obj, int Id)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<PledgeStatus> collection)
        {
            se.PledgeStatus.ToList().ForEach(collection.Add);
        }

        public void Load(ICollection<PledgeStatus> collection, DateTime dateOfData)
        {
            throw new NotImplementedException();
        }

        public void Insert(PledgeStatus obj)
        {
            throw new NotImplementedException();
        }

        public void Update(PledgeStatus obj)
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

        public void Delete(PledgeStatus obj)
        {
            throw new NotImplementedException();
        }
    }
}
