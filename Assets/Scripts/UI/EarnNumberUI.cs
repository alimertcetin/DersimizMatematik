using System;
using LessonIsMath.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using XIV.Utils;
using Random = UnityEngine.Random;

namespace LessonIsMath.UI
{
    public class EarnNumberUI : OperationUI, PlayerControls.IEarnNumberUIActions
    {
        [Header("UI Navigation")]
        [SerializeField] BlackboardMainUI blackBoardMainUI = null;
        [SerializeField] GameObject earnNumberUI;

        [Header("UI Elements")]
        [SerializeField] TMP_Text txt_EarnNumberInputField = null;
        [SerializeField] TMP_Text txt_Question = null;
        [SerializeField] Button btn_GenerateQuestion;
        [SerializeField] Button btn_Answer;

        [Header("EarnNumberUI Settings")]
        [Tooltip("The maximum value of generated question's result")]
        [SerializeField] readonly int maxValueOfAnswer = 999;
        [SerializeField] protected int InputFiedlMaxTextLenght = 7;
        ArithmeticOperation operationHelper;

        private void Awake()
        {
            InputManager.PlayerControls.EarnNumberUI.SetCallbacks(this);
        }

        public override void ShowUI()
        {
            earnNumberUI.SetActive(true);
            InputManager.EarnNumberUI.Enable();
            InputManager.BlackBoardUIManagement.Disable();

            btn_GenerateQuestion.onClick.AddListener(GenerateQuestion);
            btn_Answer.onClick.AddListener(ShowAnswerOnInputField);

            GenerateQuestion();
        }

        public override void CloseUI()
        {
            earnNumberUI.SetActive(false);
            InputManager.EarnNumberUI.Disable();
            InputManager.BlackBoardUIManagement.Enable();

            btn_GenerateQuestion.onClick.RemoveListener(GenerateQuestion);
            btn_Answer.onClick.RemoveListener(ShowAnswerOnInputField);

            blackBoardMainUI.ShowUI();
        }

        void GenerateQuestion()
        {
            int operation = Random.Range(0, 2);
            string Operator = operation == 0 ? "+" : "-";

            var answer = Random.Range(0, maxValueOfAnswer);
            if (operation == 0)
            {
                operationHelper.operationType = ArithmeticOperationType.Add;
                operationHelper.number1 = Random.Range(0, answer);
                operationHelper.number2 = answer - operationHelper.number1;
            }
            else
            {
                operationHelper.operationType = ArithmeticOperationType.Subtract;
                operationHelper.number1 = Random.Range(answer, answer + maxValueOfAnswer);
                operationHelper.number2 = operationHelper.number1 - answer;
            }

            txt_Question.text = $"Question : {operationHelper.number1} {Operator} {operationHelper.number2}";
        }

        protected override void OnNumberButtonClicked(int value)
        {
            if (txt_EarnNumberInputField.text.Length >= InputFiedlMaxTextLenght)
            {
                warningUIChannel.RaiseEvent(OperationWarnings.CANT_ENTER_ANYMORE_DIGIT);
                return;
            }

            txt_EarnNumberInputField.text += value;
        }

        protected override void Delete()
        {
            if (txt_EarnNumberInputField.text.Length == 0) return;

            txt_EarnNumberInputField.text = txt_EarnNumberInputField.text.Remove(txt_EarnNumberInputField.text.Length - 1);
        }

        void ShowAnswerOnInputField()
        {
            if (txt_EarnNumberInputField.text.Length == 0)
            {
                warningUIChannel.RaiseEvent(OperationWarnings.ENTER_A_NUMBER);
                return;
            }
            var answer = int.Parse(txt_EarnNumberInputField.text);
            if (answer != operationHelper.CalculateAnswer())
            {
                txt_EarnNumberInputField.text = "";
                warningUIChannel.RaiseEvent(OperationWarnings.WRONG_ANSWER);
                return;
            }
            // TODO : Inventory is full case
            var item = numberItems[Random.Range(0, numberItems.Length)].GetItem();
            int amount = 1;
            inventory.TryAdd(item, ref amount);
            GenerateQuestion();
            txt_EarnNumberInputField.text = "";
            warningUIChannel.RaiseEvent("You earned " + item.Value);
        }

        #region INPUT HANDLING

        void PlayerControls.IEarnNumberUIActions.OnZero(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(0);
        }

        void PlayerControls.IEarnNumberUIActions.OnOne(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(1);
        }

        void PlayerControls.IEarnNumberUIActions.OnTwo(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(2);
        }

        void PlayerControls.IEarnNumberUIActions.OnThree(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(3);
        }

        void PlayerControls.IEarnNumberUIActions.OnFour(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(4);
        }

        void PlayerControls.IEarnNumberUIActions.OnFive(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(5);
        }

        void PlayerControls.IEarnNumberUIActions.OnSix(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(6);
        }

        void PlayerControls.IEarnNumberUIActions.OnSeven(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(7);
        }

        void PlayerControls.IEarnNumberUIActions.OnEight(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(8);
        }

        void PlayerControls.IEarnNumberUIActions.OnNine(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(9);
        }

        void PlayerControls.IEarnNumberUIActions.OnDelete(InputAction.CallbackContext context)
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

        void PlayerControls.IEarnNumberUIActions.OnExit(InputAction.CallbackContext context)
        {
            if (context.performed) CloseUI();
        }

        void PlayerControls.IEarnNumberUIActions.OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed) ShowAnswerOnInputField();
        }

        void PlayerControls.IEarnNumberUIActions.OnGenerateQuestion(InputAction.CallbackContext context)
        {
            if (context.performed) GenerateQuestion();
        }

        #endregion

    }
}