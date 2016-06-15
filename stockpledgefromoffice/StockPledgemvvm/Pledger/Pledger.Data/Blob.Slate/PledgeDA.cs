using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Data.Objects;
namespace Pledger.Data.Blob.Slate
{
    public class PledgeDA : IDataProvider<Pledge>
    {
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
            throw new NotImplementedException();
        }

        public void Update(Pledge obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(Pledge obj)
        {
            throw new NotImplementedException();
        }


        public void LoadUnpledged(ICollection<Pledge> collection, DateTime dateOfData)
        {
            new SlateEntities().Pledges.Where(x => EntityFunctions.TruncateTime(x.DateEntered) == dateOfData && x.PledgeType.ToLower() == "u").ToList().ForEach(collection.Add);
        }

        public void LoadPledged(ICollection<Pledge> collection, DateTime dateOfData)
        {
            new SlateEntities().Pledges.Where(x => EntityFunctions.TruncateTime(x.DateEntered) == dateOfData && x.PledgeType.ToLower() == "p").ToList().ForEach(collection.Add);
        }


    }
}
