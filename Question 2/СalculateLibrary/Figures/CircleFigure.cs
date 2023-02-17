using System;
using СalculateLibrary.Extensions;
using СalculateLibrary.FIGURES.Base;

namespace СalculateLibrary.FIGURES
{
    public class CircleFigure : Figure
    {
        public double areaByRadius { get; set; }

        public override void Calculate(string[] valueParts)
        {
            if (valueParts == null)
            {
                throw new ArgumentException("Invalid data");
            }
            string input = valueParts[1];
            areaByRadius = valueParts[1].CircleAreaByRadius();
        }

        public override string ToString()
        {
            return "TEST";
        }
    }
}
