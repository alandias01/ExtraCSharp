using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFUtils;
using System.Data.Objects;

namespace Data.DTCInstructor
{
    public class OutgoingPledgeOrderDA:IDataProvider<OutgoingPledgeOrder>
    {
        DtcInstructorEntities DIE = new DtcInstructorEntities();
        
        public void Load(out OutgoingPledgeOrder obj, int Id)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<OutgoingPledgeOrder> collection)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<OutgoingPledgeOrder> collection, DateTime dateOfData)
        {
            DIE.OutgoingPledgeOrders.Where(x => EntityFunctions.TruncateTime(x.DateEntered) == dateOfData).ToList().ForEach(collection.Add);
        }

        public void Insert(OutgoingPledgeOrder obj)
        {
            try
            {
                DIE.OutgoingPledgeOrders.AddObject(obj);
                DIE.SaveChanges();
            }
            catch (Exception)
            {
                DIE.Detach(obj);
                throw;
            }
        }

        public void Update(OutgoingPledgeOrder obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(OutgoingPledgeOrder obj)
        {
            throw new NotImplementedException();
        }
    }
}
