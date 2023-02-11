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
    public Tilemap changeTilemap;
    Tilemap tilemap;
    CancellationTokenSource source = new();
    public Color startColor = new Color(1, 1, 1);
    public Color endColor = new Color(102f / 255f, 102f / 255f, 102f / 255f);

    public Color basicColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);

    float duration = 0.5f;
    float smoothness = 0.05f;

    List<Vector3Int> ColorChangeTileList = new List<Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        //tilemap.color = endColor;
        check();
    }


    void check()
    {
        //changeTilemap.CompressBounds();

        BoundsInt bounds = changeTilemap.cellBounds;
        //TileBase[] allTiles = changeTilemap.GetTilesBlock(bounds);

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tileBase = changeTilemap.GetTile(pos);
                if(tileBase == null)
                {
                    continue;
                }

                if(tileBase.name == "WallTileHalo")
                {
                    Debug.Log(pos);
                    changeTilemap.SetColor(pos, basicColor);
                }
            }
        }
    }

    public void ChangeTileColor(Vector3 hitPosition)
    {
        ColorChange(hitPosition).Forget();
    }

    void CheckAround(Vector3Int pos, Color color)
    {
        Vector3Int right = new Vector3Int(pos.x + 1, pos.y, 0);
        Vector3Int left = new Vector3Int(pos.x - 1, pos.y, 0);
        Vector3Int up = new Vector3Int(pos.x, pos.y + 1, 0);
        Vector3Int down = new Vector3Int(pos.x, pos.y - 1, 0);

        if (tilemap.GetSprite(right) != null)
        {
            if (tilemap.GetSprite(right).name.Contains("Corner")) changeTilemap.SetColor(right, color);
        }

        if (tilemap.GetSprite(left) != null)
        {
            if (tilemap.GetSprite(left).name.Contains("Corner")) changeTilemap.SetColor(left, color);
        }

        if (tilemap.GetSprite(up) != null)
        {
            if (tilemap.GetSprite(up).name.Contains("Corner")) changeTilemap.SetColor(up, color);
        }

        if (tilemap.GetSprite(down) != null)
        {
            if(tilemap.GetSprite(down).name.Contains("Corner")) changeTilemap.SetColor(down, color);
        }
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

        changeTilemap.SetTileFlags(tilePos, TileFlags.None);
        while (progress < 1)
        {
            Color currentColor = Color.Lerp(startColor, endColor, progress);
            changeTilemap.SetColor(tilePos, currentColor);
            CheckAround(tilePos, currentColor);

            progress += increment;
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: source.Token);
        }

        progress = 0;
        changeTilemap.SetColor(tilePos, endColor);
        CheckAround(tilePos, endColor);

        while (progress < 1)
        {
            Color currentColor = Color.Lerp(endColor, startColor, progress);
            changeTilemap.SetColor(tilemap.WorldToCell(hitPosition), currentColor);
            CheckAround(tilePos, currentColor);

            progress += increment;
            await UniTask.Delay(TimeSpan.FromSeconds(smoothness), cancellationToken: source.Token);
        }

        changeTilemap.SetColor(tilemap.WorldToCell(hitPosition), startColor);
        CheckAround(tilePos, startColor);

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
