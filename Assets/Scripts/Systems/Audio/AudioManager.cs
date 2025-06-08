using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private AudioClip Miss_SFX;

    private void Start()
    {
        _sfxSource.loop = false;
        _musicSource.loop = true;

        LoadAudioSettings();
    }

    private void LoadAudioSettings()
    {
        var audioSettings = SettingsManager.Instance.LoadSettings<AudioSettings>();
        if (audioSettings != null)
        {
            _audioMixer.SetFloat("MusicVolume", AudioSettings.LinearToDecibel(audioSettings.MusicVolume));
            _audioMixer.SetFloat("MasterVolume", AudioSettings.LinearToDecibel(audioSettings.MasterVolume));
            _audioMixer.SetFloat("SFXVolume", AudioSettings.LinearToDecibel(audioSettings.SFXVolume));
        }
    }

    public void PlaySFX(AudioClip fxSound, float duration = 0)
    {
        var rndPitch = UnityEngine.Random.Range(0.85f, 1.15f);
        _sfxSource.pitch = rndPitch;
        _sfxSource.PlayOneShot(fxSound);
        if (duration > 0)
            StartCoroutine(StopAfterDuration(duration));
    }

    private IEnumerator StopAfterDuration(float duration)
    {
        yield return WaitManager.Wait(duration);
        StartCoroutine(FadeOut(0.2f));
    }

    private IEnumerator FadeOut(float fadeOutDuration)
    {
        float currentDuration = 0;
        var originalVolume = _sfxSource.volume;
        while (currentDuration < fadeOutDuration)
        {
            currentDuration += Time.deltaTime;
            _sfxSource.volume = Mathf.Lerp(originalVolume, 0, currentDuration / fadeOutDuration);
            yield return null;
        }
        _sfxSource.Stop();
        _sfxSource.volume = originalVolume;
    }

    public void PlayBackgroundMusic(AudioClip music)
    {
        _musicSource.clip = music;
        _musicSource.Play();
    }
    public void PlayMissSFX()
    {
        _sfxSource.PlayOneShot(Miss_SFX);
    }
}