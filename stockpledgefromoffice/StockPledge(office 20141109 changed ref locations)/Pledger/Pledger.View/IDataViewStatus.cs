using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pledger.View
{
    public interface IDataViewStatus<TG, BG>
    {
        ICollection<TG> GetTopGrid();
        ICollection<BG> GetBottomGrid();
        string GetTopTotal();
        string GetBottomTotal();
    }
}
