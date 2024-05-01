using System.Collections.Generic;
using Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgmSource;
    private List<AudioSource> _sfxSources;

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    public float bgmVolume = 1f;
    public float sfxVolume = 1f;

    public bool isMute = false;

    protected override void Awake()
    {
        base.Awake();
        
        bgmSource = gameObject.AddComponent<AudioSource>();
        _sfxSources = new List<AudioSource>();
    }

    public void PlayBGM(int index)
    {
        if (index < 0 || index >= bgmClips.Length)
        {
            Debug.LogError("BGM index out of range");
            return;
        }

        bgmSource.clip = bgmClips[index];
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }
    
    public void PlayBGM(string name)
    {
        for (int i = 0; i < bgmClips.Length; i++)
        {
            if (bgmClips[i].name != name) continue;
            
            PlayBGM(i);
            return;
        }
        Debug.LogError("BGM " + name + " not found");
    }

    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        //null check
        if (clip == null) return;
        
        // �ӽ÷� �ش� ��ġ�� ȿ������ ����ϴ� �ڵ�, ���� ���ӿ�����Ʈ�� ���� �����ؼ� ������ 
        // ������Ʈ Ǯ���� ���� �����ϴ� ������� �����ؾ� ��
        AudioSource.PlayClipAtPoint(clip, position, sfxVolume);
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void Mute()
    {
        isMute = !isMute;
        bgmSource.mute = isMute;
        // sfxSource.mute = isMute;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        // sfxSource.volume = sfxVolume;
    }
}