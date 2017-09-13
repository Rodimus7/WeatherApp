using System;
using System.Linq;
using System.Reflection;

//http://taoffi.isosoft.org/post/2017/05/03/Xamarin-missing-Description-attribute

namespace WeatherApp
{
  [AttributeUsage(validOn: AttributeTargets.All, AllowMultiple = false, Inherited = false)]
  public class DescriptionAttribute : Attribute
  {
    public string _description;

    public DescriptionAttribute(string description)
    {
      _description = description;
    }

    public string Description
    {
      get { return _description; }
      set { _description = value; }
    }
  }

  public static class EnumDescriptions
  {
    public static string EnumDescription(this Enum value)
    {
      if (value == null)
        return null;

      var type = value.GetType();
      TypeInfo typeInfo = type.GetTypeInfo();
      string typeName = Enum.GetName(type, value);

      if (string.IsNullOrEmpty(typeName))
        return null;

      var field = typeInfo.DeclaredFields.FirstOrDefault(f => f.Name == typeName);

      if (field == null)
        return typeName;

      var attrib = field.GetCustomAttribute<DescriptionAttribute>();
      return attrib == null ? typeName : attrib.Description;
    }
  }
}
