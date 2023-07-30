using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(Vector3 pos, float ratio, Sound sound)
    {
        ResetSource();

        transform.position = pos;
        audioSource.volume = ratio;
        audioSource.clip = sound.AudioClip;
        audioSource.Play();

        Invoke("DisableObject", audioSource.clip.length + 0.1f);
    }

    void ResetSource() => audioSource.clip = null;

    void DisableObject() => gameObject.SetActive(false);

    private void OnDisable()
    {
        CancelInvoke();
        ObjectPooler.ReturnToPool(gameObject);
    }
}
