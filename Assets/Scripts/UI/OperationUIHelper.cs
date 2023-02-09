using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.UI;
using XIV.Utils;

namespace GameCore.UI
{
    public static class OperationUIHelper
    {
        public static bool IsInputExistInInventory(Inventory inventory, int input, out int index, out string error)
        {
            for (int i = 0; i < inventory.SlotCount; i++)
            {
                if (inventory[i].Item is NumberItem item && item.Value == input)
                {
                    index = i;
                    error = "";
                    return true;
                }
            }
            index = -1;
            error = OperationWarnings.THIS_NUMBER_IS_NOT_EXIST_IN_INVENTORY;
            return false;
        }

        public static string GetCurrentOperationReviewString(ref OperationHelper operationHelper)
        {
            return
                $"First Number : {operationHelper.number1}, " +
                $"Operation : {operationHelper.operation}, " +
                $"Second Number : {operationHelper.number2}";
        }

        public static bool CanSelectAnOperation(ref OperationHelper operationHelper, ref string currentInput, out string error)
        {
            if (operationHelper.operation != ArithmeticOperation.None)
            {
                error = OperationWarnings.COMPLETE_OR_CANCEL_OPERATION;
                return false;
            }
            else if (currentInput.Length == 0)
            {
                error = OperationWarnings.ENTER_A_NUMBER;
                return false;
            }
            error = "";
            return true;
        }

        public static bool AddInput(int maxInputLength, ref string currentInput, int input, out string error)
        {
            if (currentInput.Length >= maxInputLength)
            {
                error = OperationWarnings.CANT_ENTER_ANYMORE_DIGIT;
                return false;
            }

            currentInput += input;
            error = "";
            return true;
        }

        public static bool SelectAddOperation(ref OperationHelper operationHelper, ref string currentInput, out string error)
        {
            if (CanSelectAnOperation(ref operationHelper, ref currentInput, out error) == false) return false;

            operationHelper.number1 = int.Parse(currentInput);
            operationHelper.operation = ArithmeticOperation.Add;
            currentInput = "";
            return true;
        }

        public static bool SelectSubtractOperation(ref OperationHelper operationHelper, ref string currentInput, out string error)
        {
            if (CanSelectAnOperation(ref operationHelper, ref currentInput, out error) == false) return false;

            operationHelper.number1 = int.Parse(currentInput);
            operationHelper.operation = ArithmeticOperation.Subtract;
            currentInput = "";
            return true;
        }

        /// <summary>
        /// If answer is valid returns true and sets input text as answer
        /// </summary>
        public static bool CalculateAnswer(ref OperationHelper operationHelper, ref string currentInput, out string error)
        {
            if (currentInput.Length == 0)
            {
                error = OperationWarnings.ENTER_A_NUMBER;
                return false;
            }
            else if (operationHelper.operation == ArithmeticOperation.None)
            {
                error = OperationWarnings.SELECT_AN_OPERATION;
                return false;
            }

            operationHelper.number2 = int.Parse(currentInput);
            var answer = operationHelper.GetAnswer();

            if (answer < 0)
            {
                error = OperationWarnings.RESULT_CANT_BE_LESS_THAN_ZERO;
                return false;
            }

            currentInput = answer.ToString();
            operationHelper.number1 = 0;
            operationHelper.number2 = 0;
            operationHelper.operation = ArithmeticOperation.None;
            error = "";
            return true;

        }

        public static int[] GetDigits(string inputText)
        {
            int length = inputText.Length;
            int[] digits = new int[length];
            for (int i = 0; i < length; i++)
            {
                digits[i] = int.Parse(inputText[i].ToString());
            }
            return digits;
        }
    }
}