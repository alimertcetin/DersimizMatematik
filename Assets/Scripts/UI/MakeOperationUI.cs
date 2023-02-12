using System.Collections.Generic;
using LessonIsMath.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.InventorySystem.Items;
using XIV.Utils;

namespace LessonIsMath.UI
{
    public class MakeOperationUI : OperationUI, PlayerControls.IMakeOperationUIActions
    {
        [Header("UI Navigation")]
        [SerializeField] BlackboardMainUI blackBoardMainUI = null;
        [SerializeField] GameObject makeOperationUI = null;

        [Header("UI Elements")]
        [SerializeField] TMP_Text txt_OperationInputField = null;
        [SerializeField] TMP_Text txt_ReviewInput = null;
        [SerializeField] CustomButton btn_Subtract;
        [SerializeField] CustomButton btn_Add;
        [SerializeField] CustomButton btn_Answer;

        [Header("UI Settings")]
        [SerializeField] int InputFiedlMaxTextLenght = 7;

        ArithmeticOperation operation;
        List<NumberItem> inputNumberItems = new List<NumberItem>();

        private void Awake()
        {
            InputManager.PlayerControls.MakeOperationUI.SetCallbacks(this);
        }

        public override void ShowUI()
        {
            makeOperationUI.SetActive(true);
            btn_Subtract.Register(() => SelectOperation(ArithmeticOperationType.Subtract));
            btn_Add.Register(() => SelectOperation(ArithmeticOperationType.Add));
            btn_Answer.Register(Answer);

            InputManager.MakeOperationUI.Enable();
            InputManager.BlackBoardUIManagement.Disable();
        }

        public override void CloseUI()
        {
            makeOperationUI.SetActive(false);

            btn_Subtract.Unregister();
            btn_Add.Unregister();
            btn_Answer.Unregister();

            InputManager.MakeOperationUI.Disable();
            InputManager.BlackBoardUIManagement.Enable();
            blackBoardMainUI.ShowUI();

            CancelOperation();
        }

        void CancelOperation()
        {
            while (inputNumberItems.Count > 0)
            {
                Delete();
            }
            txt_OperationInputField.text = "";
            txt_ReviewInput.text = "";
            operation.Reset();
        }

        protected override void Delete()
        {
            void Del(int inputLength)
            {
                int index = inputNumberItems.Count - 1;
                int amount = 1;
                inventory.TryAdd(inputNumberItems[index], ref amount);
                inputNumberItems.RemoveAt(index);

                if (inputLength > 0) txt_OperationInputField.text = txt_OperationInputField.text.Remove(inputLength - 1);
            }

            if (inputNumberItems.Count == 0) return;

            int inputLength = txt_OperationInputField.text.Length;
            if (operation.operationType == ArithmeticOperationType.None)
            {
                Del(inputLength);
                return;
            }
            if (inputLength != 0)
            {
                Del(inputLength);
                return;
            }
            // Operation is selected but input field is empty
            // Clear the review field, fill input field with previous number and clear the operationType
            operation.operationType = ArithmeticOperationType.None;
            txt_OperationInputField.text = operation.number1.ToString();
            txt_ReviewInput.text = "";
        }


        private void Answer()
        {
            if (OperationErrorHelper.IsValidInput(txt_OperationInputField.text, out var error) == false)
            {
                warningUIChannel.RaiseEvent(error);
                return;
            }
            operation.number2 = int.Parse(txt_OperationInputField.text);
            if (OperationErrorHelper.CanCompleteOperation(operation, out error) == false)
            {
                warningUIChannel.RaiseEvent(error);
                return;
            }

            var currentInput = operation.CalculateAnswer().ToString();
            operation.Reset();

            txt_OperationInputField.text = currentInput;
            txt_ReviewInput.text = "";
            inputNumberItems.Clear();

            int length = currentInput.Length;
            for (int i = 0; i < length; i++)
            {
                int digit = int.Parse(currentInput[i].ToString());

                if (digit < numberItems.Length && numberItems[digit].item.Value == digit)
                {
                    inputNumberItems.Add(numberItems[digit].GetItem());
                    continue;
                }

                for (int j = 0; j < numberItems.Length; j++)
                {
                    if (numberItems[j].item.Value == digit)
                    {
                        inputNumberItems.Add(numberItems[j].GetItem());
                        break;
                    }
                }

            }
        }

        protected override void OnNumberButtonClicked(int value)
        {
            if (OperationErrorHelper.CanAddInput(txt_OperationInputField.text, InputFiedlMaxTextLenght, out var error) == false)
            {
                warningUIChannel.RaiseEvent(error);
                return;
            }
            if (OperationErrorHelper.IsInputExistInInventory(inventory, value, out var index, out error) == false)
            {
                warningUIChannel.RaiseEvent(error);
                return;
            }

            inputNumberItems.Add(inventory[index].Item as NumberItem);
            int amount = 1;
            inventory.RemoveAt(index, ref amount);
            txt_OperationInputField.text += value;
        }

        void SelectOperation(ArithmeticOperationType operationType)
        {
            if (OperationErrorHelper.CanSelectAnOperation(operation, txt_OperationInputField.text, out var error) == false)
            {
                warningUIChannel.RaiseEvent(error);
                return;
            }
            operation.number1 = int.Parse(txt_OperationInputField.text);
            operation.operationType = operationType;
            txt_OperationInputField.text = "";
            txt_ReviewInput.text = GetCurrentOperationReviewString();
        }

        string GetCurrentOperationReviewString()
        {
            return
                $"First Number : {operation.number1}, " +
                $"Operation : {operation.operationType}, " +
                $"Second Number : {operation.number2}";
        }

        #region INPUT HANDLING

        void PlayerControls.IMakeOperationUIActions.OnZero(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(0);
        }

        void PlayerControls.IMakeOperationUIActions.OnOne(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(1);
        }

        void PlayerControls.IMakeOperationUIActions.OnTwo(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(2);
        }

        void PlayerControls.IMakeOperationUIActions.OnThree(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(3);
        }

        void PlayerControls.IMakeOperationUIActions.OnFour(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(4);
        }

        void PlayerControls.IMakeOperationUIActions.OnFive(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(5);
        }

        void PlayerControls.IMakeOperationUIActions.OnSix(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(6);
        }

        void PlayerControls.IMakeOperationUIActions.OnSeven(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(7);
        }

        void PlayerControls.IMakeOperationUIActions.OnEight(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(8);
        }

        void PlayerControls.IMakeOperationUIActions.OnNine(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(9);
        }

        void PlayerControls.IMakeOperationUIActions.OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed) Answer();
        }

        void PlayerControls.IMakeOperationUIActions.OnExit(InputAction.CallbackContext context)
        {
            if (context.performed) CloseUI();
        }

        void PlayerControls.IMakeOperationUIActions.OnDelete(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Delete();
                deleteStarted = true;
            }

            if (context.canceled)
            {
                deleteTimer.Restart();
                deleteStarted = false;
            }
        }

        void PlayerControls.IMakeOperationUIActions.OnMinus(InputAction.CallbackContext context)
        {
            if (context.performed) SelectOperation(ArithmeticOperationType.Subtract);
        }
        
        void PlayerControls.IMakeOperationUIActions.OnPlus(InputAction.CallbackContext context)
        {
            if (context.performed) SelectOperation(ArithmeticOperationType.Add);
        }
    }

    #endregion
}
