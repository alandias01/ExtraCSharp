using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NorthWind.Data;
using NorthWind.Application;
using System.Windows.Input;
using System.ComponentModel;

namespace NorthWind.ViewModel
{
    public class CustomerDetailsViewModel : ToolViewModel
    {
        private readonly IUIDataProvider _dataProvider;
        public Customer Customer { get; set; }

        private bool _isDirty;
        private Command _updateCommand;
        public Command UpdateCommand
        {
            get
            {
                return _updateCommand ?? (_updateCommand = new Command(UpdateCustomer, CanUpdateCustomer));
            }
        }
        private bool CanUpdateCustomer(object o)
        {
            return _isDirty;
        }
        private void UpdateCustomer(object o)
        {
            _dataProvider.Update(Customer);
        }

        public CustomerDetailsViewModel(IUIDataProvider dataProvider, string customerID)
        {
            _dataProvider = dataProvider;
            Customer = _dataProvider.GetCustomer(customerID);
            Customer.PropertyChanged += (Customer_PropertyChanged);
            DisplayName = Customer.CompanyName;
        }

        
        void Customer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _isDirty = true;
            UpdateCommand.RaiseCanExecuteChanged();
        }


    }
}
