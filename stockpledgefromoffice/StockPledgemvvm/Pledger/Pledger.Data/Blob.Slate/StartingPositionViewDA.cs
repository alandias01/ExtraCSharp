using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
namespace Pledger.Data.Blob.Slate
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
    }
}
