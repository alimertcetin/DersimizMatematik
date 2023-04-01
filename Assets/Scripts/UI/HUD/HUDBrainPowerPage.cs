using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.StatSystems;
using LessonIsMath.UI;
using UnityEngine;
using UnityEngine.UI;

namespace XIV.UI
{
    public class HUDBrainPowerPage : PageUI, IStatListener
    {
        [SerializeField] StatEventChannelSO brainPowerStatLoadedChannel;
        [SerializeField] Image fillArea;

        void OnEnable()
        {
            brainPowerStatLoadedChannel.OnEventRaised += OnStatLoaded;
        }

        void OnDisable()
        {
            brainPowerStatLoadedChannel.OnEventRaised -= OnStatLoaded;
        }

        void OnStatLoaded(IStat stat)
        {
            stat.Register(this);
            UpdateUI(stat);
        }

        void UpdateUI(IStat stat)
        {
            StatData statData = stat.GetStatData();
            fillArea.transform.localScale = new Vector3(statData.normalizedCurrent, 1, 1);
        }

        void IStatListener.OnStatChanged(IStat stat)
        {
            UpdateUI(stat);
        }
    }
}