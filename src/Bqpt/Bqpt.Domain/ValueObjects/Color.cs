////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Linq;

namespace Bqpt.Domain
{
    public class Color : ValueObject
    {
        static Color()
        {
        }

        private Color()
        {
        }

        private Color(string code) => Code = code;

        public static Color From(string code)
        {
            var colour = new Color { Code = code };

            if (!SupportedColors.Contains(colour))
            {
                throw new UnsupportedColorException(code);
            }

            return colour;
        }

        public static Color White => new Color("#FFFFFF");

        public static Color Red => new Color("#FF5733");

        public static Color Orange => new Color("#FFC300");

        public static Color Yellow => new Color("#FFFF66");

        public static Color Green => new Color("#CCFF99 ");

        public static Color Blue => new Color("#6666FF");

        public static Color Purple => new Color("#9966CC");

        public static Color Grey => new Color("#999999");

        public string Code { get; private set; }

        public static implicit operator string(Color color) => color.ToString();

        public static explicit operator Color(string code) => From(code);

        public override string ToString() => Code;

        protected static IEnumerable<Color> SupportedColors
        {
            get
            {
                yield return White;
                yield return Red;
                yield return Orange;
                yield return Yellow;
                yield return Green;
                yield return Blue;
                yield return Purple;
                yield return Grey;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}