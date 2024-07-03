using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] Sound[] array_sfx = null;
    [SerializeField] Sound[] array_bgm = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource sfxPlayer = null;
    [SerializeField] AudioSource typeWritePlayer = null;

    Dictionary<string, AudioClip> dic_BGM;
    Dictionary<string, AudioClip> dic_SFX;

    [SerializeField] float bgmVolume;
    [SerializeField] float sfxVolume;

    private void Awake()
    {
        dic_BGM = new Dictionary<string, AudioClip>();
        dic_SFX = new Dictionary<string, AudioClip>();

        foreach (Sound sound in array_bgm)
        {
            dic_BGM.Add(sound.name, sound.clip);
        }

        foreach (Sound sound in array_sfx)
        {
            dic_SFX.Add(sound.name, sound.clip);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayTypeWriteSFX(string sfxName)
    {
        if (!dic_SFX.ContainsKey(sfxName))
        {
            Debug.LogWarning("SoundManager - Sound not found: " + sfxName);
            return;
        }

        typeWritePlayer.clip = dic_SFX[sfxName];
        typeWritePlayer.volume = sfxVolume;

        typeWritePlayer.Play();
    }

    /// <summary>
    /// sfxName 이름의 SFX 재생
    /// </summary>
    /// <param name="sfxName"></param>
    public void PlaySFX(string sfxName)
    {
        if (!dic_SFX.ContainsKey(sfxName))
        {
            Debug.LogWarning("SoundManager - Sound not found: " + sfxName);
            return;
        }

        sfxPlayer.clip = dic_SFX[sfxName];
        sfxPlayer.volume = sfxVolume;

        sfxPlayer.Play();
    }

    /// <summary>
    /// bgmName 이름의 BGM 재생
    /// </summary>
    /// <param name="bgmName"></param>
    public void PlayBGM(string bgmName)
    {
        if (!dic_BGM.ContainsKey(bgmName))
        {
            Debug.LogWarning("SoundManager - Sound not found: " + bgmName);
            return;
        }

        bgmPlayer.clip = dic_BGM[bgmName];
        bgmPlayer.volume = bgmVolume;

        bgmPlayer.Play();
    }

    /// <summary>
    /// BGM 멈춤
    /// </summary>
    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    /// <summary>
    /// SFX 멈춤
    /// </summary>
    public void StopSFX()
    {
        sfxPlayer.Stop();
    }

    /// <summary>
    /// BGM 볼륨 조절 (0 ~ 1)
    /// </summary>
    /// <param name="volume"></param>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);

        bgmPlayer.volume = bgmVolume;
    }

    /// <summary>
    /// SFX 볼륨 조절 (0 ~ 1)
    /// </summary>
    /// <param name="volume"></param>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume * 0.5f);

        sfxPlayer.volume = sfxVolume;
        typeWritePlayer.volume = sfxVolume;
    }

    public float SetBGMVolumeTweening(float _duration)
    {
        float volume = bgmPlayer.volume;

        var bgmTween = DOTween.To(() => bgmVolume, x => bgmVolume = x, 0f, _duration)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                bgmPlayer.volume = bgmVolume;
            });

        return volume;
    }

    /// <summary>
    /// SFX 목록에 해당 SFX 있는지 확인
    /// </summary>
    /// <param name="sfxName"></param>
    /// <returns></returns>
    public bool CheckSFXExist(string sfxName)
    {
        if (dic_SFX.ContainsKey(sfxName)) return true;
        else return false;
    }

    public bool CheckSFXPlayNow()
    {
        return sfxPlayer.isPlaying;
    }

    public bool CheckTypeWriteSFXPlayNow()
    {
        return typeWritePlayer.isPlaying;
    }
}