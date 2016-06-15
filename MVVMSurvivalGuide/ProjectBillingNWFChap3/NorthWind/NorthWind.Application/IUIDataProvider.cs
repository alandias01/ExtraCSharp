using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NorthWind.Data;

namespace NorthWind.Application
{
    public interface IUIDataProvider
    {
        IList<Customer> GetCustomers();
        Customer GetCustomer(string customerID);
    }
}
