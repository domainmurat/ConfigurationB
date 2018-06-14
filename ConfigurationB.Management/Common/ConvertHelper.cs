using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationB.Management.Common
{
    public static class ConvertHelper
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static T ChangeType<T>(this object o)
        {
            Type conversionType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            return (T)Convert.ChangeType(o, conversionType);
        }

        public static Boolean IsNumeric(this Object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }


        public static T TryConvertTo<T>(this Object Expression, T defaultValue)
        {
            if (Expression == null)
                return defaultValue;

            String nullable = "Nullable`1";

            var fullName = typeof(T).FullName;
            var name = typeof(T).Name;
            Object result = defaultValue;

            try
            {
                if (name == "Decimal" || (name == nullable && fullName.Contains("System.Decimal")))
                {
                    if (IsNumeric(Expression))
                    {
                        result = Convert.ToDecimal(Expression.ToString());
                    }
                }
                else if (name == "Double" || (name == nullable && fullName.Contains("System.Decimal")))
                {
                    if (IsNumeric(Expression))
                    {
                        result = Convert.ToDouble(Expression.ToString());
                    }
                }
                else if (name == "Int16" || (name == nullable && fullName.Contains("System.Int16")))
                {
                    if (IsNumeric(Expression))
                    {
                        result = Convert.ToInt16(Expression.ToString());
                    }
                }
                else if (name == "Int32" || (name == nullable && fullName.Contains("System.Int32")))
                {
                    if (IsNumeric(Expression))
                    {
                        result = Convert.ToInt32(Expression.ToString());
                    }
                }
                else if (name == "Int64" || (name == nullable && fullName.Contains("System.Int64")))
                {
                    if (IsNumeric(Expression))
                    {
                        result = Convert.ToInt64(Expression.ToString());
                    }
                }
                else if (name == "UInt16" || (name == nullable && fullName.Contains("System.UInt16")))
                {
                    if (IsNumeric(Expression))
                    {
                        result = Convert.ToUInt16(Expression.ToString());
                    }
                }
                else if (name == "UInt32" || (name == nullable && fullName.Contains("System.UInt32")))
                {
                    if (IsNumeric(Expression))
                    {
                        result = Convert.ToUInt32(Expression.ToString());
                    }
                }
                else if (name == "UInt64" || (name == nullable && fullName.Contains("System.UInt64")))
                {
                    if (IsNumeric(Expression))
                    {
                        result = Convert.ToUInt64(Expression.ToString());
                    }
                }
                else if (name == "Single" || (name == nullable && fullName.Contains("System.Single")))
                {
                    if (IsNumeric(Expression))
                    {
                        result = Convert.ToSingle(Expression.ToString());
                    }
                }
                else if (name == "Byte" || (name == nullable && fullName.Contains("System.Byte")))
                {
                    Byte temp;
                    Byte.TryParse(Expression.ToString(), out temp);
                    result = temp;
                }
                else if (name == "DateTime" || (name == nullable && fullName.Contains("System.DateTime")))
                {
                    DateTime temp;
                    DateTime.TryParse(Expression.ToString(), out temp);
                    result = temp;
                }
                else if (name == "Boolean" || (name == nullable && fullName.Contains("System.Boolean")))
                {
                    Boolean temp;
                    Boolean.TryParse(Expression.ToString(), out temp);
                    result = temp;
                }
                else if (name == "String" || (name == nullable && fullName.Contains("System.String")))
                {
                    String temp = Convert.ToString(Expression);
                    if (!String.IsNullOrEmpty(temp))
                        result = temp;
                }
            }
            catch
            {
                result = defaultValue;
            }

            return (T)result;
        }
    }
}
