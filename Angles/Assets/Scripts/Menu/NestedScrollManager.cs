using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NestedScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Transform contentTr;

    public Slider tabSlider;
    public RectTransform[] BtnRect, BtnImageRect;

    const int SIZE = 4;
    float[] pos = new float[SIZE];
    float distance, curPos, targetPos;
    bool isDrag;
    int targetIndex = 0;


    void Start()
    {
        // �Ÿ��� ���� 0~1�� pos����
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;
    }

    float SetPos()
    {
        // ���ݰŸ��� �������� ����� ��ġ�� ��ȯ
        for (int i = 0; i < SIZE; i++)
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        return 0;
    }


    public void OnBeginDrag(PointerEventData eventData) => curPos = SetPos();

    public void OnDrag(PointerEventData eventData) => isDrag = true;

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();

        // ���ݰŸ��� ���� �ʾƵ� ���콺�� ������ �̵��ϸ�
        if (curPos == targetPos)
        {
            // �� ���� ������ ��ǥ�� �ϳ� ����
            if (eventData.delta.x > 18 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
            }

            // �� ���� ������ ��ǥ�� �ϳ� ����
            else if (eventData.delta.x < -18 && curPos + distance <= 1.01f)
            {
                ++targetIndex;
                targetPos = curPos + distance;
            }
        }

        VerticalScrollUp();
    }

    void VerticalScrollUp()
    {
        // ��ǥ�� ������ũ���̰�, ������ �ŰܿԴٸ� ������ũ���� �� ���� �ø�
        for (int i = 0; i < SIZE; i++)
            if (contentTr.GetChild(i).GetComponent<ScrollScript>() && curPos != pos[i] && targetPos == pos[i])
                contentTr.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;
    }


    void Update()
    {
        tabSlider.value = scrollbar.value;

        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);

            // ��ǥ ��ư�� ũ�Ⱑ Ŀ��
            for (int i = 0; i < SIZE; i++) BtnRect[i].sizeDelta = new Vector2(i == targetIndex ? 330 : 250, i == targetIndex ? 270 : 230);
        }

        //BtnRect[i].sizeDelta.y

        if (Time.time < 0.1f) return;

        for (int i = 0; i < SIZE; i++)
        {
            // ��ư �������� �ε巴�� ��ư�� �߾����� �̵�, ũ��� 1, �ؽ�Ʈ ��Ȱ��ȭ
            Vector3 BtnTargetPos = BtnRect[i].anchoredPosition3D;
            Vector3 BtnTargetScale = Vector3.one;
            bool textActive = false;

            // ������ ��ư �������� �ణ ���� �ø���, ũ�⵵ Ű���, �ؽ�Ʈ�� Ȱ��ȭ
            if (i == targetIndex)
            {
                BtnTargetPos.y = 20f;
                BtnTargetScale = new Vector3(1.2f, 1.2f, 1);
                textActive = true;
            }
            else
            {
                BtnTargetPos.y = -100f;
            }

            BtnImageRect[i].anchoredPosition3D = Vector3.Lerp(BtnImageRect[i].anchoredPosition3D, BtnTargetPos, 0.25f);
            BtnImageRect[i].localScale = Vector3.Lerp(BtnImageRect[i].localScale, BtnTargetScale, 0.25f);
            BtnImageRect[i].transform.GetChild(0).gameObject.SetActive(textActive);
        }
    }


    public void TabClick(int n)
    {
        curPos = SetPos();
        targetIndex = n;
        targetPos = pos[n];
        VerticalScrollUp();
    }

}