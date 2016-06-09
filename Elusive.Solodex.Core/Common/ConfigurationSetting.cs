using System;
using System.Globalization;
using Elusive.Solodex.Core.Enumerations;

namespace Elusive.Solodex.Core.Common
{
    /// <summary>
    /// Support class to represent an app setting and convert it as needed
    /// thereby facilitating storage as an object.
    /// </summary>
    public class ConfigurationSetting
    {
        private readonly Type _valueType;

        private object _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSetting"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public ConfigurationSetting(ConfigurationKeysEnum key)
        {
            Key = key;
            var fieldInfo = typeof(ConfigurationKeysEnum).GetField(key.ToString());
            var attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DefaultAttribute)) as DefaultAttribute;

            if (attr != null)
            {
                _valueType = attr.Type;
                _value = ConvertFromDefault(attr.DefaultValue);
            }
            else
            {
                _valueType = typeof(string);
                Value = string.Empty;
            }
        }

        /// <summary>
        /// Gets the key enum for this setting.
        /// </summary>
        public ConfigurationKeysEnum Key { get; private set; }

        /// <summary>
        /// Gets or sets the value of the setting.
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// Converts from application setting.
        /// </summary>
        /// <param name="text">The text.</param>
        public void ConvertFromApplicationSetting(string text)
        {
            if (text == null) return;

            if (_valueType == null) return;

            if (_valueType.IsEnum)
            {
                try
                {
                    _value = Enum.Parse(_valueType, text);
                }
                catch (Exception)
                {
                    _value = null;
                }
            }
            else if (_valueType == typeof(int))
            {
                _value = ParseInt(text, CultureInfo.InvariantCulture);
            }
            else if (_valueType == typeof(bool))
            {
                _value = ParseBool(text, CultureInfo.InvariantCulture);
            }
            else if (_valueType == typeof(DateTimeOffset?))
            {
                _value = ParseDateTimeOffset(text, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Converts to application setting string value.
        /// </summary>
        /// <returns>string setting value.</returns>
        public string ConvertToApplicationSetting()
        {
            var returnValue = string.Empty;

            if (_value == null) return returnValue;
            returnValue = _value.ToString();

            if (_valueType == typeof(Enum))
            {
                returnValue = ((Enum)_value).ToString();
            }
            else if (_valueType == typeof(bool))
            {
                returnValue = ((bool)_value).ToString();
            }

            return returnValue;
        }

        /// <summary>
        /// Converts from default value set by attribute.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Converted default value</returns>
        private object ConvertFromDefault(object defaultValue)
        {
            var returnObject = defaultValue;

            if (_valueType.IsEnum)
            {
                try
                {
                    returnObject = Enum.Parse(_valueType, defaultValue.ToString());
                }
                catch (ArgumentException)
                {
                    returnObject = null;
                }
            }
            else if (_valueType == typeof(Guid))
            {
                returnObject = Guid.NewGuid().ToString();
            }
            else if (_valueType == typeof(DateTimeOffset))
            {
                returnObject = DateTimeOffset.Now;
            }

            return returnObject;
        }

        private static int ParseInt(string value, CultureInfo culture)
        {
            int outValue;
            return int.TryParse(value, NumberStyles.Integer, culture, out outValue) ? outValue : 0;
        }

        private static bool ParseBool(string value, CultureInfo culture)
        {
            bool outValue;
            return bool.TryParse(value, out outValue) && outValue;
        }

        private static DateTimeOffset? ParseDateTimeOffset(string value, CultureInfo culture)
        {
            DateTimeOffset outValue;
            if (DateTimeOffset.TryParse(value, culture, DateTimeStyles.None, out outValue))
            {
                return outValue;
            }
            return null;
        }
    }
}
