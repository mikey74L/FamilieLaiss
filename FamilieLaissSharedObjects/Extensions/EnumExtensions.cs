using FamilieLaissSharedObjects.Attributes;
using FamilieLaissSharedObjects.Enums;
using System;
using System.Globalization;
using System.Linq;

namespace FamilieLaissSharedObjects.Extensions;

public static class EnumExtensions
{
    private static string GetDescription(object enumValue)
    {
        var enumType = enumValue.GetType();
        var name = Enum.GetName(enumType, enumValue);
        var attr = enumType.GetField(name).GetCustomAttributes(false).OfType<DescriptionTranslationAttribute>().FirstOrDefault();

        if (attr is null)
        {
            return "<Not defined>";
        }

        return GetLocalizedText(attr);
    }

    private static string GetLocalizedText(DescriptionTranslationAttribute attr)
    {
        return CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de" ?
            attr.GermanDescription : attr.EnglishDescription;
    }

    public static string Description(this EnumCategoryType? enumCategoryType)
    {
        if (enumCategoryType is null)
            return string.Empty;

        return GetDescription(enumCategoryType);
    }

    public static string Description(this EnumPictureConvertStatus? enumPictureConvertStatus)
    {
        if (enumPictureConvertStatus is null)
            return string.Empty;

        return GetDescription(enumPictureConvertStatus);
    }

    public static string Description(this EnumVideoConvertStatus? enumVideoConvertStatus)
    {
        if (enumVideoConvertStatus is null)
            return string.Empty;

        return GetDescription(enumVideoConvertStatus);
    }
}
