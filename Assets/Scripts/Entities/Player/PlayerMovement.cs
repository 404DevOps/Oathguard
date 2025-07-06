using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    #region Rotation
    [Header("Rotation")]
    public float TurnSpeed = 10f;
    public float SnapThreshold = 160f; // Degrees for a near-180 turn

    public LayerMask GroundLayer;
    public Transform ModelContainer;
    public float rotationSmoothing;
    private Plane _groundPlane;

    public float alignmentThreshold = 0.5f;

    public Vector3 _moveDirection;
    #endregion

    #region References
    private PlayerEntity _playerEntity;
    private TopDownCharacterController _characterController;
    private EntityStats _playerStats;
    private AbilityExecutor _abilityExecutor;
    private Rigidbody _rb;
    #endregion

    public bool _isInitialized = false;

    internal void Initialize()
    {
        _playerEntity = GetComponent<PlayerEntity>();
        ModelContainer = transform.Find("Model");
        _characterController = GetComponent<TopDownCharacterController>();
        _playerStats = _playerEntity.Stats;
        _abilityExecutor = _playerEntity.AbilityExecutor;
        _rb = GetComponent<Rigidbody>();
        _groundPlane = new Plane(Vector3.up, transform.position);
        _isInitialized = true;

    }
    private void Update()
    {
        if (!_isInitialized) return;

        HandlePossibleActions();

        if (_playerEntity.IsDead)
        {
            _moveDirection = Vector3.zero;
            _characterController.Move(_moveDirection); //stop eventual movement
            return;
        }

        HandleMove();

        if (_playerEntity.CanRotate)
        {
            if (UserInput.Instance.CurrentControlScheme == "Keyboard")
                HandleRotation();
            else
                HandleRotationInDirection();
        }
        SetAnimationInfo();
    }


    private void HandlePossibleActions()
    {
        _playerEntity.CanMove = _playerEntity.IsDead && _abilityExecutor.IsAttacking? false : !_playerEntity.Hurt.IsHurt;
        _playerEntity.CanRotate = _playerEntity.IsDead && _abilityExecutor.IsAttacking ? false : !_playerEntity.Hurt.IsHurt;
        _playerEntity.CanUseAbilities = _playerEntity.IsDead && _abilityExecutor.IsAttacking ? false : !_playerEntity.Hurt.IsHurt;
    }

    private void SetAnimationInfo()
    {
        float moveSpeed = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z).magnitude;
        _playerEntity.Animator.SetFloat("moveSpeed", moveSpeed);

        Vector3 moveDirection = UserInput.Instance.MovementInput.normalized; //WASD input
        Vector3 facingDirection = ModelContainer.transform.forward;

        // Check dot product: <-0.5 = moving backward, between -0.5 and 0.5 = strafing, >0.5 = forward
        Vector3 translatedInput = new Vector3(moveDirection.x, 0, moveDirection.y);
        float moveY = Vector3.Dot(facingDirection, translatedInput);
        float moveX = Vector3.Dot(ModelContainer.transform.right, moveDirection);

        moveX = Mathf.Clamp(moveX, -1f, 1f);
        moveY = Mathf.Clamp(moveY, -1f, 1f);
        if (Mathf.Abs(moveX) < 0.2f) moveX = 0;
        if (Mathf.Abs(moveY) < 0.2f) moveY = 0;
        // For Blend Tree: X = right/left, Y = forward/back
        _playerEntity.Animator.SetFloat("moveX", moveX);
        _playerEntity.Animator.SetFloat("moveY", moveY);
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
        _moveDirection = Vector3.zero;

        if(_playerEntity.CanMove)
            _moveDirection = new Vector3(UserInput.Instance.MovementInput.x, 0, UserInput.Instance.MovementInput.y).normalized * _playerStats.MoveSpeed;
        
        _characterController.Move(_moveDirection);
    }
    private void HandleRotationInDirection()
    {
        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
            float angle = Quaternion.Angle(ModelContainer.rotation, targetRotation);

            if (angle > SnapThreshold && !_playerEntity.AbilityExecutor.IsAttacking)
            {
                ModelContainer.rotation = targetRotation;
            }
            else
            {
                var turnSpeedFactor = _playerEntity.AbilityExecutor.IsAttacking ? 0.5f : 1f;
                // Smoothly turn toward movement
                ModelContainer.rotation = Quaternion.Slerp(
                    ModelContainer.rotation,
                    targetRotation,
                    Time.deltaTime * (TurnSpeed * turnSpeedFactor)
                );
            }
        }
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
                var smoothingFactor = _playerEntity.AbilityExecutor.IsAttacking ? 1f : 2f;
                ModelContainer.transform.rotation = Quaternion.Slerp(ModelContainer.transform.rotation, targetRotation, (smoothingFactor * rotationSmoothing) * Time.deltaTime);
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