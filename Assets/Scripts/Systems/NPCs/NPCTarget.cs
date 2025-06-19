using UnityEngine;
using UnityEngine.AI;

public class NPCTarget : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    NPCEntity _entity;
    EntityBase _target;

    float _distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        var _distance = Vector3.Distance(this.transform.position, _target.transform.position);
        if (_distance > 1)
        {
            _navMeshAgent.isStopped = false;
        }
        else
        { 
            _navMeshAgent.isStopped = true;
            Debug.Log("In Range for Attack");
        }
    }
}
