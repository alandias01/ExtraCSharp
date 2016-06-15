using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NorthWind.Data;
using System.Collections.ObjectModel;
using NorthWind.Application;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace NorthWind.ViewModel
{
    public class MainWindowViewModel
    {
        private readonly IUIDataProvider _dataProvider;
        public ObservableCollection<ToolViewModel> Tools { get; set; }
        
        
        private Command _showDetailsCommand;
        public Command ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new Command(ShowCustomerDetails, IsCustomerSelected));
            }
        }

        private string _selectedCustomerID;
        public string SelectedCustomerID
        {
            get { return _selectedCustomerID; }
            set
            {
                _selectedCustomerID = value;
                ShowDetailsCommand.RaiseCanExecuteChanged();
            }
        }

        public string Name
        {
            get
            {
                return "Northwind";
            }
        }

        public string ControlPanelName
        {
            get
            {
                return "Control Panel";
            }
        }

        private IList<Customer> _customers;
        public IList<Customer> Customers
        {
            get
            {
                if (_customers == null)
                {
                    GetCustomers();
                }
                return _customers;
            }
        }

        public MainWindowViewModel(IUIDataProvider dataProvider)
        {            
            _dataProvider = dataProvider;
            Tools = new ObservableCollection<ToolViewModel>();
            Tools.Add(new CustomerDetailsViewModel(_dataProvider, "ALFKI"));            
        }


        private void GetCustomers()
        {
            _customers = _dataProvider.GetCustomers();
        }

        public void ShowCustomerDetails(object o)
        {
            if (!IsCustomerSelected(o))
                throw new InvalidOperationException("Unable to show customer because no customer is selected.");
            
            CustomerDetailsViewModel customerDetailsViewModel = GetCustomerDetailsTool(SelectedCustomerID);

            if (customerDetailsViewModel == null)
            {
                customerDetailsViewModel = new CustomerDetailsViewModel(_dataProvider, SelectedCustomerID);
                Tools.Add(customerDetailsViewModel);
            }
            SetCurrentTool(customerDetailsViewModel);
        }

        public bool IsCustomerSelected(object o)
        {
            return !string.IsNullOrEmpty(SelectedCustomerID);
        }

        private CustomerDetailsViewModel GetCustomerDetailsTool(string customerID)
        {
            return Tools.OfType<CustomerDetailsViewModel>().FirstOrDefault(c => c.Customer.CustomerID == customerID);
        }

        private void SetCurrentTool(ToolViewModel currentTool)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Tools);
            if (collectionView != null)
            {
                if (collectionView.MoveCurrentTo(currentTool) != true)
                {
                    throw new InvalidOperationException("Could not find the current tool.");
                }
            }
        }


    }
}
