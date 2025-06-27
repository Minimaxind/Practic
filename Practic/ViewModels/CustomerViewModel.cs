using practic.Models;
using practic.Repositories;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace practic.ViewModels
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private CustomerRepository _repository;
        private List<Customer> _customers;
        private Customer _selectedCustomer;

        public List<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged();
            }
        }

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
        }

        public CustomerViewModel()
        {
            _repository = new CustomerRepository();
        }

        public void LoadCustomers()
        {
            Customers = _repository.GetAllCustomers();
        }

        public void AddCustomer(Customer customer)
        {
            _repository.AddCustomer(customer);
            LoadCustomers();
        }

        public void UpdateCustomer(Customer customer)
        {
            _repository.UpdateCustomer(customer);
            LoadCustomers();
        }

        public void DeleteCustomer(int customerId)
        {
            _repository.DeleteCustomer(customerId);
            LoadCustomers();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}