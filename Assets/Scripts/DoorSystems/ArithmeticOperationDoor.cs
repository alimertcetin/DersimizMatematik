using System;
using LessonIsMath.InteractionSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI;
using UnityEngine;
using XIV.SaveSystems;
using XIV.Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LessonIsMath.DoorSystems
{
    [RequireComponent(typeof(DoorManager))]
    public class ArithmeticOperationDoor : MonoBehaviour, ISaveable, IUIEventListener
    {
        [SerializeField] DoorEventChannelSO lockedDoorUIChannel;
        [SerializeField] ArithmeticOperation arithmeticOperation;
        [SerializeField] int maxValueOfAnswer;
        bool questionSolved;
        DoorManager doorManager;

        void Awake()
        {
            doorManager = GetComponent<DoorManager>();
        }

        public bool IsQuestionSolved()
        {
            return questionSolved;
        }

        public bool SolveQuestion(int answer)
        {
#if UNITY_EDITOR
            if (questionSolved)
            {
                Debug.LogError("Should not reach here");
                return true;
            }
#endif
            questionSolved = arithmeticOperation.CalculateAnswer() == answer;
            if (questionSolved) doorManager.RefreshDoorState();
            return questionSolved;
        }

        public string GetQuestionString() => arithmeticOperation.ToString();

        public void OnInteract()
        {
            lockedDoorUIChannel.RaiseEvent(this, true);
            UIEventSystem.Register<LockedDoor_UI>(this);
        }

        void IUIEventListener.OnShowUI(GameUI ui) { }

        void IUIEventListener.OnHideUI(GameUI ui)
        {
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
            GetComponent<DoorManager>().RefreshDoorState();
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