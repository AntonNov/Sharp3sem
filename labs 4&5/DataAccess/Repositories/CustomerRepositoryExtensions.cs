using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using DataAccess.Loggers;

namespace DataAccessLayer.Repositories
{
    internal static class CustomerRepositoryExtensions
    {
        public static DataTable CustomerSummaryByName(this CustomerRepository repository, string firstName, string lastName)
        {
            using (var connection = new SqlConnection(repository.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var command = new SqlCommand("CustomerSummaryByName", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Transaction = transaction;
                DataTable customer;
                try
                {
                    command.Parameters.Add(new SqlParameter("@firstName", firstName));
                    command.Parameters.Add(new SqlParameter("@lastName", lastName));
                    var adapter = new SqlDataAdapter(command);
                    customer = new DataTable("Customer");
                    adapter.Fill(customer);
                    transaction.Commit();
                    return customer;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ILogger logger = new Logger();
                    logger.WriteErrorToDB(e.Message);
                }

                throw new KeyNotFoundException();
            }
        }
    }
}
