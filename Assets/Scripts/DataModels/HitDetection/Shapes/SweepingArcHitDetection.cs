using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Combat/HitDetection/SweepingArc")]
public class SweepingArcHitDetection : HitDetectionBase, IHitDetectionGizmo
{
    public float Angle = 90f;
    public bool SweepLeftToRight = true;
    public bool UseRaycast = true;
    public float RaycastStepAngle = 5f;

    private float rayLength => Range;
    private float radius => Range;

    private Ray? currentRay = null;


    public override IEnumerator Execute(EntityBase origin, LayerMask enemyLayer, float hitDuration, Action<EntityBase, EntityBase> onHitAction)
    {
        Duration = hitDuration;
        HitDetectionGizmoDrawer.Instance.DrawGizmo(this, origin.Model, hitDuration);
        yield return origin.GetComponent<MonoBehaviour>().StartCoroutine(SwingCoroutine(origin, enemyLayer, onHitAction));
    }

    private IEnumerator SwingCoroutine(EntityBase origin, LayerMask enemyLayer, Action<EntityBase, EntityBase> onHitAction)
    {
        HashSet<Collider> alreadyHit = new();
        float directionMultiplier = SweepLeftToRight ? 1f : -1f;
        float halfAngle = Angle / 2f;

        int steps = Mathf.CeilToInt(Angle / RaycastStepAngle);
        float timePerStep = Duration / steps;

        for (int i = 0; i <= steps; i++)
        {
            float t = i / (float)steps;
            float currentAngle = Mathf.Lerp(-halfAngle, halfAngle, SweepLeftToRight ? t : (1 - t));
            Vector3 dir = Quaternion.AngleAxis(currentAngle, Vector3.up) * origin.Model.forward;
            Vector3 position = origin.Model.position;

            if (UseRaycast)
            {
                var ray = new Ray(position, dir);
                currentRay = ray;
                bool hasHit = Physics.Raycast(ray, out RaycastHit hit, rayLength, enemyLayer.value);

                if (hasHit && !alreadyHit.Contains(hit.collider))
                {
                    var enemyHit = hit.collider.GetComponent<EntityBase>();
                    if (enemyHit != null)
                    {
                        alreadyHit.Add(hit.collider);
                        onHitAction?.Invoke(origin, enemyHit);
                    }
                }
            }
            else
            {
                Vector3 point = position + dir * radius;
                Collider[] hits = Physics.OverlapSphere(point, Range, enemyLayer);
                foreach (var h in hits)
                {
                    if (!alreadyHit.Contains(h))
                    {
                        var enemyHit = h.GetComponent<EntityBase>();
                        if (enemyHit != null)
                        {
                            alreadyHit.Add(h);
                            onHitAction?.Invoke(origin, enemyHit);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(timePerStep);
        }
        currentRay = null;
    }

    public void DrawGizmos(Transform origin)
    {
        float halfAngle = Angle / 2f;
        Vector3 left = Quaternion.AngleAxis(-halfAngle, Vector3.up) * origin.forward;
        Vector3 right = Quaternion.AngleAxis(halfAngle, Vector3.up) * origin.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin.position, origin.position + left * radius);
        Gizmos.DrawLine(origin.position, origin.position + right * radius);
        Gizmos.DrawWireSphere(origin.position + origin.forward * radius, 0.2f);

        //draw arc sweep
        float angleStep = RaycastStepAngle;
        for (float a = -halfAngle; a <= halfAngle; a += angleStep)
        {
             Vector3 dir = Quaternion.AngleAxis(a, Vector3.up) * origin.forward;
            Vector3 pos = origin.position + dir * radius;
            Gizmos.DrawWireSphere(pos, 0.1f);
        }
        if (currentRay.HasValue)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(currentRay.Value.origin, currentRay.Value.origin + currentRay.Value.direction * rayLength);
        }
    }
}
