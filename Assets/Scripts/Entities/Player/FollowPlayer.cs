using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowPlayer : MonoBehaviour
{
    Transform playerTransform;
    public Vector3 offset;
    public Vector3 Angle;
    public float smoothTime = 0.2f;
    public bool _isInitialized = false;

    private Vector3 velocity = Vector3.zero;
    private void Start()
    {
        GameEvents.OnEntityInitialized?.AddListener(OnEntityInitialized);
    }
    void OnEntityInitialized(EntityBase entity)
    {
        if (entity is not PlayerEntity) return;

        if (EntityManager.Instance.Player != null)
            playerTransform = EntityManager.Instance.Player.transform;
        else
            playerTransform = FindFirstObjectByType<PlayerEntity>().transform;

        _isInitialized = true;
    }

    void LateUpdate()
    {
        if (!_isInitialized) return;

        Vector3 targetPosition = playerTransform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.rotation = Quaternion.Euler(Angle);
    }
}
