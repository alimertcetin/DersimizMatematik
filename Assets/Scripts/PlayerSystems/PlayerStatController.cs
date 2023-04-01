using LessonIsMath.StatSystems.Stats;
using UnityEngine;
using XIV.EventSystem;
using XIV.EventSystem.Events;

namespace LessonIsMath.PlayerSystems
{
    public class PlayerStatController : MonoBehaviour
    {
        [SerializeField] BrainPowerStat brainPowerStat;

        // TODO : Create PreOnEnable and LateOnEnable callbacks
        void OnEnable()
        {
            brainPowerStat.PreOnEnable();
            XIVEventSystem.SendEvent(new InvokeAfterEvent(0.5f).OnCompleted(() => brainPowerStat.OnEnable()));
        }

        void OnDisable()
        {
            brainPowerStat.OnDisable();
        }
    }
}