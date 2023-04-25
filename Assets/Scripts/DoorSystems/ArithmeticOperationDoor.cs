using System;
using LessonIsMath.InteractionSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI;
using UnityEngine;
using XIV.Core.Utils;
using XIV.EventSystem;
using XIV.SaveSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LessonIsMath.DoorSystems
{
    [RequireComponent(typeof(DoorManager))]
    public class ArithmeticOperationDoor : MonoBehaviour, ISaveable, IUIEventListener
    {
        [SerializeField] ArithmeticDoorEventChannelSO arithmeticDoorQuestionTimerChannel;
        [SerializeField] ArithmeticDoorEventChannelSO arithmeticDoorUIChannel;
        [SerializeField] ArithmeticOperation arithmeticOperation;
        [SerializeField] int maxValueOfAnswer;
        public float generateQuestionDuration;
        bool questionSolved;
        DoorManager doorManager;
        IEvent generateQuestionEvent;

        void Awake()
        {
            doorManager = GetComponent<DoorManager>();
        }

        public bool IsQuestionSolved() => questionSolved;

        public bool SolveQuestion(int answer)
        {
            questionSolved = arithmeticOperation.CalculateAnswer() == answer;
            return questionSolved;
        }

        public void GenerateNewQuestion()
        {
            arithmeticOperation.GenerateQuestion((ArithmeticOperationType)UnityEngine.Random.Range(0, 2), maxValueOfAnswer);
            for (int i = 0; i < 100; i++)
            {
                var answer = arithmeticOperation.answer;
                var num1 = arithmeticOperation.number1;
                var num2 = arithmeticOperation.number2;
                if ((answer != num1 || answer != num2) && answer != 0) break;
                arithmeticOperation.GenerateQuestion((ArithmeticOperationType)UnityEngine.Random.Range(0, 2), maxValueOfAnswer);
            }
        }

        public string GetQuestionString() => arithmeticOperation.ToString();

        public void OnInteract()
        {
            arithmeticDoorUIChannel.RaiseEvent(this, true);
            UIEventSystem.Register<LockedDoor_UI>(this);
        }

        void IUIEventListener.OnShowUI(GameUI ui) { }

        void IUIEventListener.OnHideUI(GameUI ui)
        {
            arithmeticDoorQuestionTimerChannel.RaiseEvent(this, questionSolved == false);
            UIEventSystem.Unregister<LockedDoor_UI>(this);
            doorManager.OnInteractionEnd();
        }

        #region --- Save ---

        [Serializable]
        struct SaveData
        {
            public bool questionSolved;
        }

        object ISaveable.CaptureState()
        {
            return new SaveData
            {
                questionSolved = questionSolved,
            };
        }

        void ISaveable.RestoreState(object state)
        {
            SaveData saveData = (SaveData)state;
            questionSolved = saveData.questionSolved;
            doorManager.RefreshDoorState();
        }

        #endregion

#if UNITY_EDITOR

        [ContextMenu(nameof(CalculateAnswer))]
        void CalculateAnswer()
        {
            Undo.RecordObject(this, gameObject.name + nameof(CalculateAnswer));
            arithmeticOperation.CalculateAnswer();
            EditorUtility.SetDirty(this);
        }

        [ContextMenu(nameof(GenerateQuestionDependingOnAnswer))]
        void GenerateQuestionDependingOnAnswer()
        {
            Undo.RecordObject(this, gameObject.name + nameof(GenerateQuestionDependingOnAnswer));
            arithmeticOperation.GenerateQuestion(arithmeticOperation.answer, maxValueOfAnswer);
            EditorUtility.SetDirty(this);
        }

        [ContextMenu(nameof(GenerateRandomQuestion))]
        void GenerateRandomQuestion()
        {
            Undo.RecordObject(this, gameObject.name + nameof(GenerateRandomQuestion));
            arithmeticOperation.GenerateQuestion(maxValueOfAnswer);
            EditorUtility.SetDirty(this);
        }

#endif
    }
}