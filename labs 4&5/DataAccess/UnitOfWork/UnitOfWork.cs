using ConfigurationManager;
using DataAccess.Options;
using DataAccessLayer.Repositories;
using Models;

namespace DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly string connectionString;
        private CustomerRepository customerRepository;
        private ProductRepository productRepository;
        private OrderRepository orderRepository;
        public UnitOfWork()
        {
            var optionsManager = new OptionsManager();
            OptionsLib.ConnectionStrings = optionsManager.GetConfiguredOptionsModel<ConnectionStrings>();

            connectionString = OptionsLib.ConnectionStrings.ConnectionStringToAdventureWorksLT2019;
        }

        public IRepository<Customer> Customers => 
            customerRepository ?? (customerRepository = new CustomerRepository(connectionString));

        public IRepository<Order> Orders => 
            orderRepository ?? (orderRepository = new OrderRepository(connectionString));

        public IRepository<Product> Products => 
            productRepository ?? (productRepository = new ProductRepository(connectionString));
    }
}