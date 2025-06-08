using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmos()
    {
    }

    public void Move(Vector3 deltaMovement)
    {
        _rb.linearVelocity = deltaMovement;
    }

    internal void FreezePosition(bool isFreeze)
    {
        _rb.constraints = isFreeze ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.FreezeRotation;
    }
}