using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.Utils;

namespace LessonIsMath.UI
{
    public static class OperationErrorHelper
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

        public static bool CanSelectAnOperation(ArithmeticOperation operation, string currentInput, out string error)
        {
            if (operation.operationType != ArithmeticOperationType.None)
            {
                error = OperationWarnings.COMPLETE_OR_CANCEL_OPERATION;
                return false;
            }
            return IsValidInput(currentInput, out error);
        }

        public static bool CanAddInput(string currentInput, int maxInputLength, out string error)
        {
            if (currentInput.Length >= maxInputLength)
            {
                error = OperationWarnings.CANT_ENTER_ANYMORE_DIGIT;
                return false;
            }

            error = "";
            return true;
        }

        public static bool IsValidInput(string currentInput, out string error)
        {
            if (currentInput.Length == 0)
            {
                error = OperationWarnings.ENTER_A_NUMBER;
                return false;
            }
            if (int.TryParse(currentInput, out _) == false)
            {
                error = OperationWarnings.NOT_A_VALID_INPUT;
                return false;
            }

            error = "";
            return true;
        }

        public static bool CanCompleteOperation(ArithmeticOperation operation, out string error)
        {
            if (operation.operationType == ArithmeticOperationType.None)
            {
                error = OperationWarnings.SELECT_AN_OPERATION;
                return false;
            }
            if (operation.CalculateAnswer() < 0)
            {
                error = OperationWarnings.RESULT_CANT_BE_LESS_THAN_ZERO;
                return false;
            }

            error = "";
            return true;
        }
    }
}