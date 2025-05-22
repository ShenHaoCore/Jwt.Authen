using System.Reflection;

namespace Jwt.Authen.Api.Commons;

/// <summary>
/// 
/// </summary>
public static class ApiVersionHelper
{
    /// <summary>
    /// 获取指定类中的所有版本常量（const string 字段值）
    /// </summary>
    /// <param name="type">包含版本常量的类型</param>
    /// <returns>版本号字符串集合</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static IEnumerable<string> GetApiVersions(Type type)
    {
        if (type == null) { throw new ArgumentNullException(nameof(type)); }
        if (!type.IsClass && !type.IsInterface) { throw new ArgumentException("类型必须是类或接口", nameof(type)); }
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
        foreach (FieldInfo field in fields)
        {
            if (field.IsLiteral && !field.IsInitOnly)
            {
                var value = field.GetRawConstantValue();
                if (value is string strValue)
                {
                    yield return strValue;
                }
            }
        }
    }

    /// <summary>
    /// 获取版本号与字段名的映射
    /// </summary>
    /// <param name="type">包含版本常量的类型</param>
    /// <returns>字段名-版本号字典</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static Dictionary<string, string> GetVersionConstantMap(Type type)
    {
        if (type == null) { throw new ArgumentNullException(nameof(type)); }
        if (!type.IsClass && !type.IsInterface) { throw new ArgumentException("类型必须是类或接口", nameof(type)); }
        Dictionary<string, string> result = new Dictionary<string, string>();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (FieldInfo field in fields)
        {
            if (field.IsLiteral && !field.IsInitOnly)
            {
                var value = field.GetRawConstantValue();
                if (value is string strValue)
                {
                    result[field.Name] = strValue;
                }
            }
        }
        return result;
    }
}
