using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFUtils;
namespace Data.DTC
{
    public class spAlanGetRealTimePositions_Pledger_ResultDA : IDataProvider<spAlanGetRealTimePositions_Pledger_Result>
    {

        public void Load(out spAlanGetRealTimePositions_Pledger_Result obj, int Id)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<spAlanGetRealTimePositions_Pledger_Result> collection)
        {
            new DTCEntities().spAlanGetRealTimePositions_Pledger("0269").ToList().ForEach(collection.Add);
        }

        public void Load(ICollection<spAlanGetRealTimePositions_Pledger_Result> collection, DateTime dateOfData)
        {
            throw new NotImplementedException();
        }

        public void Insert(spAlanGetRealTimePositions_Pledger_Result obj)
        {
            throw new NotImplementedException();
        }

        public void Update(spAlanGetRealTimePositions_Pledger_Result obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(spAlanGetRealTimePositions_Pledger_Result obj)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<spAlanGetRealTimePositions_Pledger_Result> collection, string book)
        {            
            new DTCEntities().spAlanGetRealTimePositions_Pledger(book).ToList().ForEach(collection.Add);
        }

    }
}
