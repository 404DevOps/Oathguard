using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class HitDetectionBase
{
    public virtual List<EntityBase> CheckHit(EntityBase origin, Vector3 offset, LayerMask layerMask)
    {
        var pos = GetOriginPositionWithOffset(origin, offset);
        if (Application.isEditor)
        {
            HitDetectionGizmoDrawer.Instance.Draw(this, pos, origin);
        }

        //will be overriden
        return null;
    }

    public Vector3 GetOriginPositionWithOffset(EntityBase origin, Vector3 offset)
    {
        //todo use rotation
        //if (!origin.FacingRight)
        //    offset = new Vector3(-offset.x, offset.y, offset.z);

        return origin.transform.position + offset;
    }

    public abstract void DrawGizmo(Vector3 originPos, EntityBase origin);

}