using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using LessonIsMath.ScriptableObjects.ChannelSOs;

namespace LessonIsMath.Timelines
{
    [RequireComponent(typeof(PlayableDirector))]
    public class TimelineManager : MonoBehaviour
    {
        [SerializeField] VoidEventChannelSO closeupTransitionChannel;
        [SerializeField] TimelineAsset closeupTransitionTimeline;
        PlayableDirector director;
        CinemachineBrain cinemachineBrain;

        void Awake()
        {
            director = GetComponent<PlayableDirector>();
            cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        }

        void OnEnable()
        {
            closeupTransitionChannel.OnEventRaised += MakeTransitionToCloseup;
        }

        void OnDisable()
        {
            closeupTransitionChannel.OnEventRaised -= MakeTransitionToCloseup;
        }

        void MakeTransitionToCloseup()
        {
            if (director.state == PlayState.Playing) return;
            director.playableAsset = closeupTransitionTimeline;
            // Bind tracks
            TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;
            foreach (TrackAsset outputTrack in timelineAsset.GetOutputTracks())
            {
                foreach (PlayableBinding playableBinding in outputTrack.outputs)
                {
                    director.SetGenericBinding(playableBinding.sourceObject, cinemachineBrain);
                    break;
                }
            }

            director.Play();
        }

        // Update is called once per frame
#if UNITY_EDITOR
        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.B) == false || director.state == PlayState.Playing) return;
            MakeTransitionToCloseup();
        }
#endif
    }
}