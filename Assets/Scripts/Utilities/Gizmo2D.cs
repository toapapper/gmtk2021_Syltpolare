using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo2D : MonoBehaviour
{
    [SerializeField] private string _gizmo;
    [SerializeField] private bool _allowScaling = true;
    [SerializeField] private Placement _placement = Placement.Transform;

    private Collider2D _collider;

    private enum Placement
    {
        Transform,
        ColliderBoundCenter,
    }

    private void OnDrawGizmos()
    {
        Vector3 position = Vector3.zero;

        switch (_placement)
        {
            case Placement.Transform:
                position = transform.position;
                break;
            case Placement.ColliderBoundCenter:
                if (_collider == null)
                    TryGetComponent<Collider2D>(out _collider);

                position = _collider.bounds.center;
                break;
            default:
                break;
        }

        Gizmos.DrawIcon(position, _gizmo, _allowScaling);
    }
}
