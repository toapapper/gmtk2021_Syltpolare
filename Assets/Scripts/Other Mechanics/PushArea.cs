using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushArea : MonoBehaviour
{
    [SerializeField] private Vector2 _force = new Vector2(50000, 0);
    [SerializeField] private LayerMask _layerMask;

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (_layerMask.Contains(obj.layer))
        {
            Rigidbody2D body = obj.GetComponent<Rigidbody2D>();

            if (body != null)
            {
                Vector3 point = transform.InverseTransformPoint(obj.transform.position);
                body.AddForce(new Vector2(_force.x * ((point.x > 0.0f) ? 1 : -1), _force.y));
            }
        }
    }
}
