using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask ignoreLayer;
    public LayerMask targetLayer;

    public List<GameObject> visableTargets = new List<GameObject>();

    private void Update()
    {
        FindVisableTargets();
    }
    void FindVisableTargets()
    {
        visableTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetLayer);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            GameObject target = targetsInViewRadius[i].gameObject;
            Transform targetTansform = target.GetComponent<Transform>();
            Vector2 directionToTarget = (targetTansform.position - transform.position).normalized;
            if (Vector2.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, targetTansform.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ignoreLayer))
                {
                    visableTargets.Add(target);
                }
            }
        }
    }
}
