using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class BearController : MonoBehaviour
{
    private Dictionary<Type, BearState> _states;
    private readonly Dictionary<Type, BearAbility> _abilitiesCache = new Dictionary<Type, BearAbility>();
    private BearState _currentState;
   
    private NavMeshAgent _agent;
    private Animator _animator;

    public BearStat Stats;
    public Transform Target { get; private set; }
    
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        SetStates();
    }
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        _currentState.Update();
    }

    private void SetStates()
    {
        _states = new Dictionary<Type, BearState>
        {
            {typeof(BearAttackState), new BearAttackState(this)},
            {typeof(BearChaseState), new BearChaseState(this)},
            {typeof(BearDeathState), new BearDeathState(this)},
            {typeof(BearHitState), new BearHitState(this)},
            {typeof(BearIdleState), new BearIdleState(this)},
            {typeof(BearPatrolState), new BearPatrolState(this)},
            {typeof(BearReturnState), new BearReturnState(this)},
            {typeof(BearWaitAttackState), new BearWaitAttackState(this)},
        };
        _currentState = _states[typeof(BearIdleState)];
    }
    
    public T GetAbility<T>() where T : BearAbility
    {
        var type = typeof(T);

        if (_abilitiesCache.TryGetValue(type, out BearAbility ability))
        {
            return ability as T;
        }
        
        ability = GetComponent<T>();

        if (ability != null)
        {
            _abilitiesCache[ability.GetType()] = ability;

            return ability as T;
        }
        
        throw new Exception($"어빌리티 {type.Name}을 {gameObject.name}에서 찾을 수 없습니다.");
    }
    
    public void ChangeState<T>() where T : BearState
    {
        _currentState?.Exit();
        _currentState = _states[typeof(T)];
        _currentState.Enter();
    }

    public void SetTarget(Transform target)
    {
        if (Target != null) return;
        Target = target;
        ChangeState<BearChaseState>();
    }

    public bool CanAttack()
    {
        if (Target == null) return false;
        float distance = Vector3.Distance(Target.position, transform.position);
        return distance <= Stats.AttackDistance;
    }

    public void MoveTo(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }
}