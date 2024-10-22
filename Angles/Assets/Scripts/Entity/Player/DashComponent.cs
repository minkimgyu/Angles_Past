using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class DashComponent : MonoBehaviour
{
    Rigidbody2D _rigid;
    CancellationTokenSource _source = new();
    bool _nowDash = false;
    bool _nowFinish = false;
    public bool NowFinish
    {
        get { return _nowFinish; }
    }

    public bool NowDash
    {
        get { return _nowDash; }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void PlayDash(Vector2 dir, float thrust, float duration, bool nowFreeze = true)
    {
        StopEntity();
        if (_nowDash == true) return;

        _nowFinish = false;
        DashTask(dir, thrust, duration, nowFreeze).Forget();
    }

    public void PlayDash(Vector2 dir, float thrust, bool nowFreeze = true)
    {
        StopEntity();
        if (_nowDash == true) return;

        _nowFinish = false;
        DashTask(dir, thrust, nowFreeze).Forget();
    }

    private async UniTaskVoid DashTask(Vector2 dir, float thrust, float duration, bool nowFreeze = true)
    {
        _nowDash = true;

        if(nowFreeze)
            _rigid.freezeRotation = true;

        _rigid.AddForce(dir * thrust, ForceMode2D.Impulse);
        await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _source.Token);

        if (nowFreeze)
            _rigid.freezeRotation = false;

        _nowDash = false;

        _nowFinish = true;
    }

    private async UniTaskVoid DashTask(Vector2 dir, float thrust, bool nowFreeze = true)
    {
        _nowDash = true;
        if (nowFreeze)
            _rigid.freezeRotation = true;

        _rigid.AddForce(dir * thrust, ForceMode2D.Impulse);
        await UniTask.Delay(TimeSpan.FromSeconds(0), cancellationToken: _source.Token);

        if (nowFreeze)
            _rigid.freezeRotation = false;
        _nowDash = false;

        _nowFinish = true;
    }

    public void QuickEndTask()
    {
        if(_source != null)
        {
            _source.Cancel();
            _source.Dispose();
            _source = new(); // 취소하면 다시 넣어주기
            CancelDash();
        }
    }

    public void CancelDash(bool nowFreeze = true)
    {
        if(nowFreeze)
            _rigid.freezeRotation = false;
        _nowDash = false;
        _nowFinish = false;
    }

    private void StopEntity()
    {
        _rigid.velocity = Vector2.zero;
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