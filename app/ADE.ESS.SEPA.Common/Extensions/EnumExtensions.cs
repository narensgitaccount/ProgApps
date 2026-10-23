using System.ComponentModel;
using System.Reflection;

namespace ADE.ESS.SEPA.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// get the description attribute value of an Enumerable
        /// </summary>
        /// <param name="en">
        /// the Enumerable
        /// </param>
        /// <returns>
        /// the description value if found, otherwise returns the Enumerable.ToString value 
        /// </returns>
        public static string GetDescription(this Enum en)
        {
            var type = en.GetType();

            var memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        public static string GetDescriptionFromValue(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }


        public static T GetEnumValueFromDescription<T>(string description) where T : Enum
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();

            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        public static bool In<T>(this T val, params T[] values) where T : Enum
        {
            return values.Contains(val);
        }

        public static List<T> GetAllEnumsAsList<T>() where T : Enum
        {
            var list = new List<T>();
            foreach (T i in Enum.GetValues(typeof(T)))
            {
                list.Add(i);
            }

            return list;
        }

        public static bool TryParseEnumDescription<TEnum>(string description, out TEnum result) where TEnum : struct, Enum
        {
            foreach (var field in typeof(TEnum).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                    {
                        result = (TEnum)field.GetValue(null);
                        return true;
                    }
                }
                else if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    result = (TEnum)field.GetValue(null);
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static IEnumerable<TEnum> GetValuesInDeclarationOrder<TEnum>() where TEnum : Enum
        {
            return typeof(TEnum)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .OrderBy(f => f.MetadataToken)
                .Select(f => (TEnum)f.GetValue(null));
        }

    }
}
