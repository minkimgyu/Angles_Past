using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class ColorChangeComponent : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    CancellationTokenSource _source = new();

    float m_duration;

    float smoothness = 0.01f;

    Color originColor;
    Color finalColor;

    // Start is called before the first frame update
    public void Init(float duration)
    {
        m_duration = duration;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tmpTr = transform.GetChild(i);
            if (tmpTr.CompareTag("InnerSprite") == false) continue;

            spriteRenderer = tmpTr.GetComponent<SpriteRenderer>();
            break;
        }

        if (spriteRenderer == null) return;

        originColor = spriteRenderer.color;
        finalColor = new Color(originColor.r, originColor.g, originColor.b, 1);
    }


    public async UniTaskVoid ColorChangeTask()
    {
        if (spriteRenderer == null) return;

        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / m_duration; //The amount of change to apply.
        while (progress < 1)
        {
            spriteRenderer.color = Color.Lerp(originColor, finalColor, progress);
            progress += increment;
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: _source.Token);
        }
    }

    public void ResetColor()
    {
        if (spriteRenderer == null) return;

        spriteRenderer.color = originColor;
    }

    public void QuickEndTask()
    {
        _source.Cancel();
        _source.Dispose();
        _source = new(); 
    }

    private void OnDestroy()
    {
        _source.Cancel();
        _source.Dispose();
    }

    private void OnEnable()
    {
        if (_source != null)
            _source.Dispose();

        _source = new();
    }

    private void OnDisable()
    {
        _source.Cancel();
    }
}
