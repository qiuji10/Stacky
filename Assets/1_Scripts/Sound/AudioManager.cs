using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource SFX_Source, BGM_Source;


    [SerializeField] float bgmVolume;
    [SerializeField] float sfxVolume;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public void SetVolume()
    {
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        SFX_Source.volume = sfxVolume;
    }

    public void SetBGMVolume()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        BGM_Source.volume = bgmVolume;
    }

    SoundFile GetSound(AudioData _audioType, string _name)
    {
        List<SoundFile> temp = new List<SoundFile>(_audioType.audioList);

        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].name == _name)
            {
                return temp[i];
            }
        }

        return null;
    }

    SoundFile GetSound(AudioData _audioType, int _index)
    {
        List<SoundFile> temp = new List<SoundFile>(_audioType.audioList);
        return temp[_index];
    }

    public void PlaySFX(AudioData _audioData, string _name)
    {
        SoundFile sound = GetSound(_audioData, _name);
        if (sound != null)
        {
            SFX_Source.PlayOneShot(sound.clip, sound.volume);
        }
    }

    public void PlaySFX(AudioData _audioData, int _num)
    {
        SoundFile sound = GetSound(_audioData, _num);
        if (sound != null)
        {
            SFX_Source.PlayOneShot(sound.clip, sound.volume);
        }
    }

    public void PlayBGM(AudioData _audioData, string _name, bool loop = true)
    {
        SoundFile sound = GetSound(_audioData, _name);
        if (sound != null)
        {
            BGM_Source.volume = sound.volume;
            BGM_Source.clip = sound.clip;
            BGM_Source.loop = loop;

            BGM_Source.Play();
        }
    }

    public void StopBGM()
    {
        BGM_Source.Stop();
    }

    public void StopAllSound()
    {
        SFX_Source.Stop();
        BGM_Source.Stop();
    }
}
