using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADPDTC.Data
{
    public class PARTPODataProvider
    {
        ADPDTCEntities AE = new ADPDTCEntities();

        public void Load(out PARTPO obj, int Id)
        {
            obj = AE.PARTPOes.FirstOrDefault(x => x.Id == Id);
        }

        public void Load(ICollection<PARTPO> collection)
        {
            AE.PARTPOes.ToList().ForEach(collection.Add);

        }

        public void Load(ICollection<PARTPO> collection, DateTime dateOfData)
        {
            AE.PARTPOes.Where(x => x.DateofData == dateOfData).ToList().ForEach(collection.Add);
        }

        public void Insert(PARTPO obj)
        {
            AE.PARTPOes.AddObject(obj);
            AE.SaveChanges();
        }

        public void Update(PARTPO obj)
        {
            if (AE.PARTPOes.Any(x => x.Id == obj.Id))
            {
                AE.SaveChanges();
            }
        }

        public void Delete(PARTPO obj)
        {
            AE.PARTPOes.DeleteObject(obj);
        }
    }
}
