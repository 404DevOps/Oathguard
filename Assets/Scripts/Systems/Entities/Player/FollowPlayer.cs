using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform playerTransform;
    public Vector3 offset;
    public Vector3 Angle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (EntityManager.Instance.Player != null)
            playerTransform = EntityManager.Instance.Player.transform;
        else
            playerTransform = FindFirstObjectByType<PlayerEntity>().transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerTransform.position + offset;
        transform.rotation = Quaternion.Euler(Angle);// new Quaternion(Angle.x, Angle.y, Angle.z, 0);
    }
}
