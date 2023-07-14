using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class AttackComponent : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigid;
    bool _nowAttack = false;

    public bool NowAttack
    {
        get { return _nowAttack; }
    }

    CancellationTokenSource _source = new();

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    //void SubUpdate()
    //{
    //    if (PlayManager.Instance.moveJoy.moveInputComponent.NowCancelAttack() == true) // 현재 공격을 취소해야할 경우
    //    {
    //        QuickEndTask();
    //    }
    //}

    public void AttackReady()
    {
        
    }

    public void Attack(float rushTime)
    {
        WaitAttackEndTask(rushTime).Forget();
    }

    public async UniTaskVoid WaitAttackEndTask(float rushTime)
    {
        _nowAttack = true;
        

        //skillComponent 추가 --> 스킬 사용가능하게끔 제작

        await UniTask.Delay(TimeSpan.FromSeconds(rushTime), cancellationToken: _source.Token);
        CancelAttack();

        _nowAttack = false;
    }

    public void QuickEndTask()
    {
        _source.Cancel();
        CancelAttack();
    }

    public void CancelAttack()
    {
        rigid.freezeRotation = false;
        _nowAttack = false;
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
