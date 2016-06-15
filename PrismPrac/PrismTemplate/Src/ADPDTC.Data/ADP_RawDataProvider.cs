using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ADPDTC.Data
{
    public class ADP_RawDataProvider : IDataProvider<ADP_Raw>
    {
        ADPDTCEntities AE = new ADPDTCEntities();
                
        public void Load(out ADP_Raw obj, int Id)
        {
            obj = AE.ADP_Raw.FirstOrDefault(x => x.Tran_Id == Id);
        }

        public void Load(ICollection<ADP_Raw> collection)
        {            
            AE.ADP_Raw.ToList().ForEach(collection.Add);
        }

        public void Load(ICollection<ADP_Raw> collection, DateTime dateOfData)
        {
            AE.ADP_Raw.Where(x => x.Process_Date == dateOfData).ToList().ForEach(collection.Add);
        }
        
        public void Insert(ADP_Raw obj)
        {
            AE.ADP_Raw.AddObject(obj);
        }

        public void Update(ADP_Raw obj)
        {
            if (AE.ADP_Raw.Any(x => x.Tran_Id == obj.Tran_Id))
            {
                AE.SaveChanges();
            }
        }

        public void Delete(ADP_Raw obj)
        {
            AE.ADP_Raw.DeleteObject(obj);
        }

        
    }
}
