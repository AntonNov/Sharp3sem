using System;

namespace ConfigurationManager
{
    public class NameAttribute : Attribute
    {
        public string Name { get; private set; }
        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}