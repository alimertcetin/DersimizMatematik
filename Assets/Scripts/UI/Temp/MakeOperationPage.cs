using System.Collections.Generic;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.Collections;
using XIV.InventorySystem.Items;
using XIV.Utils;

namespace LessonIsMath.UI
{
    public class MakeOperationPage : GameUI, PlayerControls.IMakeOperationUIActions, IKeypadListener
    {
        [SerializeField] Keypad keypad;
        [SerializeField] TMP_Text txt_InputField = null;
        [SerializeField] TMP_Text txt_ReviewInput = null;
        [SerializeField] CustomButton btn_Back;
        [SerializeField] CustomButton btn_Subtract;
        [SerializeField] CustomButton btn_Add;
        [SerializeField] float deleteDuration;
        [SerializeField] float waitDeleteDuration;
        Timer deleteTimer;
        Timer waitDeleteStartTimer;

        [SerializeField] int InputFiedlMaxTextLenght = 7;
        ArithmeticOperation operation;
        bool deleteStarted = false;
        Stack<int> inputNumberItems;
        BlackboardUI blackboardUI;

        protected override void Awake()
        {
            base.Awake();
            InputManager.PlayerControls.MakeOperationUI.SetCallbacks(this);
            keypad.SetListener(this);
            inputNumberItems = new Stack<int>(InputFiedlMaxTextLenght);
        }

        void Update()
        {
            if (deleteStarted == false || waitDeleteStartTimer.Update(Time.deltaTime) == false || deleteTimer.Update(Time.deltaTime) == false) return;

            Delete();
            var newDuration = deleteTimer.Duration - 0.025f;
            newDuration = newDuration < 0 ? 0 : newDuration;
            deleteTimer = new Timer(newDuration);
        }

        public override void Show()
        {
            base.Show();
            btn_Back.RegisterOnClick(OnBackPressed);
            btn_Subtract.RegisterOnClick(() => SelectOperation(ArithmeticOperationType.Subtract));
            btn_Add.RegisterOnClick(() => SelectOperation(ArithmeticOperationType.Add));
            keypad.Enable();

            InputManager.MakeOperationUI.Enable();
            InputManager.BlackBoardUIManagement.Disable();
        }

        public override void Hide()
        {
            base.Hide();
            btn_Back.UnregisterOnClick();
            btn_Subtract.UnregisterOnClick();
            btn_Add.UnregisterOnClick();
            keypad.Disable();

            CancelOperation();
            InputManager.MakeOperationUI.Disable();
            InputManager.BlackBoardUIManagement.Enable();
        }

        public void SetBlackboard(BlackboardUI blackboardUI)
        {
            this.blackboardUI = blackboardUI;
        }

        void OnBackPressed()
        {
            Hide();
            UISystem.Show<BlackboardUI>();
        }

        void OnNumberButtonClicked(int value)
        {
            if (OperationErrorHelper.CanAddInput(txt_InputField.text, InputFiedlMaxTextLenght, out var error) == false)
            {
                blackboardUI.ShowWarning(error);
                return;
            }
            if (blackboardUI.IsNumberExistsInInventory(value) == false)
            {
                blackboardUI.ShowWarning(OperationWarnings.THIS_NUMBER_IS_NOT_EXIST_IN_INVENTORY);
                return;
            }

            blackboardUI.RemoveNumberFromInventory(value);
            inputNumberItems.Push(value);
            txt_InputField.text += value;
        }

        void CancelOperation()
        {
            while (inputNumberItems.Count > 0)
            {
                Delete();
            }
            txt_InputField.text = "";
            txt_ReviewInput.text = "";
            operation.Reset();
        }

        void Delete()
        {
            void Del(int inputLength)
            {
                blackboardUI.AddNumberToInventory(inputNumberItems.Pop());

                if (inputLength > 0) txt_InputField.text = txt_InputField.text.Remove(inputLength - 1);
            }

            if (inputNumberItems.Count == 0) return;

            int inputLength = txt_InputField.text.Length;
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
            txt_InputField.text = operation.number1.ToString();
            txt_ReviewInput.text = "";
        }

        private void Answer()
        {
            if (OperationErrorHelper.IsValidInput(txt_InputField.text, out var error) == false)
            {
                blackboardUI.ShowWarning(error);
                return;
            }
            operation.number2 = int.Parse(txt_InputField.text);
            if (OperationErrorHelper.CanCompleteOperation(operation, out error) == false)
            {
                blackboardUI.ShowWarning(error);
                return;
            }

            var answer = operation.CalculateAnswer();
            var answerText = answer.ToString();
            txt_InputField.text = answerText;
            operation.Reset();
            inputNumberItems.Clear();

            int length = answerText.Length;
            for (int i = 0; i < length; i++)
            {
                int digit = int.Parse(answerText[i].ToString());
                inputNumberItems.Push(digit);
            }
        }

        void SelectOperation(ArithmeticOperationType operationType)
        {
            if (OperationErrorHelper.CanSelectAnOperation(operation, txt_InputField.text, out var error) == false)
            {
                blackboardUI.ShowWarning(error);
                return;
            }
            operation.number1 = int.Parse(txt_InputField.text);
            operation.operationType = operationType;
            txt_InputField.text = "";
            txt_ReviewInput.text = GetCurrentOperationReviewString();
        }

        string GetCurrentOperationReviewString()
        {
            return
                $"First Number : {operation.number1}, " +
                $"Operation : {operation.operationType}, " +
                $"Second Number : {operation.number2}";
        }

        void IKeypadListener.OnEnter() => Answer();
        void IKeypadListener.OnDeleteStarted()
        {
            Delete();
            deleteStarted = true;
            deleteTimer = new Timer(deleteDuration);
            waitDeleteStartTimer = new Timer(waitDeleteDuration);
        }
        void IKeypadListener.OnDeleteCanceled()
        {
            waitDeleteStartTimer.Restart();
            deleteTimer.Restart();
            deleteStarted = false;
        }

        void IKeypadListener.OnNumberPressed(int value) => OnNumberButtonClicked(value);

        void PlayerControls.IMakeOperationUIActions.OnExit(InputAction.CallbackContext context)
        {
            if (context.performed) OnBackPressed();
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
}
