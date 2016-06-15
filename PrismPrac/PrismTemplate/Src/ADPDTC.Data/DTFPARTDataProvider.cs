using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADPDTC.Data
{
    public class DTFPARTDataProvider : IDataProvider<DTFPART>
    {
        ADPDTCEntities AE = new ADPDTCEntities();

        public void Load(out DTFPART obj, int Id)
        {
            obj = AE.DTFPARTs.FirstOrDefault(x => x.Id == Id);
        }

        public void Load(ICollection<DTFPART> collection)
        {
            AE.DTFPARTs.ToList().ForEach(collection.Add);

        }

        public void Load(ICollection<DTFPART> collection, DateTime dateOfData)
        {            
            AE.DTFPARTs.Where(x => x.DateofData == dateOfData).ToList().ForEach(collection.Add);
        }

        public void Insert(DTFPART obj)
        {
            AE.DTFPARTs.AddObject(obj);
            AE.SaveChanges();
        }

        public void Update(DTFPART obj)
        {
            if (AE.DTFPARTs.Any(x => x.Id == obj.Id))
            {
                AE.SaveChanges();
            }
        }

        public void Delete(DTFPART obj)
        {
            AE.DTFPARTs.DeleteObject(obj);
        }

        
    }
}
