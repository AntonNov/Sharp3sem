using System;
using System.Data;
using System.Data.SqlClient;
using DataAccess.Options;

namespace DataAccess.Loggers
{
    internal class Logger : ILogger
    {
        public void WriteErrorToDB(string massage)
        {
            using (var connection = new SqlConnection(OptionsLib.ConnectionStrings.ConnectionStringToErrorLogs))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var command = new SqlCommand("AddError", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Transaction = transaction;

                try
                {
                    command.Parameters.Add(new SqlParameter("@Error", massage));
                    command.Parameters.Add(new SqlParameter("@Time", DateTime.Now.ToString("s")));
                    var reader = command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}