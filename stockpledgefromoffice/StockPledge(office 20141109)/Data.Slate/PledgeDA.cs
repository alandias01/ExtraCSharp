using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFUtils;
using System.Data.Objects;
namespace Data.Slate
{
    public class PledgeDA : IDataProvider<Pledge>
    {
        SlateEntities se = new SlateEntities();

        public void Load(out Pledge obj, int Id)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<Pledge> collection)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<Pledge> collection, DateTime dateOfData)
        {
            throw new NotImplementedException();
        }
        
        public void Insert(Pledge obj)
        {
            try
            {
                se.Pledges.AddObject(obj);
                se.SaveChanges();
            }
            catch (Exception)
            {
                se.Detach(obj);
                throw;
            }
        }

        public void Update(Pledge obj)
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

        public void Delete(Pledge obj)
        {
            throw new NotImplementedException();
        }


        public void LoadUnpledged(ICollection<Pledge> collection, DateTime dateOfData)
        {
            se.Pledges.Where(x => EntityFunctions.TruncateTime(x.DateEntered) == dateOfData && x.PledgeType.ToLower() == "u").ToList().ForEach(collection.Add);
        }

        public void LoadPledged(ICollection<Pledge> collection, DateTime dateOfData)
        {
            se.Pledges.Where(x => EntityFunctions.TruncateTime(x.DateEntered) == dateOfData && x.PledgeType.ToLower() == "p").ToList().ForEach(collection.Add);
        }


    }
}
