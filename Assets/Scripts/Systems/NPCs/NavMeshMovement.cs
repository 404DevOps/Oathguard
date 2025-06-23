using System;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMovement : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    NPCEntity _entity;
    EntityBase _target;

    public float DistanceThreshold;

    float _distance;
    bool _paused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _paused = true;
        GameEvents.OnGameStarted.AddListener(OnGameStarted);

    }
    private void OnDisable()
    {
        GameEvents.OnGameStarted.RemoveListener(OnGameStarted);
    }

    private void OnGameStarted()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _target = EntityManager.Instance.Player;
        _paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_paused) return;

        _distance = Vector3.Distance(this.transform.position, _target.transform.position);
        if (_distance > DistanceThreshold)
        {
            _navMeshAgent.SetDestination(_target.transform.position);
            _navMeshAgent.isStopped = false;
        }
        else
        {
            _navMeshAgent.isStopped = true;
            Debug.Log("In Range for Attack");
        }
    }

    public void TogglePause(bool isPause)
    {
        if (_paused != isPause)
            _paused = isPause;
    }
}
