using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererEffectPlayer : BasicEffectPlayer
{
    [SerializeField]
    protected List<Vector3> m_pos;

    LineRenderer m_lineRenderer;

    float storedTime;

    [SerializeField]
    float maxLineWidthRatio = 0.5f;

    bool overHalf = false;
    bool nowFinished = false;

    float LineWidth 
    { 
        set 
        {
            m_lineRenderer.startWidth = value;
            m_lineRenderer.endWidth = value;
        } 
    }

    float HalfDuration
    {
        get { return m_duration * 0.5f; }
    }

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Init(Transform tr, float duration, List<Vector3> pos)
    {
        m_posTr = tr;
        m_pos = pos;
        transform.position = m_posTr.position;
        m_duration = duration;
    }

    protected override void Update()
    {
        base.Update();

        if (nowFinished == true) return;

        if(overHalf == false)
        {
            storedTime += Time.deltaTime;
            LineWidth = (storedTime / HalfDuration) * maxLineWidthRatio;

            if (storedTime >= HalfDuration)
            {
                storedTime = HalfDuration;
                overHalf = true;
            }
        }
        else
        {
            storedTime -= Time.deltaTime;
            LineWidth = (storedTime / HalfDuration) * maxLineWidthRatio;

            if (storedTime <= 0)
            {
                storedTime = 0;
                nowFinished = true;
            }
        }
    }

    public override void PlayEffect()
    {
        ResetLine();
        InitLine();

        Invoke("DisableObject", m_duration + 0.5f);
    }

    protected override void OnDisable()
    {
        ResetLine();
        base.OnDisable();
    }

    public override void RotationEffect(float rotation)
    {
    }

    protected override void DisableObject()
    {
        base.DisableObject();
    }

    void InitLine()
    {
        m_lineRenderer.positionCount = m_pos.Count + 1;

        m_lineRenderer.SetPosition(0, Vector3.zero);
        for (int i = 0; i < m_pos.Count; i++)
        {
            m_lineRenderer.SetPosition(i + 1, m_pos[i]);
        }

        m_lineRenderer.startWidth = 0;
        m_lineRenderer.endWidth = 0;
        nowFinished = false;
        overHalf = false;
    }

    void ResetLine()
    {
        storedTime = 0;
        LineWidth = 0;
        m_lineRenderer.positionCount = 0;
    }

    public override void StopEffect()
    {
        ResetLine();
    }
}
