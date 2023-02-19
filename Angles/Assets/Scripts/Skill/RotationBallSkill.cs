using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBallSkill : BasicSkill
{
    List<BasicEffect> basicEffects = new List<BasicEffect>();
    float distanceFromPlayer = 2f;
    int speed = 70;

    int ballCount = 3;

    Transform playerTr = null;

    public int BallCount { get { return ballCount; } set { if (value > 0) ballCount = value; } }

    public override void PlaySkill(SkillSupportData skillSupportData)
    {
        playerTr = skillSupportData.player.transform;

        Debug.Log("PlaySkill");
        InitBall();
        base.PlaySkill(skillSupportData);
    }

    void InitBall()
    {
        for (int i = 0; i < BallCount; i++)
        {
            float angle = (360.0f * i) / ballCount;

            Vector3 offset = Vector3.up * distanceFromPlayer;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 rotatedOffset = rotation * offset;

            GameObject go = ObjectPooler.SpawnFromPool("Orb", transform.position, Quaternion.identity, transform);
            BallEffect basicEffect = go.GetComponent<BallEffect>();
            basicEffect.PlayEffect();
            basicEffect.transform.localPosition = rotatedOffset;

            basicEffects.Add(basicEffect);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTr != null) transform.position = playerTr.position;

        transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * speed);
    }
}
