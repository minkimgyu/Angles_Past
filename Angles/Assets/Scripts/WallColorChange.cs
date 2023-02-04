using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;

public class WallColorChange : MonoBehaviour
{
    Tilemap tilemap;
    CancellationTokenSource source = new();
    public Color startColor = new Color(1, 1, 1);
    public Color endColor = new Color(102f / 255f, 102f / 255f, 102f / 255f);

    float duration = 0.5f;
    float smoothness = 0.05f;

    List<Vector3Int> ColorChangeTileList = new List<Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        //tilemap.color = endColor;
    }

    public void ChangeTileColor(Vector3 hitPosition)
    {
        ColorChange(hitPosition).Forget();
    }

    private async UniTaskVoid ColorChange(Vector3 hitPosition)
    {
        float progress = 0;

        float increment = smoothness / duration;

        Vector3Int tilePos = tilemap.WorldToCell(hitPosition);

        if (ColorChangeTileList.Contains(tilePos) == true)
        {
            return;
        }

        ColorChangeTileList.Add(tilePos);

        tilemap.SetTileFlags(tilePos, TileFlags.None);
        while (progress < 1)
        {
            Color currentColor = Color.Lerp(startColor, endColor, progress);
            tilemap.SetColor(tilemap.WorldToCell(hitPosition), currentColor);

            progress += increment;
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: source.Token);
        }

        progress = 0;
        tilemap.SetColor(tilemap.WorldToCell(hitPosition), endColor);

        while (progress < 1)
        {
            Color currentColor = Color.Lerp(endColor, startColor, progress);
            tilemap.SetColor(tilemap.WorldToCell(hitPosition), currentColor);

            progress += increment;
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: source.Token);
        }

        tilemap.SetColor(tilemap.WorldToCell(hitPosition), startColor);
        ColorChangeTileList.Remove(tilePos);
    }

    private void OnDestroy()
    {
        source.Cancel();
        source.Dispose();
    }

    private void OnDisable()
    {
        source.Cancel();
    }

    private void OnEnable()
    {
        if (source != null) source.Dispose();
        source = new CancellationTokenSource();
    }
}
