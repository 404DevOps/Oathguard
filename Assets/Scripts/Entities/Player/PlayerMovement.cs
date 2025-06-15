using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Rotation
    [Header("Rotation")]
    public LayerMask GroundLayer;
    public Transform ModelContainer;
    public float rotationSmoothing;
    private Plane _groundPlane;

    public float alignmentThreshold = 0.5f;
    #endregion

    #region References
    private PlayerEntity _playerEntity;
    private CharacterController _characterController;
    private EntityStats _playerStats;
    private AbilityExecutor _abilityExecutor;
    private Rigidbody _rb;
    #endregion

    internal void Initialize()
    {
        _playerEntity = GetComponent<PlayerEntity>();
        ModelContainer = transform.Find("Model");
        _characterController = GetComponent<CharacterController>();
        _playerStats = _playerEntity.Stats;
        _abilityExecutor = _playerEntity.AbilityExecutor;
        _rb = GetComponent<Rigidbody>();
        _groundPlane = new Plane(Vector3.up, transform.position);
    }
    private void Update()
    {
        HandlePossibleActions();

        if (_playerEntity.IsDead) return;

        HandleMove();

        if (!_abilityExecutor.IsAttacking && _playerEntity.CanRotate)
            HandleRotation();

        SetAnimationInfo();
    }

    private void HandlePossibleActions()
    {
        _playerEntity.CanMove = !_playerEntity.Hurt.IsHurt;
        _playerEntity.CanRotate = !_playerEntity.Hurt.IsHurt;
        _playerEntity.CanUseAbilities = !_playerEntity.Hurt.IsHurt;
    }

    private void SetAnimationInfo()
    {
        float moveSpeed = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z).magnitude;
        _playerEntity.Animator.SetFloat("moveSpeed", moveSpeed);

        Vector3 moveDirection = UserInput.Instance.MovementInput.normalized; //WASD input
        Vector3 facingDirection = ModelContainer.transform.forward;

        // Check dot product: <-0.5 = moving backward, between -0.5 and 0.5 = strafing, >0.5 = forward
        Vector3 translatedInput = new Vector3(moveDirection.x, 0, moveDirection.y);
        float alignment = Vector3.Dot(facingDirection, translatedInput);
        float sideAlignment = Vector3.Dot(ModelContainer.transform.right, moveDirection);

        //todo adjust threshold to have smooth front/back/strafe movement
        bool isStrafing = alignment > -alignmentThreshold && alignment < alignmentThreshold;
        bool isStrafingLeft = isStrafing && sideAlignment < -0.1f;
        bool isStrafingRight = isStrafing && sideAlignment > 0.1f;

        _playerEntity.Animator.SetBool("isMovingBackward", alignment < -alignmentThreshold);
        _playerEntity.Animator.SetBool("isStrafingLeft", isStrafingLeft);
        _playerEntity.Animator.SetBool("isStrafingRight", isStrafingRight);
        _playerEntity.Animator.SetBool("isMovingForward", alignment > alignmentThreshold);
    }

    #region Movement

    public void StopMove()
    {
        _characterController.Move(new Vector3(0, 0));
        _rb.linearVelocity = Vector3.zero;
    }

    public void HandleMove()
    {
        if (_playerStats == null)
            return;
        Vector3 moveVector = Vector3.zero;

        if(_playerEntity.CanMove)
            moveVector = new Vector3(UserInput.Instance.MovementInput.x * _playerStats.MoveSpeed, 0, UserInput.Instance.MovementInput.y * _playerStats.MoveSpeed);
        
        _characterController.Move(moveVector);
    }
    private void HandleRotation()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Utility.Camera.ScreenPointToRay(mousePos);

        if (_groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                ModelContainer.transform.rotation = Quaternion.Slerp(ModelContainer.transform.rotation, targetRotation, rotationSmoothing * Time.deltaTime);
            }
        }
    }

    #endregion

    #region Lock Character in Place
    private Coroutine lockCoroutine = null;
    private float _currentLockDuration = 0f;
    internal void LockCharacter(float duration)
    {
        if (duration > _currentLockDuration)
        {
            _currentLockDuration = duration;
        }

        if (lockCoroutine != null)
        {
            StopCoroutine(lockCoroutine);
        }

        _characterController.FreezePosition(true);
        lockCoroutine = StartCoroutine(UnlockCharacter());

    }
    internal IEnumerator UnlockCharacter()
    {
        while (_currentLockDuration > 0)
        {
            yield return null;
            _currentLockDuration -= Time.deltaTime;
        }

        // When the time is up, unlock the character
        _characterController.FreezePosition(false);
        lockCoroutine = null;
    }
    internal void ForceUnlockCharacter()
    {
        if (lockCoroutine != null)
            StopCoroutine(lockCoroutine);

        _currentLockDuration = 0f;
        _characterController.FreezePosition(false);
        lockCoroutine = null;
    }

    #endregion
}