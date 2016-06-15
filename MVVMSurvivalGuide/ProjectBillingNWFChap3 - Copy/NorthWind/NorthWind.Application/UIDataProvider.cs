using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NorthWind.Data;

namespace NorthWind.Application
{
    public class UIDataProvider : IUIDataProvider
    {
        private NORTHWINDEntities _northwindEntities = new NORTHWINDEntities();

        public IList<Data.Customer> GetCustomers()
        {
            return _northwindEntities.Customers.ToList();
        }

        public Customer GetCustomer(string customerID)
        {
            return _northwindEntities.Customers.Single(c => c.CustomerID == customerID);
        }
    }
}
