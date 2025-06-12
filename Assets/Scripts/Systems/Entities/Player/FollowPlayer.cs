using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowPlayer : MonoBehaviour
{
    Transform playerTransform;
    public Vector3 offset;
    public Vector3 Angle;
    public float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (EntityManager.Instance.Player != null)
            playerTransform = EntityManager.Instance.Player.transform;
        else
            playerTransform = FindFirstObjectByType<PlayerEntity>().transform;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = playerTransform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.rotation = Quaternion.Euler(Angle);
    }
}
