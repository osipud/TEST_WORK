using Microsoft.VisualStudio.TestTools.UnitTesting;
using СalculateLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using СalculateLibrary.FIGURES;

namespace СalculateLibrary.Tests
{
    [TestClass()]
    public class CalculateTests
    {
        [TestMethod()]
        public void RightTriangleTest()
        {
            // arrange
            Calculate library = new Calculate();
            string dataTriangle = "TRIANGLE, 6,8,10";
            bool expected = true;

            // act
            var calc = library.Parse(dataTriangle);
            var resultTriangle = (TriangleFigure)calc;

            // assert
            bool actual = resultTriangle.rightTriangle;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AreaOfTriangleTest()
        {
            // arrange
            Calculate library = new Calculate();
            string dataTriangle = "TRIANGLE, 6,8,10";
            double expected = 20.784609690826528;

            // act
            var calc = library.Parse(dataTriangle);
            var resultTriangle = (TriangleFigure)calc;

            // assert
            double actual = resultTriangle.areaTriangle;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CalcCircleAreByRadiusTest()
        {
            // arrange
            Calculate library = new Calculate();
            string dataCircle = "CIRCLE, 15";
            double expected = 706.8583470577034;

            // act
            var calc = library.Parse(dataCircle);
            var resultCircle = (CircleFigure)calc;

            // assert
            double actual = resultCircle.areaByRadius;

            Assert.AreEqual(expected, actual);
        }

    }
}