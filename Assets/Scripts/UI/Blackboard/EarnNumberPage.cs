using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using XIV.Core.Utils;
using Random = UnityEngine.Random;

namespace LessonIsMath.UI
{
    public class EarnNumberPage : PageUI, PlayerControls.IPageUIActions, PlayerControls.IEarnNumberUIActions, IKeypadListener
    {
        [SerializeField] PageUIEventChannelSO earnNumberPageEventChannel;
        [SerializeField] BoolEventChannelSO solveQuestionSuccessChannel;
        [SerializeField] Keypad keypad;
        [SerializeField] TMP_Text txt_InputField = null;
        [SerializeField] TMP_Text txt_Question = null;
        [SerializeField] CustomButton btn_GenerateQuestion;
        [SerializeField] CustomButton btn_Back;
        [SerializeField] float deleteDuration;
        [SerializeField] float waitDeleteDuration;
        [SerializeField] Image timerFillImage;
        [SerializeField] Timer generateQuestionTimer = new Timer(10f);
        Timer deleteTimer;
        Timer waitDeleteStartTimer;

        [SerializeField] int maxValueOfAnswer = 999;
        [SerializeField] int InputFiedlMaxTextLenght = 7;
        ArithmeticOperation operation;
        bool deleteStarted = false;
        BlackboardUI blackboardUI;

        void Awake()
        {
            keypad.SetListener(this);
        }

        void Update()
        {
            if (isActive == false) return;

            if (generateQuestionTimer.Update(Time.deltaTime))
            {
                txt_InputField.text = string.Empty;
                GenerateQuestion();
            }

            timerFillImage.fillAmount = 1f - generateQuestionTimer.NormalizedTime;
            
            if (deleteStarted == false || waitDeleteStartTimer.Update(Time.deltaTime) == false || deleteTimer.Update(Time.deltaTime) == false) return;

            Delete();
            var newDuration = deleteTimer.Duration - 0.025f;
            newDuration = newDuration < 0 ? 0 : newDuration;
            deleteTimer = new Timer(newDuration);
        }

        void OnEnable()
        {
            btn_Back.RegisterOnClick(OnBackPressed);
        }

        void OnDisable()
        {
            btn_Back.UnregisterOnClick();
        }

        public override void Show()
        {
            base.Show();
            CursorManager.UnlockCursor();
            InputManager.PageUI.SetCallbacks(this);
            InputManager.EarnNumberUI.SetCallbacks(this);
            InputManager.EarnNumberUI.Enable();
            InputManager.PageUI.Enable();
            InputManager.Keypad.Enable();
            keypad.Enable();
            
            txt_InputField.text = "";
            btn_GenerateQuestion.onClick.AddListener(GenerateQuestion);
            GenerateQuestion();
            earnNumberPageEventChannel.RaiseEvent(this);
        }

        public override void Hide()
        {
            base.Hide();
            InputManager.EarnNumberUI.Disable();
            InputManager.PageUI.Disable();
            InputManager.Keypad.Disable();
            keypad.Disable();

            btn_GenerateQuestion.onClick.RemoveListener(GenerateQuestion);
            earnNumberPageEventChannel.RaiseEvent(this);
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

            txt_InputField.text += value;
        }

        void GenerateQuestion()
        {
            var operationType = Random.Range(0, 2) == 0 ? ArithmeticOperationType.Add : ArithmeticOperationType.Subtract;
            operation.GenerateQuestion(operationType, maxValueOfAnswer);

            txt_Question.text = $"{operation.number1} {operation.GetOperator()} {operation.number2} = ?";
            generateQuestionTimer.Restart();
        }

        void Delete()
        {
            if (txt_InputField.text.Length == 0) return;

            txt_InputField.text = txt_InputField.text.Remove(txt_InputField.text.Length - 1);
        }

        void ShowAnswerOnInputField()
        {
            if (txt_InputField.text.Length == 0)
            {
                blackboardUI.ShowWarning(OperationWarnings.ENTER_A_NUMBER);
                return;
            }
            var answer = int.Parse(txt_InputField.text);
            if (answer != operation.CalculateAnswer())
            {
                txt_InputField.text = "";
                blackboardUI.ShowWarning(OperationWarnings.WRONG_ANSWER);
                solveQuestionSuccessChannel.RaiseEvent(false);
                return;
            }
            var number = Random.Range(0, 10);
            if (blackboardUI.AddNumberToInventory(number) == false)
            {
                blackboardUI.ShowWarning("Inventory is full");
                return;
            }
            GenerateQuestion();
            txt_InputField.text = "";

            blackboardUI.ShowWarning("You earned " + number);
            solveQuestionSuccessChannel.RaiseEvent(true);
        }

        void PlayerControls.IPageUIActions.OnBack(InputAction.CallbackContext context)
        {
            if (context.performed) OnBackPressed();
        }

        void PlayerControls.IEarnNumberUIActions.OnGenerateQuestion(InputAction.CallbackContext context)
        {
            if (context.performed) GenerateQuestion();
        }

        void IKeypadListener.OnEnter() => ShowAnswerOnInputField();
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
    }
}