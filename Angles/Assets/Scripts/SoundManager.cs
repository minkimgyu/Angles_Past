using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public Sound(string name, AudioClip audioClip)
    {
        this.name = name;
        this.audioClip = audioClip;
    }

    [SerializeField]
    string name;
    public string Name { get { return name; } }

    [SerializeField]
    AudioClip audioClip;
    public AudioClip AudioClip { get { return audioClip; } }
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    List<Sound> sounds;

    AudioSource bgmPlayer;

    [SerializeField]
    AudioSource[] sfxPlayer;

    [SerializeField]
    float sfxMasterVolume = 1;
    public float SfxMasterVolume 
    { 
        get { return sfxMasterVolume; } 
        set 
        { 
            sfxMasterVolume = value;
            for (int i = 0; i < sfxPlayer.Length; i++)
            {
                sfxPlayer[i].volume = sfxMasterVolume;
            }
        } 
    }

    [SerializeField]
    float bgmMasterVolume = 1;
    public float BgmMasterVolume { get { return bgmPlayer.volume; }  set { bgmPlayer.volume = value; } }

    protected override void Awake()
    {
        base.Awake();
        bgmPlayer = GetComponent<AudioSource>();
    }

    public void StopBGM()
    {
        if (bgmPlayer.isPlaying == true) bgmPlayer.Stop();
    }

    public void PlayBGM(string name)
    {
        Sound sound = sounds.Find(x => x.Name == name);
        if (sound == null) return;

        StopBGM();

        bgmPlayer.clip = sound.AudioClip;
        bgmPlayer.Play();
    }

    public void PlaySFX(string name, float sfxVolume = 1)
    {
        Sound sound = sounds.Find(x => x.Name == name);
        if (sound == null) return;

        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            if(sfxPlayer[i].isPlaying == false)
            {
                sfxPlayer[i].clip = sound.AudioClip;
                sfxPlayer[i].Play();
                break;
            }
        }
    }

    public void PlaySFX(Vector3 pos, string name, float sfxVolume = 1)
    {
        SoundPlayer player = ObjectPooler.SpawnFromPool<SoundPlayer>("SfxPlayer");
        if (player == null) return;

        Sound sound = sounds.Find(x => x.Name == name);
        if (sound == null) return;

        player.Play(pos, sfxVolume * sfxMasterVolume, sound);
    }

    public void PlaySFX(Transform transform, string name, float sfxVolume = 1)
    {
        SoundPlayer player = ObjectPooler.SpawnFromPool<SoundPlayer>("SfxPlayer");
        if (player == null) return;

        Sound sound = sounds.Find(x => x.Name == name);
        if (sound == null) return;

        player.Play(transform, sfxVolume * sfxMasterVolume, sound);
    }

    public void SetBGMVolume(float ratio)
    {
        bgmPlayer.volume = ratio;
    }
}
