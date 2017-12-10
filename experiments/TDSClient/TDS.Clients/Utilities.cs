using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.SqlServer.TDS.Clients
{
    /// <summary>
    /// Utility functions
    /// </summary>
    internal class Utilities
    {
        /// <summary>
        /// Log object content into console
        /// </summary>
        /// <param name="log">Destination</param>
        /// <param name="prefix">Prefix the output with</param>
        /// <param name="instance">Object to log</param>
        internal static void LogObject(TextWriter log, string prefix, object instance)
        {
            // Get object type
            Type objectType = instance.GetType();

            // Iterate all public properties
            foreach (PropertyInfo info in objectType.GetProperties())
            {
                // Get property value
                object value = info.GetValue(instance, null);

                // Check if property is special case
                if (info.PropertyType == typeof(TDS.Login7.TDSLogin7TokenOptionalFlags1)
                    || info.PropertyType == typeof(TDS.Login7.TDSLogin7TokenOptionalFlags2)
                    || info.PropertyType == typeof(TDS.Login7.TDSLogin7TokenOptionalFlags3)
                    || info.PropertyType == typeof(TDS.Login7.TDSLogin7TokenTypeFlags)
                    || info.PropertyType == typeof(TDS.AllHeaders.TDSAllHeadersToken)
                    || info.PropertyType == typeof(TDS.AllHeaders.TDSQueryNotificationsHeader)
                    || info.PropertyType == typeof(TDS.AllHeaders.TDSTransactionDescriptorHeader))
                {
                    // Log its content
                    LogObject(log, string.Format("{0}.{1}", prefix, info.Name), value);
                }
                else if (info.PropertyType.IsGenericType)  // IList<T>
                {
                    int index = 0;

                    // Log values
                    foreach (object o in (value as System.Collections.IEnumerable))
                    {
                        LogObject(log, string.Format("{0}.{1}[{2}]", prefix, info.Name, index++), o);
                    }
                }
                else if (info.PropertyType.IsArray)
                {
                    // Log prefix
                    log.Write("{0}.{1}.{2}: ", prefix, objectType.Name, info.Name);

                    // Log values
                    foreach (object o in (value as Array))
                    {
                        log.Write("{0} ", o);
                    }

                    // Move to the next line
                    log.WriteLine();
                }
                else
                {
                    // Log as is
                    log.WriteLine("{0}.{1}.{2}: {3}", prefix, objectType.Name, info.Name, value);
                }
            }
        }
    }
}
