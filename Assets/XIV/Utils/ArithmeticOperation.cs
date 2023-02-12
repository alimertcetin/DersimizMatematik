using System;

namespace XIV.Utils
{
    public enum ArithmeticOperationType
    {
        None = 0,
        Add = 1,
        Subtract = 2,
        Multiply = 3,
        Divide = 4,
    }

    [System.Serializable]
    public struct ArithmeticOperation
    {
        public ArithmeticOperationType operationType;
        public int number1;
        public int number2;
        public int answer;
        static Random rnd;
        static Random random
        {
            get
            {
                if (rnd == null) rnd = new Random();
                return rnd;
            }
            set => rnd = value;
        }

        public const int MAX_VALUE_OF_ANSWER = 1000000;

        public int CalculateAnswer()
        {
            answer = operationType switch
            {
                ArithmeticOperationType.Add => number1 + number2,
                ArithmeticOperationType.Subtract => number1 - number2,
                ArithmeticOperationType.Multiply => number1 * number2,
                ArithmeticOperationType.Divide => number1 / number2,
                _ => 0,
            };
            return answer;
        }

        public string GetOperator()
        {
            return operationType switch
            {
                ArithmeticOperationType.Add => "+",
                ArithmeticOperationType.Subtract => "-",
                ArithmeticOperationType.Multiply => "*",
                ArithmeticOperationType.Divide => "/",
                _ => "",
            };
        }

        public void GenerateQuestion()
        {
            GenerateQuestion((ArithmeticOperationType)random.Next(1, 5), random.Next(0, MAX_VALUE_OF_ANSWER));
        }

        public void GenerateQuestion(ArithmeticOperationType operationType)
        {
            GenerateQuestion(operationType, random.Next(0, MAX_VALUE_OF_ANSWER));
        }

        public void GenerateQuestion(int answer, int maxValueOfAnswer)
        {
            GenerateQuestion((ArithmeticOperationType)random.Next(1, 5), answer, maxValueOfAnswer);
        }

        public void GenerateQuestion(int maxValueOfAnswer)
        {
            GenerateQuestion((ArithmeticOperationType)random.Next(1, 5), random.Next(0, maxValueOfAnswer), maxValueOfAnswer);
        }

        public void GenerateQuestion(ArithmeticOperationType operationType, int answer, int maxValueOfAnswer = MAX_VALUE_OF_ANSWER)
        {
            this.operationType = operationType;
            this.answer = answer;
            switch (operationType)
            {
                case ArithmeticOperationType.None:
                    break;
                case ArithmeticOperationType.Add:
                    number1 = random.Next(0, answer);
                    number2 = answer - number1;
                    break;
                case ArithmeticOperationType.Subtract:
                    if (answer + maxValueOfAnswer < 0) throw new ArgumentOutOfRangeException();

                    number1 = random.Next(answer, answer + maxValueOfAnswer);
                    number2 = number1 - answer;
                    break;
                case ArithmeticOperationType.Multiply:
                    number1 = answer;
                    number2 = 1;
                    for (int i = answer / 2; i > 0; i--)
                    {
                        if (answer % i != 0) continue;

                        number1 = i;
                        number2 = answer / i;
                        break;
                    }
                    break;
                case ArithmeticOperationType.Divide:
                    number1 = answer;
                    number2 = 1;
                    for (int i = 2; i < answer; i++)
                    {
                        if (answer % i != 0) continue;

                        number1 = answer * i;
                        number2 = i;
                        break;
                    }
                    break;
            }
        }

        public void Reset()
        {
            number1 = 0;
            number2 = 0;
            operationType = ArithmeticOperationType.None;
        }

        public override string ToString()
        {
            return $"{number1} {GetOperator()} {number2}";
        }
    }
}