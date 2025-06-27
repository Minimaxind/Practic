using Npgsql;
using practic.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace practic.Repositories
{
    public class CustomerRepository
    {
        private string connectionString;

        public CustomerRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM productstores.customers", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                CustomerId = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Address = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Phone = reader.IsDBNull(3) ? null : reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return customers;
        }

        public void AddCustomer(Customer customer)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "INSERT INTO productstores.customers (full_name, address, phone) VALUES (@fullName, @address, @phone)",
                    connection))
                {
                    command.Parameters.AddWithValue("@fullName", customer.FullName);
                    command.Parameters.AddWithValue("@address", (object)customer.Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@phone", (object)customer.Phone ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "UPDATE productstores.customers SET full_name = @fullName, address = @address, phone = @phone WHERE customer_id = @id",
                    connection))
                {
                    command.Parameters.AddWithValue("@id", customer.CustomerId);
                    command.Parameters.AddWithValue("@fullName", customer.FullName);
                    command.Parameters.AddWithValue("@address", (object)customer.Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@phone", (object)customer.Phone ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCustomer(int customerId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "DELETE FROM productstores.customers WHERE customer_id = @id",
                    connection))
                {
                    command.Parameters.AddWithValue("@id", customerId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetOverdueOrders(int customerId)
        {
            var table = new DataTable();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(@"
            SELECT r.receipt_id, r.purchase_date, r.total_amount, 
                   e.full_name as employee_name, s.store_name
            FROM productstores.receipts r
            JOIN productstores.employees e ON r.employee_id = e.employee_id
            JOIN productstores.stores s ON e.store_id = s.store_id
            WHERE r.customer_id = @customerId 
            AND r.purchase_date < CURRENT_DATE - INTERVAL '7 days'", connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        adapter.Fill(table);
                    }
                }
            }
            return table;
        }
    }


}