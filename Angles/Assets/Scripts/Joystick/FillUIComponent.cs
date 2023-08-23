using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class FillUIComponent : MonoBehaviour
{
    [SerializeField]
    PlayerActionEventSO playerActionEventSO;

    List<Image> images = new List<Image>();

    private void Awake() => InitImages();

    private void OnEnable()
    {
        playerActionEventSO.OnActionRequested += FillDashIcon;
    }

    private void OnDisable()
    {
        playerActionEventSO.OnActionRequested -= FillDashIcon;
    }

    protected void InitImages()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tr = transform.GetChild(i);
            Image image = tr.GetComponent<Image>();

            if (image == null) continue;

            images.Add(image);
        }
    }

    protected void ResetFillAmount()
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].fillAmount = 0;
        }
    }

    protected void FillDashIcon(float ratio) // 0.66435
    {
        float perImage = (float)1 / images.Count;
        bool nowFillComplete = false;

        for (int i = 1; i < images.Count + 1; i++)
        {
            int indexOfImage = i - 1;

            if (nowFillComplete == true)
            {
                images[indexOfImage].fillAmount = 0;
                continue;
            }

            float perImageRatio = perImage * i;

            if (ratio > perImageRatio)
            {
                images[indexOfImage].fillAmount = 1;
            }
            else
            {
                int lastIndex = i - 1;
                if (lastIndex < 0) lastIndex = 0;

                float lastPerImageRatio = ratio - (perImage * (lastIndex));
                images[indexOfImage].fillAmount = lastPerImageRatio * images.Count;
                nowFillComplete = true;
            }
        }
    }
}
