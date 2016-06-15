using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADPDTC.Data
{
    public interface IDataProvider<T>
    {        
        void Load(out T obj, int Id);
        void Load(ICollection<T> collection);
        void Load(ICollection<T> collection, DateTime dateOfData );                
        void Insert(T obj);
        void Update(T obj);
        void Delete(T obj);
    }
}
