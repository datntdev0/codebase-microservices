using datntdev.Abp.Extensions;
using System.Text.RegularExpressions;

namespace datntdev.Microservice.Helpers;

public static partial class ValidationHelper
{
    [GeneratedRegex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    private static partial Regex EmailRegex();

    public static bool IsEmail(string value)
    {
        return value.IsNullOrEmpty() ? false : EmailRegex().IsMatch(value);
    }
}
