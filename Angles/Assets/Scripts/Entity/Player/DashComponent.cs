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
    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void PlayDash(Vector2 dir, float thrust, float duration)
    {
        StopEntity();
        if (_nowDash == true) return;

        _nowFinish = false;
        DashTask(dir, thrust, duration).Forget();
    }

    private async UniTaskVoid DashTask(Vector2 dir, float thrust, float duration)
    {
        _nowDash = true;
        _rigid.freezeRotation = true;

        _rigid.AddForce(dir * thrust, ForceMode2D.Impulse);
        await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _source.Token);

        _rigid.freezeRotation = false;
        _nowDash = false;

        _nowFinish = true;
        print("DashEnd");
    }

    public void QuickEndTask()
    {
        _source.Cancel();
        _source.Dispose();
        _source = new(); // 취소하면 다시 넣어주기
        CancelDash();
    }

    public void CancelDash()
    {
        _rigid.freezeRotation = false;
        _nowDash = false;
        print("DashEndPrint");
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