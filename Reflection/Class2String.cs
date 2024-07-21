using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReflectionHomeWork
{
    /// <summary>
    /// сериализатор-десериализатор для классов, лежащих в этом же намспейсе
    /// для любых классов - не подходит, т.к. для сложных классов принцип - строка на экземпляр - может подойти ,
    /// например, если сериализовать в json или в xml, 
    /// а потом то, что получилось минимизировать и записать в строку. 
    /// Но это противоречит духу задания
    /// поэтому - только классы с пустым конструктором, только простые типы и никаких коллекций
    /// </summary>
    public class Class2String
    {
        const string FIELD = "F";
        const string PROPERTIES = "P";
        const string NULL = "[NULL]";
        public object Deserialize(string value)
        {

            string[] classSceleton = Regex.Split(value, @"(?<!\?),");
            var className = classSceleton[0];

            Type type = Type.GetType(className);
            object o = Activator.CreateInstance(type);
            for (var i = 1; i < classSceleton.Length; i+=3)
            {
                string prefix = classSceleton[i]??string.Empty;
                string name = classSceleton [i+1];
                string strValue = NULL.Equals(classSceleton[i + 2]) ? null : classSceleton[i + 2].Replace("?,", ",");

                switch (prefix)
                {
                case FIELD:
                    FieldInfo fInfo = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    fInfo.SetValue(o, (strValue == null)?null:Convert.ChangeType(strValue, fInfo.FieldType));
                    break;
                case PROPERTIES:
                        PropertyInfo pInfo = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        pInfo.SetValue(o, (strValue == null) ? null : Convert.ChangeType(strValue, pInfo.PropertyType), null);
                break;
                default:
                    throw new Exception($"Wrong prefix: {prefix}");
                }                

            }
            return o;
        }
        /// <summary>
        /// простой сериализатор для класов с пустым конструктором и полями/свойствами из простых типов
        /// класс- в строку, разделитель ",".Формат, тип, пары ключ-значение. ключ,значение ,   Формат ключа  префмкс( F - поля, P - свойства),тип,имя
        /// Пример:
        /// MyClass,F,i1,1,F,i2,2,F,i3,3,F,i4,4,F,i5,5,P,i6,99"
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Setialize(object obj)
        {
            var stringBuilder = new StringBuilder();
            var rezult = string.Empty;
            var objType = obj.GetType();
            stringBuilder.Append(objType.FullName);
            var objFields = objType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var prefix = FIELD;
            foreach (var field in objFields)
            {
                stringBuilder.Append(",");
                AddString(stringBuilder,prefix,  field.Name, field.GetValue(obj));
            }
            prefix = PROPERTIES;

            var objProperties = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var prop in objProperties)
            {
                stringBuilder.Append(",");
                AddString(stringBuilder,prefix,  prop.Name, prop.GetValue(obj));
            }

            return stringBuilder.ToString();
        }
        private void AddString(StringBuilder stringBuilder, string prefix, string key, object? value)
        {
            var strValue = value == null ? null : value.ToString();
            stringBuilder.Append(prefix);
            stringBuilder.Append(",");
            stringBuilder.Append(key);
            stringBuilder.Append(",");
            if (strValue != null)
            {
                if (strValue.Contains(","))
                {
                    strValue = strValue.Replace(",", "?,");
                }

                stringBuilder.Append(strValue);
            }
            else
            {
                stringBuilder.Append(NULL);
            }
        }
    }
}
