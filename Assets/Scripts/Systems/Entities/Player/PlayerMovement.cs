using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;
using Unity.VisualScripting;
using UnityEngine.Android;

public class PlayerMovement : MonoBehaviour
{
    #region Movement

    public Vector3 Velocity;
    public bool IsDodgeSlowdown = false;

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
    private BoxCollider2D _playerCollider;
    private EntityStats _playerStats;
    private Rigidbody2D _rb;
    #endregion

    internal void Initialize()
    {
        _playerEntity = GetComponent<PlayerEntity>();
        _config = GetComponent<PlayerConfiguration>();
        _characterController = GetComponent<PlayerController>();
        _playerCollider = GetComponentInChildren<BoxCollider2D>();
        _playerStats = _playerEntity.Stats;
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (DodgeCooldown > 0)
            DodgeCooldown -= Time.deltaTime;
    }

    #region Collider Checks

    private void OnCollisionStay2D(Collision2D collision)
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

        var moveVector = new Vector3(UserInput.Instance.MovementInput.x * _playerStats.MoveSpeed, UserInput.Instance.MovementInput.x * _playerStats.MoveSpeed);
        _characterController.Move(moveVector);
        HandleFlip(UserInput.Instance.MovementInput.x);
    }
    private void HandleFlip(float xAxis)
    {
        //if (xAxis != 0)
        //{
        //    //flip character
        //    transform.localScale = xAxis < 0 ? new Vector3(-1, transform.localScale.y) : new Vector3(1, transform.localScale.y);
        //    _playerEntity.FacingRight = xAxis > 0;
        //}
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
            HandleFlip(_dodgeDirection);
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