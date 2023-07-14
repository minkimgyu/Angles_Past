using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBallSkill : MonoBehaviour
{
    //int speed = 70;
    //int ballCount = 3;

    //Transform playerTr = null;

    //[SerializeField]
    //List<BallEffect> storedBallEffects;

    //public int BallCount { get { return ballCount; } set { if (value > 0) ballCount = value; } }

    //public override void PlaySkill(SkillSupportData data, BasicBattleComponent battleComponent)
    //{
    //    base.PlaySkill(data, battleComponent);
    //    playerTr = data.player.transform;
    //    InitBall();
    //}

    //public override void PlayAddition()
    //{
    //    base.PlayAddition();
    //    InitBall();
    //}

    //protected override void OnDisable()
    //{
    //    base.OnDisable();
    //}

    //public void RemoveBallInList(BallEffect ballEffect)
    //{
    //    storedBallEffects.Remove(ballEffect);
    //    if(storedBallEffects.Count <= 0)
    //    {
    //        DisableObject();
    //    }
    //}

    //void InitBall()
    //{
    //    for (int i = 0; i < BallCount; i++)
    //    {
    //        GameObject go = ObjectPooler.SpawnFromPool("Orb", transform.position, Quaternion.identity, transform);
    //        BallEffect basicEffect = go.GetComponent<BallEffect>();

    //        basicEffect.Init(this);
    //        storedBallEffects.Add(basicEffect);
    //    }

    //    print(storedBallEffects.Count);

    //    for (int j = 0; j < storedBallEffects.Count; j++)
    //    {
    //        storedBallEffects[j].transform.position = Vector3.zero;

    //        float angle = (360.0f * j) / storedBallEffects.Count;

    //        print(angle);

    //        Vector3 offset = Vector3.up * SkillData.OffsetRange.magnitude;
    //        Quaternion rotation = Quaternion.Euler(0, 0, angle);
    //        Vector3 rotatedOffset = rotation * offset;

    //        storedBallEffects[j].ResetPosition(rotatedOffset);
    //        storedBallEffects[j].PlayEffect();
    //    }
    //}


    //public bool HitEnemy(GameObject go)
    //{
    //    if (SkillData.CanHitSkill(go.tag) == true)
    //    {
    //        DamageToEntity(go, SkillData.KnockBackThrust);
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (playerTr != null) transform.position = playerTr.position;
    //    transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * speed);
    //}
}
