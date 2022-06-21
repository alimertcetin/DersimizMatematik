using UnityEngine;
using System;


[RequireComponent(typeof(AudioSource))]
public class Door_Animation_Sound : Sound_Manager
{
    AudioSource Source;
    [SerializeField] AudioClip[] DoorOpeningClips = null;
    [SerializeField] AudioClip[] DoorClosingClips = null;
    Door_Animation door_Animation;

    private void Awake()
    {
        Source = GetComponent<AudioSource>();
        door_Animation = GetComponentInParent<Door_Animation>();
        if (door_Animation == null)
        {
            door_Animation = GetComponentInChildren<Door_Animation>();
            if (door_Animation == null) Debug.LogError(this.name + " Animation scriptine ulaşamadı. Sesler düzgün oynatılamayabilir.");
        }
    }

    protected override void PlayOneShot(AudioSource Source, AudioClip clip)
    {
        Source.PlayOneShot(clip);
    }

    protected override void PlayContinuously(AudioSource Source, AudioClip clip)
    {
        //throw new NotImplementedException();
    }

    public void PlayOpeningClip()
    {
        AudioClip _clip = DoorOpeningClips[UnityEngine.Random.Range(0, DoorOpeningClips.Length)];
        var TempPitch = Source.pitch;

        Source.pitch = door_Animation.Rnd_AnimSpeed;
        PlayOneShot(Source, _clip);
        Source.pitch = TempPitch;
    }

    public void PlayClosingClip()
    {
        AudioClip _clip = DoorClosingClips[UnityEngine.Random.Range(0, DoorClosingClips.Length)];
        var TempPitch = Source.pitch;

        Source.pitch = door_Animation.Rnd_AnimSpeed;
        PlayOneShot(Source, _clip);
        Source.pitch = TempPitch;
    }

}
