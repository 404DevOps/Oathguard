using System;
using System.Collections.Generic;
using UnityEngine;

public class HitDetectionGizmoDrawer : Singleton<HitDetectionGizmoDrawer>
{
    private struct GizmoEntry
    {
        public IHitDetectionGizmo gizmo;
        public float time;
        public Transform origin;
        public float duration;
    }

    private List<GizmoEntry> _gizmoEntries = new();

    private void Start()
    {
        _gizmoEntries = new();
    }
    private void Update()
    {
        _gizmoEntries.RemoveAll(entry => Time.time - entry.time > entry.duration); // Show for swing duration
    }

    public void DrawGizmo(IHitDetectionGizmo gizmo, Transform origin, float duration)
    {
        _gizmoEntries.Add(new GizmoEntry { gizmo = gizmo, time = Time.time, origin = origin, duration = duration }); ;
    }

    public void OnDrawGizmos()
    {
        foreach (var entry in _gizmoEntries)
        {
            entry.gizmo.DrawGizmos(entry.origin);
        }
    }
}

