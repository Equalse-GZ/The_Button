using System;
using System.Globalization;

namespace Game.Services
{
    public static class NumberConverter
    {
        public static string NumToString(long value)
        {
            if (value == 0) return "0";
            var culture = new CultureInfo("de-DE");
            return value.ToString("#,#", culture);
        }

        public static string NumToString(int value)
        {
            if (value == 0) return "0";
            var culture = new CultureInfo("de-DE");
            return value.ToString("#,#", culture);
        }
    }
}
