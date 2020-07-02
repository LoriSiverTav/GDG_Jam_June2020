using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseSound
{
    public AudioClip audioClip;
    public bool loop = false;

    [Range(min: 0, max: 1)]
    public float volume;

    [HideInInspector]
    public AudioSource audioSource;
}

[System.Serializable]
public class SoundEffect : BaseSound
{
    public SoundManager.SoundName soundName;
}

[System.Serializable]
public class BackgroundTheme : BaseSound
{
    public float pauseVolume = 0.65f;
}

public class SoundManager : MonoBehaviour
{
    public enum SoundName
    {
        lockUnlock, lockClick, footsteps
    };

    public static SoundManager instance;
    public SoundEffect[] soundEffects;
    public BackgroundTheme backgroundSong;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Add a new Audio Source Component to game object for each sound effect with all information set on Unity
        foreach (var s in soundEffects)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.volume = s.volume;
            s.audioSource.loop = s.loop;
        }

        backgroundSong.audioSource = gameObject.AddComponent<AudioSource>();
        backgroundSong.audioSource.clip = backgroundSong.audioClip;
        backgroundSong.audioSource.loop = backgroundSong.loop;
        backgroundSong.audioSource.volume = backgroundSong.volume;

        PlayBackground();
    }

    // Plays the first audio clip that matches with the level index. Will notify if there's no audio clips, or more than one for a scene  
    public void PlayBackground()
    {
        if (!backgroundSong.audioSource.isPlaying)
        {
            backgroundSong.audioSource.Play();
        }
    }

    // Looks for first specified sound-effect by enum name
    public void PlaySound(SoundName soundName)
    {
        SoundEffect soundCombo = soundEffects.FirstOrDefault(x => x.soundName == soundName);

        if (soundCombo == null) { return; }

        soundCombo.audioSource.Play();
    }

    public void LowerBackgroundMusicVolume()
    {
        backgroundSong.audioSource.volume = backgroundSong.pauseVolume;
    }

    public void RaiseBackgroundMusicVolume()
    {
        backgroundSong.audioSource.volume = backgroundSong.volume;
    }
}
