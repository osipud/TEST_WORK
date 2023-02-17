using System;
using СalculateLibrary.Extensions;
using СalculateLibrary.FIGURES.Base;

namespace СalculateLibrary.FIGURES
{
    public class TriangleFigure : Figure
    {
        public double areaTriangle { get; set; }
        public bool rightTriangle { get; set; }

        public override void Calculate(string[] valueParts)
        {
            if (valueParts == null)
            {
                throw new ArgumentException("Invalid data");
            }
            rightTriangle = valueParts[0].RightTriangle(valueParts[1], valueParts[2], valueParts[3]);
            areaTriangle = valueParts[0].TriangleArea(valueParts[1], valueParts[2], valueParts[3]);
        }

        public override string ToString()
        {
            return "TEST-TRIANGLE";
        }
    }
}
