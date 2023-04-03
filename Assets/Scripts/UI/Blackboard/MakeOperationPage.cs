using System;
using System.Collections.Generic;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.Extensions;
using XIV.Utils;

namespace LessonIsMath.UI
{
    public class MakeOperationPage : PageUI, PlayerControls.IMakeOperationUIActions, PlayerControls.IPageUIActions, IKeypadListener
    {
        [SerializeField] PageUIEventChannelSO makeOperationPageEventChannel;
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
        string currentReview;
        readonly List<string> reviewHistory = new();

        void Awake()
        {
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
            CursorManager.UnlockCursor();
            InputManager.PageUI.SetCallbacks(this);
            InputManager.MakeOperationUI.SetCallbacks(this);
            InputManager.MakeOperationUI.Enable();
            InputManager.PageUI.Enable();
            InputManager.Keypad.Enable();
            keypad.Enable();

            btn_Back.RegisterOnClick(OnBackPressed);
            btn_Subtract.RegisterOnClick(() => SelectOperation(ArithmeticOperationType.Subtract));
            btn_Add.RegisterOnClick(() => SelectOperation(ArithmeticOperationType.Add));

            reviewHistory.Clear();
            txt_ReviewInput.text = "";
            txt_InputField.text = "";
            makeOperationPageEventChannel.RaiseEvent(this);
        }

        public override void Hide()
        {
            base.Hide();
            InputManager.MakeOperationUI.Disable();
            InputManager.PageUI.Disable();
            InputManager.Keypad.Disable();
            keypad.Disable();
            
            btn_Back.UnregisterOnClick();
            btn_Subtract.UnregisterOnClick();
            btn_Add.UnregisterOnClick();
            keypad.Disable();

            CancelOperation();
            makeOperationPageEventChannel.RaiseEvent(this);
        }

        public void SetBlackboard(BlackboardUI blackboardUI)
        {
            this.blackboardUI = blackboardUI;
        }

        void OnBackPressed()
        {
            Hide();
            UISystem.GetUI<BlackboardUI>().ComeBack(this);
        }

        void OnNumberButtonClicked(int value)
        {
            if (txt_InputField.text.Length >= InputFiedlMaxTextLenght)
            {
                blackboardUI.ShowWarning(OperationWarnings.CANT_ENTER_ANYMORE_DIGIT);
                return;
            }
            if (blackboardUI.IsNumberExistsInInventory(value) == false)
            {
                blackboardUI.ShowWarning(OperationWarnings.THIS_NUMBER_DOES_NOT_EXIST_IN_INVENTORY);
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
            currentReview = "";
            UpdateReview();
        }

        void Answer()
        {
            if (IsValidInput() == false) return;

            operation.number2 = int.Parse(txt_InputField.text);
            if (operation.operationType == ArithmeticOperationType.None)
            {
                blackboardUI.ShowWarning(OperationWarnings.SELECT_AN_OPERATION);
                return;
            }
            var answer = operation.CalculateAnswer();
            if (answer < 0)
            {
                blackboardUI.ShowWarning(OperationWarnings.RESULT_CANT_BE_LESS_THAN_ZERO);
                return;
            }

            var answerText = answer.ToString();
            txt_InputField.text = answerText;
            inputNumberItems.Clear();

            int length = answerText.Length;
            for (int i = 0; i < length; i++)
            {
                int digit = int.Parse(answerText[i].ToString());
                inputNumberItems.Push(digit);
            }
            
            var operatorStr = operation.operationType switch
            {
                ArithmeticOperationType.Add => operation.GetOperator().Green(),
                ArithmeticOperationType.Subtract => operation.GetOperator().Red(),
                _ => operation.GetOperator(),
            };
            currentReview = $"{operation.number1.ToString()} {operatorStr} {operation.number2.ToString()} = {answerText.Green()}";
            if(reviewHistory.Count > 10) reviewHistory.RemoveAt(0);
            reviewHistory.Add(currentReview);
            UpdateReview();
            currentReview = "";
            operation.Reset();
        }

        void SelectOperation(ArithmeticOperationType operationType)
        {
            if (CanSelectOperation(out var number) == false) return;

            operation.number1 = number;
            operation.operationType = operationType;
            txt_InputField.text = "";
            var operatorStr = operationType switch
            {
                ArithmeticOperationType.Add => operation.GetOperator().Green(),
                ArithmeticOperationType.Subtract => operation.GetOperator().Red(),
                _ => operation.GetOperator(),
            };

            currentReview = $"{operation.number1.ToString().Green()} {operatorStr} ?";
            UpdateReview();
            txt_ReviewInput.text += currentReview;
        }

        void UpdateReview()
        {
            txt_ReviewInput.text = "";
            for (var i = 0; i < reviewHistory.Count; i++)
            {
                txt_ReviewInput.text += reviewHistory[i] + System.Environment.NewLine;
            }
        }

        bool CanSelectOperation(out int number)
        {
            number = 0;
            if (operation.operationType != ArithmeticOperationType.None)
            {
                blackboardUI.ShowWarning(OperationWarnings.COMPLETE_OR_CANCEL_OPERATION);
                return false;
            }
            if (txt_InputField.text.Length == 0)
            {
                blackboardUI.ShowWarning(OperationWarnings.ENTER_A_NUMBER);
                return false;
            }
            if (int.TryParse(txt_InputField.text, out number) == false)
            {
                blackboardUI.ShowWarning(OperationWarnings.NOT_A_VALID_INPUT);
                return false;
            }
            return true;
        }

        bool IsValidInput()
        {
            if (txt_InputField.text.Length == 0)
            {
                blackboardUI.ShowWarning(OperationWarnings.ENTER_A_NUMBER);
                return false;
            }
            if (int.TryParse(txt_InputField.text, out _) == false)
            {
                blackboardUI.ShowWarning(OperationWarnings.NOT_A_VALID_INPUT);
                return false;
            }
            return true;
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

        void PlayerControls.IPageUIActions.OnBack(InputAction.CallbackContext context)
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
