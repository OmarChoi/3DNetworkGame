using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class BearController : MonoBehaviourPun
{
    private Dictionary<Type, BearState> _states;
    private readonly Dictionary<Type, BearAbility> _abilitiesCache = new Dictionary<Type, BearAbility>();
    private BearState _currentState;
    private bool _initialized;

    public BearStat Stats;
    public event Action<Transform> OnTargetDetected;
    public event Action OnTargetLost;
    public event Action OnAttackAnimationEnd;
    public event Action OnHitAnimationEnd;

    public Vector3 SpawnPosition { get; private set; }
    public Transform Target { get; private set; }
    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }


    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        SpawnPosition = transform.position;

        SetStates();
    }
    
    private void Update()
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;
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

    [PunRPC]
    public void SetAnimTriggerRpc(int triggerHash)
    {
        Animator.SetTrigger(triggerHash);
    }

    public void SetTarget(Transform target)
    {
        if (Target != null) return;
        Target = target;
        OnTargetDetected?.Invoke(target);
    }

    public void ClearTarget()
    {
        Target = null;
        OnTargetLost?.Invoke();
    }

    public bool CanAttack()
    {
        if (Target == null) return false;
        float distance = Vector3.Distance(Target.position, transform.position);
        return distance <= Stats.AttackDistance;
    }

    public void NotifyHit()
    {
        if (_currentState is BearHitState or BearDeathState) return;
        ChangeState<BearHitState>();
    }

    public void NotifyDeath()
    {
        if (_currentState is BearDeathState) return;
        ChangeState<BearDeathState>();
    }
    
    public void AttackHit()
    {
        GetAbility<BearAttackAbility>().Attack();
    }

    public void AttackAnimationEnd()
    {
        OnAttackAnimationEnd?.Invoke();
    }

    public void HitAnimationEnd()
    {
        OnHitAnimationEnd?.Invoke();
    }
}