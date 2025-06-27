using practic.Repositories;
using practic.ViewModels;
using System.Windows;

namespace practic
{
    public partial class MainWindow : Window
    {
        private CustomerViewModel _customerViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _customerViewModel = new CustomerViewModel();
            _customerViewModel.LoadCustomers();
            DataContext = _customerViewModel;
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var newCustomer = new Models.Customer
            {
                FullName = "Новый клиент",
                Address = "",
                Phone = ""
            };
            _customerViewModel.AddCustomer(newCustomer);
        }

        private void SaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (_customerViewModel.SelectedCustomer != null)
            {
                _customerViewModel.UpdateCustomer(_customerViewModel.SelectedCustomer);
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (_customerViewModel.SelectedCustomer != null)
            {
                _customerViewModel.DeleteCustomer(_customerViewModel.SelectedCustomer.CustomerId);
            }
        }

        private void FindOrders_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(CustomerIdTextBox.Text, out int customerId))
            {
                var repository = new CustomerRepository();
                OrdersDataGrid.ItemsSource = repository.GetOverdueOrders(customerId).DefaultView;
            }
            else
            {
                MessageBox.Show("Введите корректный ID клиента");
            }
        }


    }
}   