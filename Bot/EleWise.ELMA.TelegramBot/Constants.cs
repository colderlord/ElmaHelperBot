using System;
using System.Collections.Generic;
using System.Text;

namespace EleWise.ELMA.TelegramBot
{
    // ПЕРЕМЕСТИТЬ В ЯДРО


    /// <summary>
    /// Константы
    /// </summary>
    public static class Constants
    {
        public static List<string> PositiveAnswers = new List<string>
        {
            "да",
            "yes",
            "y",
            "+"
        };

        public static List<string> NegativeAnswers = new List<string>
        {
            "нет",
            "no",
            "n",
            "-"
        };

        public static bool IsPositive(string value)
        {
            return PositiveAnswers.IndexOf(value.ToLowerInvariant()) > -1;
        }

        public static bool IsNegative(string value)
        {
            return NegativeAnswers.IndexOf(value.ToLowerInvariant()) > -1;
        }
    }
}
