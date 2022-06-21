using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sound_Manager : MonoBehaviour
{

    protected abstract void PlayOneShot(AudioSource Source, AudioClip clip);

    protected abstract void PlayContinuously(AudioSource Source, AudioClip clip);

}
