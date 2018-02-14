using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using YoutubeExplode.Models;

namespace YoutubeDown
{
    public static class Extensions
    {
        public static int CompareTo(this Video x, Video y, Func<Video, IComparable> func)
        {
            return func.Invoke(x).CompareTo(func.Invoke(y));
        }

        public static void Invoke(this Control control, MethodInvoker methodInvoker)
        {
            control.Invoke(methodInvoker);
        }

        // https://stackoverflow.com/questions/564366/convert-generic-list-enumerable-to-datatable
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in list)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }
    }
}
