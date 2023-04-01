using System.Collections.Generic;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI;
using UnityEngine;
using XIV.EventSystem;
using XIV.EventSystem.Events;

namespace LessonIsMath.StatSystems.Stats
{
    [System.Serializable]
    public class BrainPowerStat : IStat
    {
        [SerializeField] StatEventChannelSO brainPowerStatLoadedChannel;
        [SerializeField] PageUIEventChannelSO earnNumberPageEventChannel;
        [SerializeField] PageUIEventChannelSO makeOperationPageEventChannel;
        [SerializeField] StatData statData;
        bool uiStatusChangedFlag;
        List<IStatListener> statListeners = new List<IStatListener>(2);
        
        public void PreOnEnable()
        {
            StartIncrease();
            earnNumberPageEventChannel.OnEventRaised += HandlePageEvents;
            makeOperationPageEventChannel.OnEventRaised += HandlePageEvents;
        }

        public void OnEnable()
        {
            brainPowerStatLoadedChannel.RaiseEvent(this);
        }

        public void OnDisable()
        {
            earnNumberPageEventChannel.OnEventRaised -= HandlePageEvents;
            makeOperationPageEventChannel.OnEventRaised -= HandlePageEvents;
        }

        void HandlePageEvents(PageUI pageUI)
        {
            uiStatusChangedFlag = !uiStatusChangedFlag;
            if (pageUI.isActive) StartDecrease();
            else StartIncrease();
        }

        void SetCurrent(float value)
        {
            statData.current = Mathf.Clamp(value, statData.min, statData.max);
            InformListeners();
        }

        void StartDecrease()
        {
            var currentStatus = uiStatusChangedFlag;
            XIVEventSystem.SendEvent(new InvokeUntilEvent().AddAction(() => SetCurrent(Mathf.MoveTowards(statData.current, statData.min, statData.decreaseSpeed * Time.deltaTime)))
                .AddCancelCondition(() => Mathf.Abs(statData.current - statData.min) < Mathf.Epsilon || currentStatus != uiStatusChangedFlag));
        }

        void StartIncrease()
        {
            var currentStatus = uiStatusChangedFlag;
            XIVEventSystem.SendEvent(new InvokeUntilEvent().AddAction(() => SetCurrent(Mathf.MoveTowards(statData.current, statData.max, statData.increaseSpeed * Time.deltaTime)))
                .AddCancelCondition(() => Mathf.Abs(statData.max - statData.current) < Mathf.Epsilon || currentStatus != uiStatusChangedFlag));
        }

        StatData IStat.GetStatData() => statData;
        void IStat.SetStatData(StatData newStatData)
        {
            statData = newStatData;
            SetCurrent(statData.current);
        }

        void IStat.Register(IStatListener statListener)
        {
            statListeners.Add(statListener);
        }

        void IStat.Unregister(IStatListener statListener)
        {
            statListeners.Remove(statListener);
        }

        void InformListeners()
        {
            for (int i = 0; i < statListeners.Count; i++)
            {
                statListeners[i].OnStatChanged(this);
            }
        }
    }
}