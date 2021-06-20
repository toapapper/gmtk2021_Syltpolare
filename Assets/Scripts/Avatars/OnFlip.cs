using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFlip : MonoBehaviour
{
    private bool _isFlipped;

    public void OnFlipChange(InputContext context)
    {
        Vector3 scale = transform.localScale;
        if (context.Value > 0 && _isFlipped)
        {
            _isFlipped = false;
            transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        else if (context.Value < 0 && !_isFlipped)
        {
            _isFlipped = true;
            transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
        }
    }
}
