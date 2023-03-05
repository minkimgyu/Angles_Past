using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallEffect : BasicEffect
{
    RotationBallSkill storedSkill;

    public void Init(RotationBallSkill storedSkill)
    {
        this.storedSkill = storedSkill;
    }

    public void ResetPosition(Vector3 rotatedOffset)
    {
        transform.localPosition = rotatedOffset;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (storedSkill == null) return;

        if (storedSkill.HitEnemy(col.gameObject) == true)
        {
            storedSkill.RemoveBallInList(this);
            ObjectPooler.ReturnToTransform(transform);
            DisableObject();
        }
    }

    private void OnDisable()
    {
        storedSkill = null;
        ObjectPooler.ReturnToPool(gameObject);
    }
}
