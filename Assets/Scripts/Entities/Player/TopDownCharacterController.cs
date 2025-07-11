﻿using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
    }

    public void Move(Vector3 deltaMovement)
    {
        //_rb.MovePosition(deltaMovement);
        _rb.linearVelocity = deltaMovement;
    }

    internal void FreezePosition(bool isFreeze)
    {
        _rb.constraints = isFreeze ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.FreezePositionY;
    }
}