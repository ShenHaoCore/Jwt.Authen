using System.Collections.Immutable;

namespace Jwt.Authen.Api.Commons;

/// <summary>
/// 版本
/// </summary>
public static class ApiVersionConsts
{
    public const string V1 = "1"; //V1版本
    public const string V2 = "2"; //V2版本

    public static readonly ImmutableArray<string> All = [V1, V2];
}
