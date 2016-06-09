using System;

namespace Elusive.Solodex.Core.Common
{
    public class DefaultAttribute : Attribute
    {
        public DefaultAttribute()
        {
            DefaultValue = string.Empty;
            Type = typeof (string);
        }

        public DefaultAttribute(object value)
        {
            DefaultValue = value;
            Type = typeof (string);
        }

        public DefaultAttribute(object value, Type type)
        {
            DefaultValue = value;
            Type = type;
        }

        public object DefaultValue { get; private set; }

        public Type Type { get; private set; }
    }
}