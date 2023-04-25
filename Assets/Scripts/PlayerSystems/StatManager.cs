using System;
using System.Collections.Generic;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.StatSystems;
using LessonIsMath.StatSystems.ScriptableObjects;
using LessonIsMath.StatSystems.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI;
using LessonIsMath.StatSystems.Drivers;
using LessonIsMath.StatSystems.Stats;
using UnityEngine;
using XIV.SaveSystems;
using XIV.ScriptableObjects.Channels;

namespace LessonIsMath.PlayerSystems
{
    [RequireComponent(typeof(SaveableEntity))]
    public class StatManager : MonoBehaviour, IStatContainerListener, ISaveable
    {
        [SerializeField] StatContainerChannelSO statContainerLoadedChannel;
        [SerializeField] StatContainerChangeChannelSO statContainerChangeChannel;
        [SerializeField] StatContainerSO statContainerSO;
        [SerializeField] PageUIEventChannelSO earnNumberPageEventChannel;
        [SerializeField] PageUIEventChannelSO makeOperationPageEventChannel;
        [SerializeField] VoidEventChannelSO onSceneReady;
        [SerializeField] BoolEventChannelSO solveQuestionSuccessChannel;
        
        StatContainer statContainer;
        List<StatDriver> statDrivers;

        void Awake()
        {
            statContainer = statContainerSO.GetStatContainer();
            statDrivers = new List<StatDriver>
            {
                new BrainStatDriver(statContainer) { isDecreasing = false },
            };
        }

        void Start() => statContainerLoadedChannel.RaiseEvent(statContainer);

        void Update()
        {
            for (int i = 0; i < statDrivers.Count; i++)
            {
                statDrivers[i].Update();
            }
        }

        void OnEnable()
        {
            earnNumberPageEventChannel.OnEventRaised += HandleBlackboardPageEvents;
            makeOperationPageEventChannel.OnEventRaised += HandleBlackboardPageEvents;
            onSceneReady.OnEventRaised += OnSceneReady;
            solveQuestionSuccessChannel.OnEventRaised += OnQuestionSolved;
            statContainer.AddListener(this);
        }

        void OnDisable()
        {
            earnNumberPageEventChannel.OnEventRaised -= HandleBlackboardPageEvents;
            makeOperationPageEventChannel.OnEventRaised -= HandleBlackboardPageEvents;
            onSceneReady.OnEventRaised -= OnSceneReady;
            solveQuestionSuccessChannel.OnEventRaised -= OnQuestionSolved;
            statContainer.RemoveListener(this);
        }

        void OnQuestionSolved(bool success)
        {
            if (success)
            {
                statContainer.UpdateStatItemExperience<BrainPowerStatItem>(5f);
                statContainer.UpdateStatItemExperience<BrainCoreStatItem>(5f);
            }
            else
            {
                var statData = statContainer.GetStatData<BrainPowerStatItem>();
                statData.current -= 5f;
                statContainer.UpdateStat<BrainPowerStatItem>(statData);
            }
        }

        void HandleBlackboardPageEvents(PageUI pageUI)
        {
            GetDriver<BrainStatDriver>().isDecreasing = pageUI.isActive;
        }

        T GetDriver<T>() where T : StatDriver
        {
            int count = statDrivers.Count;
            for (int i = 0; i < count; i++)
            {
                if (statDrivers[i] is T driver) return driver;
            }

            return default;
        }

        void OnSceneReady()
        {
            statContainerLoadedChannel.RaiseEvent(statContainer);
        }

        void IStatContainerListener.OnStatContainerChanged(StatContainerChange statContainerChange)
        {
            statContainerChangeChannel.RaiseEvent(statContainerChange);
        }

        #region ---Save---

        [System.Serializable]
        struct SaveData
        {
            public StatItemBase[] statItems;
        }
        
        object ISaveable.CaptureState()
        {
            StatItemBase[] statItems = new StatItemBase[statContainer.Count];
            for (int i = 0; i < statContainer.Count; i++)
            {
                statItems[i] = statContainer[i].StatItem;
            }

            return new SaveData { statItems = statItems, };
        }

        void ISaveable.RestoreState(object state)
        {
            var saveData = (SaveData)state;
            for (int i = 0; i < statContainer.Count; i++)
            {
                statContainer.RemoveAt(i, false);
            }

            int length = saveData.statItems.Length;
            for (int i = 0; i < length - 1; i++)
            {
                statContainer.Add(saveData.statItems[i], false);
            }

            statContainer.Add(saveData.statItems[^1], true);
        }
        
        #endregion
    }
}