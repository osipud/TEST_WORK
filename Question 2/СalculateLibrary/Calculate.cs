using СalculateLibrary.FIGURES.Base;

namespace СalculateLibrary
{
    public class Calculate
    {
        public Figure Parse(string message)
        {
            var messageParts = message.Split(',');
            var classType = FigureConstants.GetClassType(messageParts[0]);
            var newInstance = (Figure)Activator.CreateInstance(classType);
            newInstance.Calculate(messageParts);
            return newInstance;
        }




    }
}