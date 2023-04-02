using LessonIsMath.UI;
using LessonIsMath.StatSystems;
using LessonIsMath.StatSystems.ScriptableObjects.ChannelSOs;
using LessonIsMath.StatSystems.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace XIV.UI
{
    public class HUDBrainPowerPage : PageUI
    {
        [SerializeField] StatContainerChannelSO statContainerLoadedChannel;
        [SerializeField] StatContainerChangeChannelSO statContainerChangedChannel;
        [SerializeField] Image fillArea;
        [SerializeField] Image brainFillImage;

        StatContainer statContainer;

        void OnEnable()
        {
            statContainerLoadedChannel.Register(OnStatContainerLoaded);
            statContainerChangedChannel.Register(OnStatContainerChanged);
        }

        void OnDisable()
        {
            statContainerLoadedChannel.Unregister(OnStatContainerLoaded);
            statContainerChangedChannel.Unregister(OnStatContainerChanged);
        }

        void OnStatContainerLoaded(StatContainer statContainer)
        {
            this.statContainer = statContainer;
        }

        void OnStatContainerChanged(StatContainerChange statContainerChange)
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            UpdateBrainPower(statContainer.GetStatData<BrainPowerStatItem>());
            UpdateBrainCore(statContainer.GetStatData<BrainCoreStatItem>());
        }

        void UpdateBrainPower(StatData statData)
        {
            fillArea.transform.localScale = new Vector3(statData.normalizedCurrent, 1, 1);
        }

        void UpdateBrainCore(StatData statData)
        {
            brainFillImage.fillAmount = statData.normalizedCurrent;
        }
    }
}