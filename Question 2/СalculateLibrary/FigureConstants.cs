using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using СalculateLibrary.FIGURES;
using СalculateLibrary.Exceptions;

namespace СalculateLibrary
{
    public static class FigureConstants
    {
        private static readonly Dictionary<string, Type> TypeDictionary = new Dictionary<string, Type>
        {
            {"CIRCLE", typeof(CircleFigure)},
            //{"SQUARE", typeof(SquareFigure)},
            {"TRIANGLE", typeof(TriangleFigure)}
        };

        /// <summary>
        /// Returns the correct class type of the message.
        /// </summary>
        /// <param name="typeName">The type name given.</param>
        /// <returns>The class type.</returns>
        /// <exception cref="UnknownTypeException">Given if the type is unkown.</exception>
        public static Type GetClassType(string typeName)
        {
            Type result;
            TypeDictionary.TryGetValue(typeName, out result);

            if (result == null)
            {
                throw new UnknownTypeException();
            }

            return result;
        }
    }
}
