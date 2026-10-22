using System;

public static class EnumExtension
{
    public static T ParseEnum<T>(string input) where T : struct
    {
        if (Enum.TryParse(input, true, out T enumValue))
        {
            return enumValue;
        }
        return default(T);
    }
}