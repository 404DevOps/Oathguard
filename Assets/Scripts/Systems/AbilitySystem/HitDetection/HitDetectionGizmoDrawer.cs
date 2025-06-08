using System.Collections.Generic;
using UnityEngine;

public class HitDetectionGizmoDrawer : MonoBehaviour
{
    private static HitDetectionGizmoDrawer instance;

    private List<HitDetectionEntry> hitDetectionEntries = new List<HitDetectionEntry>();
    public float gizmoLifetime = 0.5f;

    public static HitDetectionGizmoDrawer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("HitDetectionGizmoDrawer");
                instance = go.AddComponent<HitDetectionGizmoDrawer>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes if needed
        }
    }

    public void Draw(HitDetectionBase hitDetection, Vector3 position, EntityBase origin)
    {
        hitDetectionEntries.Add(new HitDetectionEntry(hitDetection, position, Time.time, origin));
    }

    private void OnDrawGizmos()
    {
        // Filter out expired gizmos
        hitDetectionEntries.RemoveAll(entry => Time.time - entry.creationTime > gizmoLifetime);

        foreach (var entry in hitDetectionEntries)
        {
            entry.hitDetection.DrawGizmo(entry.position, entry.origin);
        }
    }

    private class HitDetectionEntry
    {
        public HitDetectionBase hitDetection;
        public Vector3 position;
        public float creationTime;
        public EntityBase origin;

        public HitDetectionEntry(HitDetectionBase hitDetection, Vector3 position, float creationTime, EntityBase origin)
        {
            this.hitDetection = hitDetection;
            this.position = position;
            this.creationTime = creationTime;
            this.origin = origin;
        }
    }
}