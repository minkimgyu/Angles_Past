using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionDesignation : MonoBehaviour
{
    Transform posTr;
    bool isFix;

    public virtual void Init(Transform posTr)
    {
        this.posTr = posTr;
        isFix = true;
    }

    public virtual void Init(Vector3 posVec)
    {
        transform.position = posVec;
        isFix = false;
    }

    protected virtual void Update()
    {
        if (isFix == true) transform.position = posTr.position;
    }

    protected virtual void DisableObject()
    {
        gameObject.SetActive(false);
        posTr = null;
        isFix = false;
    }
}

// Factory�� �����ϴ� ��� �����ڷ� �ʱ�ȭ
// �ƴϸ� AddState �Լ� ���

public class SoundPlayer : PositionDesignation
{
    AudioSource audioSource;

    private void Awake() => audioSource = GetComponent<AudioSource>();

    public override void Init(Transform posTr)
    {
        base.Init(posTr);
        ResetSource();
    }

    public override void Init(Vector3 posVec)
    {
        base.Init(posVec);
        ResetSource();
    }

    public void Play(Vector3 pos, float ratio, Sound sound) // loop, transform�� �߰�������
    {
        Init(pos);

        audioSource.volume = ratio;
        audioSource.clip = sound.AudioClip;
        audioSource.Play();

        Invoke("DisableObject", audioSource.clip.length + 0.1f);
    }

    // trasform�� ���, �پ ����ٴϱ� ������ loop�� �ɾ��ش�.
    public void Play(Transform transform, float ratio, Sound sound) // loop, transform�� �߰�������
    {
        Init(transform);

        audioSource.volume = ratio;
        audioSource.clip = sound.AudioClip;
        //audioSource.loop = true;
        audioSource.Play();

        Invoke("DisableObject", audioSource.clip.length + 0.1f); // �ð� ������ ����
    }

    void ResetSource() => audioSource.clip = null;

    private void OnDisable()
    {
        CancelInvoke();
        ObjectPooler.ReturnToPool(gameObject);
    }
}
