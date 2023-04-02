using System;
using System.Collections.Generic;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.StatSystems;
using LessonIsMath.StatSystems.ScriptableObjects;
using LessonIsMath.StatSystems.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI;
using LessonIsMath.StatSystems.Drivers;
using UnityEngine;

namespace LessonIsMath.PlayerSystems
{
    public class StatManager : MonoBehaviour, IStatContainerListener
    {
        [SerializeField] StatContainerChannelSO statContainerLoadedChannel;
        [SerializeField] StatContainerChangeChannelSO statContainerChangeChannel;
        [SerializeField] VoidEventChannelSO onSceneReady;
        [SerializeField] StatContainerSO statContainerSO;
        [SerializeField] PageUIEventChannelSO earnNumberPageEventChannel;
        [SerializeField] PageUIEventChannelSO makeOperationPageEventChannel;
        
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
            statContainer.AddListener(this);
        }

        void OnDisable()
        {
            earnNumberPageEventChannel.OnEventRaised -= HandleBlackboardPageEvents;
            makeOperationPageEventChannel.OnEventRaised -= HandleBlackboardPageEvents;
            onSceneReady.OnEventRaised -= OnSceneReady;
            statContainer.RemoveListener(this);
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
    }
}