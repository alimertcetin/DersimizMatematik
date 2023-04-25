using System.Collections.Generic;
using LessonIsMath.DoorSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using XIV.Core.Utils;

namespace LessonIsMath.UI
{
    public class DoorQuestionTimerUI : MonoBehaviour
    {
        [SerializeField] ArithmeticDoorEventChannelSO arithmeticDoorQuestionTimerChannel;
        [SerializeField] QuestionTimerIndicator questionTimerIndicatorPrefab;
        Dictionary<ArithmeticOperationDoor, QuestionTimerIndicator> kvp = new();

        void OnEnable()
        {
            arithmeticDoorQuestionTimerChannel.OnEventRaised += OnQuestionTimerRaised;
        }

        void OnDisable()
        {
            arithmeticDoorQuestionTimerChannel.OnEventRaised -= OnQuestionTimerRaised;
        }

        public void RemoveKey(ArithmeticOperationDoor door) => OnQuestionTimerRaised(door, false);
        
        void OnQuestionTimerRaised(ArithmeticOperationDoor door, bool value)
        {
            if (value == false)
            {
                if (kvp.TryGetValue(door, out var indicator) == false) return;
                kvp.Remove(door);
                Destroy(indicator.gameObject);
            }
            else
            {
                if (kvp.ContainsKey(door)) return;
            
                var indicator = Instantiate(questionTimerIndicatorPrefab, Vector3.zero, Quaternion.identity, this.transform).GetComponent<QuestionTimerIndicator>();
                indicator.doorQuestionTimerUI = this;
                indicator.arithmeticOperationDoor = door;
                indicator.timer = new Timer(door.generateQuestionDuration);
                kvp.Add(door, indicator);                
            }

        }
    }
}
