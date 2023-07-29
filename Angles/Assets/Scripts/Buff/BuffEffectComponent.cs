using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffectComponent : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void ReturnBaseColor()
    {
        spriteRenderer.color = new Color(0, 0, 0, 0);
    }
}
