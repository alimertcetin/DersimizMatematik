using System.Collections;
using Cinemachine;
using LessonIsMath.DoorSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using XIV.Core.Utils;
using CameraType = LessonIsMath.CameraSystems.CameraType;

namespace LessonIsMath.InteractionSystems
{
    [System.Serializable]
    public class KeycardDoorInteraction
    {
        [SerializeField] CameraTransitionEventChannelSO cameraTransitionChannel;
        [SerializeField] Timer timer = new Timer(2.5f);
        bool inInteraction;

        public IEnumerator OnInteractionStart(DoorManager doorManager)
        {
            if (doorManager.GetState().HasFlag(DoorState.RequiresKeycard) == false) yield break;

            inInteraction = true;
            cameraTransitionChannel.RaiseEvent(CameraType.SideViewLeft);
            yield return null;
            
            var activeBrain = CinemachineCore.Instance.GetActiveBrain(0);
            while (timer.Update(Time.deltaTime) == false && activeBrain.IsBlending)
            {
                yield return null;
            }
            timer.Restart();
        }

        public IEnumerator OnInteractionEnd()
        {
            if (inInteraction == false) yield break;
            inInteraction = false;
            
            cameraTransitionChannel.RaiseEvent(CameraType.Character);
            yield return null;
            
            var activeBrain = CinemachineCore.Instance.GetActiveBrain(0);
            while (timer.Update(Time.deltaTime) == false && activeBrain.IsBlending)
            {
                yield return null;
            }
            timer.Restart();
        }
    }
}