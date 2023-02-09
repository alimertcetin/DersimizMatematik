namespace XIV.Utils
{
    public enum ArithmeticOperation
    {
        None = 0,
        Add = 1,
        Subtract = 2,
        Multiply = 3,
        Divide = 4,
    }

    public struct OperationHelper
    {
        public int number1;
        public int number2;
        public ArithmeticOperation operation;

        public int GetAnswer()
        {
            return operation switch
            {
                ArithmeticOperation.Add => Add(),
                ArithmeticOperation.Subtract => Substract(),
                ArithmeticOperation.Multiply => Muliply(),
                ArithmeticOperation.Divide => Divide(),
                _ => 0,
            };
        }

        public string GetOperator()
        {
            return operation switch
            {
                ArithmeticOperation.Add => "+",
                ArithmeticOperation.Subtract => "-",
                ArithmeticOperation.Multiply => "*",
                ArithmeticOperation.Divide => "/",
                _ => "",
            };
        }

        int Add() => number1 + number2;
        int Substract() => number1 - number2;
        int Muliply() => number1 * number2;
        int Divide() => number1 / number2;
    }
}