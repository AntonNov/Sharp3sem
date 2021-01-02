namespace ConfigurationManager
{
    interface IParser<TOptionsModel> where TOptionsModel : class, new()
    {
        string Extension { get; }
        TOptionsModel Parse(string path);
        void Serialize(TOptionsModel configureObject, string path);
    }
}