using DataAccessLayer.Repositories;
using Models;

namespace DataAccessLayer.UnitOfWork
{
    public interface IUnitOfWork
    { 
        IRepository<Customer> Customers { get; }
        IRepository<Order> Orders { get; }
        IRepository<Product> Products { get; }
    }
}