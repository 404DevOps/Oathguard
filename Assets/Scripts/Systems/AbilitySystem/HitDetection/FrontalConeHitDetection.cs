using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FrontalConeHitDetection : HitDetectionBase
{
    public float radius = 5f;
    [Range(0f, 180f)] public float angle = 45f;

    public override List<EntityBase> CheckHit(EntityBase origin, Vector3 offset, LayerMask layerMask)
    {
        var originPos = GetOriginPositionWithOffset(origin, offset);
        var results = new List<EntityBase>();

        Collider[] hits = Physics.OverlapSphere(originPos, radius, layerMask);
        Vector3 forward = origin.transform.forward; // Character facing cursor

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EntityBase>(out var entity) && entity != origin)
            {
                Vector3 dirToTarget = (entity.transform.position - originPos).normalized;
                float angleToTarget = Vector3.Angle(forward, dirToTarget);

                if (angleToTarget <= angle)
                {
                    results.Add(entity);
                }
            }
        }

        if (Application.isEditor)
        {
            HitDetectionGizmoDrawer.Instance.Draw(this, originPos, origin);
        }

        return results;
    }

    public override void DrawGizmo(Vector3 originPos, EntityBase origin)
    {
        Vector3 forward = origin.transform.forward;

        Quaternion leftRot = Quaternion.AngleAxis(-angle, Vector3.up);
        Quaternion rightRot = Quaternion.AngleAxis(angle, Vector3.up);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(originPos, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(originPos, originPos + leftDir * radius);
        Gizmos.DrawLine(originPos, originPos + rightDir * radius);
    }
}
