using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace AList
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FormatAttribute : Attribute
    {
        public string Str { get; set; }

        public FormatAttribute(string str)
        {
            Str = str;
        }
    }

    public class CollectionPretifier
    {
        public string Str { get; set; }
        
        public CollectionPretifier(IEnumerable list)
        {
            Str = ListFormat(list);
        }
        
        string ListFormat(IEnumerable list)
        {
            Type type = list.GetType();
            PropertyInfo[] props = type.GetProperties();

            StringBuilder sb = new StringBuilder();

            foreach (var prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    if (attr is FormatAttribute)
                    {
                        sb.Append($"{prop.Name}: {prop.GetValue(list)}\n");
                    }
                    else
                    {
                        sb.Append($"{prop.Name}\n");
                    }
                }
            }

            return sb.ToString();
        }
    }
}