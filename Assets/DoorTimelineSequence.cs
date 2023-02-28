using System.Collections;
using System.Collections.Generic;
using LessonIsMath.PlayerSystems;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DoorTimelineSequence : MonoBehaviour
{
    PlayableDirector director;
    public Animator[] animators;
    
    void Awake()
    {
        animators = new Animator[3];
        director = GetComponent<PlayableDirector>();
        animators[2] = FindObjectOfType<PlayerController>().GetComponent<Animator>();
        animators[1] = animators[2].transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // director.RebindPlayableGraphOutputs();
        // director.SetGenericBinding();
        if (Input.GetKeyDown(KeyCode.B) == false || director.state == PlayState.Playing) return;

        int count = 0;
        TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;
        using (IEnumerator<TrackAsset> outputTracks = timelineAsset.GetOutputTracks().GetEnumerator())
        {
            while (outputTracks.MoveNext())
            {
                TrackAsset current = outputTracks.Current;
                
                foreach (PlayableBinding playableBinding in current.outputs)
                {
                    director.SetGenericBinding(playableBinding.sourceObject, animators[count++]);
                    break;
                }
            }
        }
        
        director.Play();
        director.played += OnPlayed;

        void OnPlayed(PlayableDirector playableDirector)
        {
            playableDirector.played -= OnPlayed;
            Debug.Log("Director has finished playing");
        }


        // var obj = new SerializedObject(director);
        // var bindings = obj.FindProperty("m_SceneBindings");
        // for (int i = 0; i < bindings.arraySize; i++)
        // {
        //     SerializedProperty binding = bindings.GetArrayElementAtIndex(i);
        //     SerializedProperty trackProp = binding.FindPropertyRelative("key");
        //     SerializedProperty sceneObjProp = binding.FindPropertyRelative("value");
        //     Object track = trackProp.objectReferenceValue;
        //     Object sceneObj = sceneObjProp.objectReferenceValue;
        //
        //     Debug.LogFormat("Binding {0} {1}", track != null ? track.name : "Null", sceneObj != null ? sceneObj.name : "Null");
        // }
    }
}
