////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace Bqpt.Common
{
    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string value) => (T)Enum.Parse(typeof(T), value, true);

        public static string ToYesOrNo(this bool? value)
        {
            if (value is null) return "<span class=\"badge badge-danger\">N/A</span>";

            return $"<span class=\"badge badge-danger\">{((bool)value ? "YES" : "NO")}</span>";
        }

        public static string ToFileSize(this double value)
        {
            string[] suffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            for (var i = 0; i < suffixes.Length; i++)
            {
                if (value <= (Math.Pow(1024, i + 1)))
                {
                    return ThreeNonZeroDigits(value /
                        Math.Pow(1024, i)) +
                        " " + suffixes[i];
                }
            }

            return $"{ThreeNonZeroDigits(value / Math.Pow(1024, suffixes.Length - 1))} {suffixes[suffixes.Length - 1]}";
        }

        public static string ToEnumDisplayName(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString()).First();

            if (memberInfo == null || !memberInfo.CustomAttributes.Any()) return enumValue.ToString();

            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute == null) return enumValue.ToString();

            if (displayAttribute.ResourceType != null && displayAttribute.Name != null)
            {
                var manager = new ResourceManager(displayAttribute.ResourceType);

                return manager.GetString(displayAttribute.Name);
            }

            return displayAttribute.Name ?? enumValue.ToString();
        }

        public static string ToEnumDisplayDescription(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString()).First();

            if (memberInfo == null || !memberInfo.CustomAttributes.Any()) return enumValue.ToString();

            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute == null) return enumValue.ToString();

            if (displayAttribute.ResourceType != null && displayAttribute.Description != null)
            {
                var manager = new ResourceManager(displayAttribute.ResourceType);

                return manager.GetString(displayAttribute.Description);
            }

            return displayAttribute.Description ?? enumValue.ToString();
        }

        public static string ToEnumOrder(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString()).First();

            if (memberInfo == null || !memberInfo.CustomAttributes.Any()) return enumValue.ToString();

            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute == null) return enumValue.ToString();

            if (displayAttribute.ResourceType != null && displayAttribute?.Order != 0)
            {
                var manager = new ResourceManager(displayAttribute.ResourceType);

                return manager.GetString(displayAttribute.Order.ToString());
            }

            return displayAttribute.Order.ToString() ?? enumValue.ToString();
        }

        public static string ToErrorSeverityColor(this int severity)
        {
            switch (severity)
            {
                case 1:
                    return "<div class=\"badge badge-secondary p-1\"><span class=\"fa fa-circle fa-lg color-red mr-1\"></span>Fatal</div>";

                case 2:
                    return "<div class=\"badge badge-secondary p-1\"><span class=\"fa fa-circle fa-lg color-orange mr-1\"></span>Error</div>";

                case 3:
                    return "<div class=\"badge badge-secondary p-1\"><span class=\"fa fa-circle fa-lg color-yellow mr-1\"></span>Warn</div>";

                case 4:
                    return "<div class=\"badge badge-secondary p-1\"><span class=\"fa fa-circle fa-lg color-blue mr-1\"></span>Info</div>";

                case 5:
                    return "<div class=\"badge badge-secondary p-1\"><span class=\"fa fa-circle fa-lg color-green mr-1\"></span>Debug</div>";

                default:
                    return string.Empty;
            }
        }

        public static string ToVisualStatus(this bool value) => !value
                ? "<span class=\"badge badge-danger\"><span class='fa fa-thumbs-down mr-1'></span>INACTIVE</span>"
                : "<span class=\"badge badge-success\"><span class='fa fa-thumbs-up mr-1'></span>INACTIVE</span>";

        private static string ThreeNonZeroDigits(double value)
        {
            if (value >= 100) return value.ToString("0,0");
            if (value >= 10) return value.ToString("0.0");

            return value.ToString("0.00");
        }

        public static string ToFixedLength(this string s, int length)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            var words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words[0].Length > length)
                return words[0];

            var sb = new StringBuilder();

            foreach (var word in words)
            {
                if (($"{sb}{word}").Length > length)
                    return $"{sb.ToString().TrimEnd(' ')}...";

                sb.Append($"{word} ");
            }

            return $"{sb.ToString().TrimEnd(' ')}";
        }

        public static string ToCleanString(this string str) => Regex.Replace(str.Replace("$", " "), "[^a-zA-Z0-9_.%@#<>]+", " ", RegexOptions.Compiled);

        public static Uri CheckIsValidUrl(this string url)
        {
            var checkIfUrl = Uri.TryCreate(url, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return checkIfUrl ? uriResult : null;
        }

        public static decimal ConvertToDecimal(this string str)
        {
            decimal number;

            return decimal.TryParse(str, out number) ? number : 0;
        }

        public static string ToSlug(this string phrase)
        {
            var str = RemoveAccent(phrase).ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\.\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = Regex.Replace(str, @"\s", "-");

            str = Regex.Replace(str, @"\.", string.Empty);

            return str;
        }

        private static string RemoveAccent(string txt)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);

            return Encoding.ASCII.GetString(bytes);
        }
    }
}