using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBallSkill : BasicSkill
{
    List<BasicEffect> basicEffects = new List<BasicEffect>();
    float distanceFromPlayer = 2f;
    int speed = 70;

    int ballCount = 3;
    public int BallCount 
    {
        get
        {
            return ballCount;
        }
        set 
        {
            if(value > 0) ballCount = value;
        } 
    }


    public override void PlaySkill(Vector2 dir, List<Collision2D> entity)
    {
        Debug.Log("PlaySkill");
        InitBall();
        base.PlaySkill(dir, entity);
    }

    protected override void DisableObject()
    {
        for (int i = 0; i < basicEffects.Count; i++)
        {
            ObjectPooler.ReturnToPool(basicEffects[i].gameObject, true);
            basicEffects[i].gameObject.SetActive(false); // 꺼주면 안에 있는 함수로 알아서 원래 부모로 돌아감
        }
        gameObject.SetActive(false);
    }

    void InitBall()
    {
        for (int i = 0; i < ballCount; i++)
        {
            float angle = (360.0f * i) / ballCount;

            Vector3 offset = Vector3.up * distanceFromPlayer;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 rotatedOffset = rotation * offset;

            GameObject effectGo = GetEffectUsingName(transform.position, Quaternion.identity, transform);
            BasicEffect basicEffect = effectGo.GetComponent<BasicEffect>();
            basicEffect.transform.localPosition = rotatedOffset;

            basicEffects.Add(basicEffect);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * speed);

        if (moveTr != null) transform.position = moveTr.position;
    }
}
