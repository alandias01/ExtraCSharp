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
        
        public void Update(Customer customer)
        {
            Data.Customer CustomerEntity = _northwindEntities.Customers.Single(c => c.CustomerID == customer.CustomerID);
            CustomerEntity.CompanyName = customer.CompanyName;
            CustomerEntity.ContactName = customer.ContactName;
            CustomerEntity.Address = customer.Address;
            CustomerEntity.City = customer.City;
            CustomerEntity.Country = customer.Country;
            CustomerEntity.Region = customer.Region;
            CustomerEntity.PostalCode = customer.PostalCode;
            CustomerEntity.Phone = customer.Phone;
            _northwindEntities.SaveChanges();            
        }
    }
}
