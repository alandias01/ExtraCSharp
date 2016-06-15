using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFUtils;
namespace Data.Slate
{
    public class StartingPositionViewDA : IDataProvider<StartingPositionView>
    {
        public void Load(out StartingPositionView obj, int Id)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<StartingPositionView> collection)
        {
            new SlateEntities().StartingPositionViews.ToList().ForEach(collection.Add);
        }

        public void Load(ICollection<StartingPositionView> collection, DateTime dateOfData)
        {
            throw new NotImplementedException();
        }

        public void Insert(StartingPositionView obj)
        {
            throw new NotImplementedException();
        }

        public void Update(StartingPositionView obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(StartingPositionView obj)
        {
            throw new NotImplementedException();
        }

        public void Load(ICollection<StartingPositionView> collection, string CounterpartyCode, string ClearingNo, string Cusip)
        {
            new SlateEntities().StartingPositionViews.Where(x => x.CounterpartyCode == CounterpartyCode && x.ClearingNo == ClearingNo && x.Cusip == Cusip).ToList().ForEach(collection.Add);
        }
    }
}
