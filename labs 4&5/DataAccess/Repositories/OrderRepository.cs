using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Loggers;

namespace DataAccessLayer.Repositories
{
    internal class OrderRepository : IRepository<Order>
    {
        public readonly string ConnectionString;
        public OrderRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        public void Create(Order item) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();

        public async Task<Order> Get(int id)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    var command = new SqlCommand("GetOrderByID", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Transaction = transaction;

                    try
                    {
                        command.Parameters.Add(new SqlParameter("@ID", id));
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var order = new Order
                                {
                                    ID = id,
                                    Count = reader.GetInt16(0),
                                    UnitPrice = reader.GetDecimal(1),
                                    CustomerID = reader.GetInt32(2),
                                    ProductID = reader.GetInt32(3),
                                };
                                return order;
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        ILogger logger = new Logger();
                        logger.WriteErrorToDB(e.Message);
                    }
                }

                throw new KeyNotFoundException();
            });
        }
        public async Task<IEnumerable<Order>> GetAll()
        {
            return await Task.Run(() =>
            {
                var orders = new List<Order>();
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("GetOrders", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var order = new Order
                            {
                                ID = reader.GetInt32(0),
                                Count = reader.GetInt16(1),
                                UnitPrice = reader.GetDecimal(2),
                                CustomerID = reader.GetInt32(3),
                                ProductID = reader.GetInt32(4),
                            };
                            orders.Add(order);
                        }
                    }
                }

                return orders;
            });
        }
        public void Update(Order item) => throw new NotImplementedException();
    }
}
