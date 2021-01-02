using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Loggers;
using Models;

namespace DataAccessLayer.Repositories
{
    internal class ProductRepository : IRepository<Product>
    {
        public readonly string ConnectionString;
        public ProductRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        public void Create(Product item) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();

        public async Task<Product> Get(int id)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    var command = new SqlCommand("GetProductByID", connection)
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
                                var product = new Product
                                {
                                    Id = id,
                                    Name = reader.GetString(1),
                                };
                                return product;
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
        public void Update(Product item) => throw new NotImplementedException();
        Task<IEnumerable<Product>> IRepository<Product>.GetAll() => throw new NotImplementedException();
    }
}
