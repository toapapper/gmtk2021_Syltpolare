using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;

    public void OnFlip(InputContext context)
    {
        if (context.Value > 0)
            _spriteRenderer.flipX = false;
        else if (context.Value < 0)
            _spriteRenderer.flipX = true;
    }
}
