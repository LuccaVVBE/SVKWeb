using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Web;

namespace Svk.Client.Extensions;

public static class ObjectExtensions
{
    public static string AsQueryString<T>(this T obj)
    {
        var properties = from p in typeof(T).GetProperties()
            where p.GetValue(obj, null) != null
            select GetValueString(obj, p);

        return string.Join("&", properties.ToArray());
    }

    private static string GetValueString<T>(T obj, PropertyInfo property)
    {
        object value = property.GetValue(obj, null)!;

        if (value is DateTime dateTime)
        {
            // Convert DateTime to UTC string
            return property.Name + "=" +
                   HttpUtility.UrlEncode(dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
        else
        {
            return property.Name + "=" + HttpUtility.UrlEncode(value.ToString());
        }
    }

    public static T FromQueryString<T>(this T obj, string queryString)
    {
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var queryStringParam = GetQueryStringParameter(queryString, property.Name);

            if (queryStringParam != null)
            {
                SetValueFromQueryString(obj, property, queryStringParam);
            }
        }

        return obj;
    }


    private static string GetQueryStringParameter(string queryString, string paramName)
    {
        var decodedQueryString = HttpUtility.UrlDecode(queryString);
        var parameters = HttpUtility.ParseQueryString(decodedQueryString);
        return parameters[paramName];
    }

    private static void SetValueFromQueryString<T>(T obj, PropertyInfo property, string queryStringParam)
    {
        if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
        {
            // Convert the UTC string back to DateTime
            var dateTimeValue = DateTime.ParseExact(queryStringParam, "yyyy-MM-ddTHH:mm:ssZ",
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            property.SetValue(obj, dateTimeValue);
        }
        else if (Nullable.GetUnderlyingType(property.PropertyType) != null)
        {
            TypeConverter conv = TypeDescriptor.GetConverter(property.PropertyType);
            property.SetValue(obj, conv.ConvertFromInvariantString(queryStringParam));
        }
        else
        {
            // Set the property value from the query string parameter
            var convertedValue = Convert.ChangeType(queryStringParam, property.PropertyType);
            property.SetValue(obj, convertedValue);
        }
    }
}