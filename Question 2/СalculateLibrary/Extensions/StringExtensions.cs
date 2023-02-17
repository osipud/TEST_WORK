using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace СalculateLibrary.Extensions
{
    public static class StringExtensions
    {
        public static double CircleAreaByRadius(this string inputString)
        {
            return Math.PI * (double.Parse(inputString) * double.Parse(inputString));
        }
        
        public static bool RightTriangle(this string inputString, string PointA, string PointB, string PointC)
        {
            if (string.IsNullOrEmpty(inputString)) return false;

            if (Math.Pow(int.Parse(PointA),2) == (Math.Pow(int.Parse(PointB),2) + Math.Pow(int.Parse(PointC),2)) ||
                Math.Pow(int.Parse(PointB),2) == (Math.Pow(int.Parse(PointA),2) + Math.Pow(int.Parse(PointC),2)) ||
                Math.Pow(int.Parse(PointC),2) == (Math.Pow(int.Parse(PointA),2) + Math.Pow(int.Parse(PointB),2))
                )
            {
                return true;
            }
            else 
            {
                return false;
            } 
        }

        public static double TriangleArea(this string inputString, string PointA, string PointB, string PointC)
        {
            if (string.IsNullOrEmpty(inputString)) return 0.0;

            double semiPerimeter = ((double.Parse(PointA) + double.Parse(PointB) + double.Parse(PointC)) / 2);
            return Math.Sqrt((semiPerimeter * (semiPerimeter - (double.Parse(PointA)) * (semiPerimeter - (double.Parse(PointB)) * (semiPerimeter - (double.Parse(PointC)))))));
        }



    }
}
