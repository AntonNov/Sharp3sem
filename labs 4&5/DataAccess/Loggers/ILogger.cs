namespace DataAccess.Loggers
{
    internal interface ILogger
    {
        void WriteErrorToDB(string massage);
    }
}