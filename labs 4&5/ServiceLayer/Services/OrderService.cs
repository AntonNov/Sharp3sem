using System;
using Models;
using DataAccessLayer.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork Database { get; set; }

        public OrderService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        private async Task<IEnumerable<Order>> GetOrders() => await Database.Orders.GetAll();
        private async Task<Customer> GetCustomer(int id) => await Database.Customers.Get(id);
        private async Task<Product> GetProduct(int id) => await Database.Products.Get(id);
        public async Task<IEnumerable<OrderInfo>> GetOrdersInfo()
        {
            return await Task.Run(async () =>
            {
                Func<Order, Task<Customer>> getCustomer = async (order) => await GetCustomer(order.CustomerID);
                Func<Order, Task<Product>> getProduct = async (order) => await GetProduct(order.CustomerID);
                var orders = await GetOrders();
                return (from order in orders
                    let customer = getCustomer.Invoke(order).Result
                    let product = getProduct.Invoke(order).Result
                    select new OrderInfo()
                    {
                        ProductName = product.Name,
                        CustomerFirstName = customer.FirstName,
                        CustomerLastName = customer.LastName,
                        CustomerEmail = customer.EmailAddress,
                        CustomerAddress = customer.City + customer.Address,
                        CustomerPhone = customer.Phone,
                    });
            });
        }
    }
}