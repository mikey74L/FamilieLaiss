using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace FamilieLaissSharedUI.Extensions;

public static class NavigationManagerExtensions
{
    public static bool TryGetQueryString(this NavigationManager navManager, Type type, string key, out object value)
    {
        var uri = navManager.ToAbsoluteUri(navManager.Uri);

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
        {
            if (type == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
            {
                value = valueAsInt;
                return true;
            }

            if (type == typeof(long) && long.TryParse(valueFromQueryString, out var valueAsLong))
            {
                value = valueAsLong;
                return true;
            }

            if (type == typeof(string))
            {
                value = valueFromQueryString.ToString();
                return true;
            }

            if (type == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
            {
                value = valueAsDecimal;
                return true;
            }
        }

        value = default;

        return false;
    }
}
