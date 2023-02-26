using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;

public class WallColorChange : UnitaskUtility
{
    public Tilemap changeTilemap;
    Tilemap tilemap;

    public Color startColor = new Color(1, 1, 1);
    public Color endColor = new Color(102f / 255f, 102f / 255f, 102f / 255f);
    public Color basicColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);

    float duration = 0.5f;
    float smoothness = 0.01f;

    List<Vector3Int> ColorChangeTileList = new List<Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        SetHaloInvisible();
    }


    void SetHaloInvisible()
    {
        BoundsInt bounds = changeTilemap.cellBounds;

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tileBase = changeTilemap.GetTile(pos);

                if(tileBase == null) continue;
                if(tileBase.name == "WallTileHalo")
                {
                    changeTilemap.SetColor(pos, basicColor);
                }
            }
        }
    }

    public void ChangeTileColor(Vector3 hitPosition)
    {
        Vector3Int tilePos = tilemap.WorldToCell(hitPosition);
        CheckAround(tilePos);
    }

    void CheckAround(Vector3Int pos)
    {
        Vector3Int right = new Vector3Int(pos.x + 1, pos.y, 0);
        Vector3Int left = new Vector3Int(pos.x - 1, pos.y, 0);
        Vector3Int up = new Vector3Int(pos.x, pos.y + 1, 0);
        Vector3Int down = new Vector3Int(pos.x, pos.y - 1, 0);

        ColorChange(pos).Forget();

        if (tilemap.GetSprite(right) != null && tilemap.GetSprite(right).name.Contains("Corner"))
        {
            if (ColorChangeTileList.Contains(right) == false) ColorChange(right).Forget();
        }
        if (tilemap.GetSprite(left) != null && tilemap.GetSprite(left).name.Contains("Corner"))
        {
            if (ColorChangeTileList.Contains(right) == false) ColorChange(left).Forget();
        }
        if (tilemap.GetSprite(up) != null && tilemap.GetSprite(up).name.Contains("Corner"))
        {
            if (ColorChangeTileList.Contains(right) == false) ColorChange(up).Forget();
        }
        if (tilemap.GetSprite(down) != null && tilemap.GetSprite(down).name.Contains("Corner"))
        {
            if (ColorChangeTileList.Contains(right) == false) ColorChange(down).Forget();
        }
    }

    private async UniTaskVoid ColorChange(Vector3Int pos)
    {
        nowRunning = true;
        float progress = 0;
        float increment = smoothness / duration;

        ColorChangeTileList.Add(pos);

        changeTilemap.SetTileFlags(pos, TileFlags.None);
        while (progress < 1)
        {
            Color currentColor = Color.Lerp(startColor, endColor, progress);
            changeTilemap.SetColor(pos, currentColor);

            progress += increment;
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: source.Token);
        }

        progress = 0;
        changeTilemap.SetColor(pos, endColor);

        while (progress < 1)
        {
            Color currentColor = Color.Lerp(endColor, startColor, progress);
            changeTilemap.SetColor(pos, currentColor);

            progress += increment;
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: source.Token);
        }

        changeTilemap.SetColor(pos, startColor);

        ColorChangeTileList.Remove(pos);
        nowRunning = false;
    }
}
