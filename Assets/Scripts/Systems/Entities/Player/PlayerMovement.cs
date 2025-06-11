using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Movement
    [Header("Movement")]
    public Vector3 Velocity;
    public bool IsDodgeSlowdown = false;

    #endregion
    #region Rotation
    [Header("Rotation")]
    public LayerMask GroundLayer;
    public Transform ModelContainer;
    public float rotationSmoothing;
    private Plane _groundPlane;

    public float alignmentThreshold = 0.5f;

    #endregion
    #region Dodge Variables

    public bool IsDodging = false;
    public float DodgeCooldown = 0;
    private float _dodgeDirection;
    private Vector2 _dodgeStartPosition;

    #endregion
    #region References
    private PlayerEntity _playerEntity;
    private PlayerConfiguration _config;

    private PlayerController _characterController;
    private BoxCollider _playerCollider;
    private EntityStats _playerStats;
    private Rigidbody _rb;
    #endregion

    internal void Initialize()
    {
        _playerEntity = GetComponent<PlayerEntity>();
        ModelContainer = transform.Find("Model");
        _config = GetComponent<PlayerConfiguration>();
        _characterController = GetComponent<PlayerController>();
        _playerCollider = GetComponentInChildren<BoxCollider>();
        _playerStats = _playerEntity.Stats;
        _rb = GetComponent<Rigidbody>();
         _groundPlane = new Plane(Vector3.up, transform.position);
    }
    private void Update()
    {
        if (DodgeCooldown > 0)
            DodgeCooldown -= Time.deltaTime;

        HandleMove();
        HandleRotation();

        SetAnimationInfo();
    }

    private void SetAnimationInfo()
    {
        float moveSpeed = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z).magnitude;
        _playerEntity.Animator.SetFloat("moveSpeed", moveSpeed);

        Vector3 moveDirection = UserInput.Instance.MovementInput.normalized; //WASD input
        Vector3 facingDirection = transform.forward;

        // Check dot product: <-0.5 = moving backward, between -0.5 and 0.5 = strafing, >0.5 = forward
        Vector3 translatedInput = new Vector3(moveDirection.x, 0, moveDirection.y);
        float alignment = Vector3.Dot(facingDirection, translatedInput);
        float sideAlignment = Vector3.Dot(transform.right, moveDirection);


        //todo adjust threshold to have smooth front/back/strafe movement
        bool isStrafing = alignment > -alignmentThreshold && alignment < alignmentThreshold;
        bool isStrafingLeft = isStrafing && sideAlignment < -0.1f;
        bool isStrafingRight = isStrafing && sideAlignment > 0.1f;

        _playerEntity.Animator.SetBool("isMovingBackward", alignment < -alignmentThreshold);
        _playerEntity.Animator.SetBool("isStrafingLeft", isStrafingLeft);
        _playerEntity.Animator.SetBool("isStrafingRight", isStrafingRight);
        _playerEntity.Animator.SetBool("isMovingForward", alignment > alignmentThreshold);

        Debug.Log("Alignment = " + alignment.ToString());
    }

    #region Collider Checks

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            IsDodging = false;
            _dodgeDirection = 0;
            Velocity = new Vector3(0, Velocity.y);
        }
    }

    #endregion

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

        var moveVector = new Vector3(UserInput.Instance.MovementInput.x * _playerStats.MoveSpeed, 0, UserInput.Instance.MovementInput.y * _playerStats.MoveSpeed);
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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothing * Time.deltaTime);
            }
        }
    }

    #endregion

    #region Dodging

    public bool CanDodge()
    {
        return DodgeCooldown <= 0;
    }

    public void StartDodge()
    {
        //if (DodgeCooldown > 0)
        //    return;

        //Debug.Log("Start Dodge");

        //IsDodging = true;
        //EnableCollision(true);

        //DodgeCooldown = _playerStats.DodgeCD;

        //_dodgeStartPosition = transform.position;
        //if (UserInput.Instance.MovementInput.x != 0)
        //    _dodgeDirection = UserInput.Instance.MovementInput.x > 0 ? 1 : -1;
        //else
        //    _dodgeDirection = _playerEntity.FacingRight ? 1 : -1;

        //StartCoroutine(StopDodge());
    }

    private void EnableCollision(bool changeToDodge)
    {
        if (changeToDodge)
        {
            _playerEntity.GetCollider().gameObject.layer = LayerMask.NameToLayer("Dodge");
            _playerEntity.gameObject.layer = LayerMask.NameToLayer("Dodge");
        }
        else
        {
            _playerEntity.GetCollider().gameObject.layer = LayerMask.NameToLayer("Player");
            _playerEntity.gameObject.layer = LayerMask.NameToLayer("Player");
        }

    }

    public void HandleDodge()
    {
        if (!IsDodging)
            return;
        if (IsDodgeSlowdown)
            return;

        if (_playerStats == null)
        {
            throw new System.Exception("PlayerStats not initialized");
        }
        else
        {
            var moveVector = new Vector3(_dodgeDirection * _config.DodgeSpeed, 0);
            _characterController.Move(moveVector);
            //HandleRotation(_dodgeDirection);
        }
    }

    private IEnumerator SlowDownOverTime(float duration)
    {
        IsDodgeSlowdown = true;
        Debug.Log("Start Slowdown");
        float currentDuration = duration;

        while (currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
            var percentage = currentDuration / duration;
            if (percentage < 0) percentage = 0;

            var newSpeed = Mathf.Lerp(_playerStats.MoveSpeed, _config.DodgeSpeed, percentage);
            Debug.Log("Slowdown Speed: " + newSpeed);

            var moveVector = new Vector3(_dodgeDirection * newSpeed, 0);
            _characterController.Move(moveVector);
            yield return null;
        }
        Debug.Log("Slow Down Finished. IsDodging = " + IsDodging);
        IsDodging = false;
        EnableCollision(false);
        IsDodgeSlowdown = false;
    }

    private IEnumerator StopDodge()
    {
        yield return new WaitForSeconds(_config.DodgeDuration);
        StartCoroutine(SlowDownOverTime(_config.DodgeSlowDownDuration));
    }

    #endregion

    #region Locking
    private Coroutine lockCoroutine = null;
    private float _currentLockDuration = 0f;
    internal void LockCharacter(float duration)
    {
        // If the new duration is longer, update it
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