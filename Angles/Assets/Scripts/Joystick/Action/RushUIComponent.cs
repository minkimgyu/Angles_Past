using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class RushUIComponent : FillUIComponent, IObserver<Player.ObserverType, PlayerData>
{
    Player player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.AddObserver(this);
        InitImages();
    }

    private void OnDestroy()
    {
        player.RemoveObserver(this);
    }

    public void OnNotify(Player.ObserverType state, PlayerData data)
    {
        if (state == Player.ObserverType.ShowRushUI)
            FillDashIcon(data.RushRatio);
        else if (state == Player.ObserverType.HideRushUI)
            FillDashIcon(0);
    }
}
