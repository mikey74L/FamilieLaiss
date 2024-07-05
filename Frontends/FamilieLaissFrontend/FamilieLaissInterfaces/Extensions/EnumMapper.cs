namespace FamilieLaissInterfaces.Extensions;

public static class EnumMapper
{
    public static TTargetEnum? MapEnum<TSourceEnum, TTargetEnum>(this TSourceEnum? enumValue)
        where TSourceEnum : Enum where TTargetEnum : Enum
    {
        if (enumValue is not null)
        {
            var sourceType = typeof(TSourceEnum);
            var sourceName = Enum.GetName(sourceType, enumValue);

            if (sourceName is not null)
            {
                var targetType = typeof(TTargetEnum);
                var result = (TTargetEnum)Enum.Parse(targetType, sourceName);

                return result;
            }
        }

        return default(TTargetEnum);
    }
}