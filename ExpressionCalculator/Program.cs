using Common;

namespace ExpressionCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var processor = TypeFactory.Get<IExpressionCalculatorProcessor>();
            processor.Process();
        }
    }
}
