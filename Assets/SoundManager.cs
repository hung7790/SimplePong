using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    private AudioSource Audio;
    public AudioClip[] sounds;

    void Start()
    {
        Audio = gameObject.GetComponent<AudioSource>();
    }

    public void SoundEvent(int soundId)
    {
        if (sounds.Length < soundId) return;
        if (soundId < 0) return;
        if (Audio == null) return;
        Audio.clip = sounds[soundId];
        Audio.mute = false;
        Audio.Play();
    }
}
