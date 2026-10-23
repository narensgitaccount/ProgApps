using System.ComponentModel;
using System.Reflection;
using ADE.ESS.SEPA.Application.Models;

namespace ADE.ESS.SEPA.Client.Auth;

public static class UserStateExtensions
{
    public static bool HasPermission(this UserState user, ESSUserPermissionsEnum permission)
    {
        if (user.Permissions.Count == 0) return false;
        var description = GetEnumDescription(permission);
        return user.Permissions.Any(p => string.Equals(p.Permission, description, StringComparison.OrdinalIgnoreCase));
    }

    public static bool HasAnyPermission(this UserState user, params ESSUserPermissionsEnum[] permissions)
    {
        if (user.Permissions.Count == 0) return false;
        var descriptions = permissions.Select(GetEnumDescription).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return user.Permissions.Any(p => descriptions.Contains(p.Permission));
    }

    private static string GetEnumDescription<TEnum>(TEnum value) where TEnum : Enum
    {
        var name = value.ToString();
        var attr = typeof(TEnum).GetField(name)?.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? name;
    }
}
