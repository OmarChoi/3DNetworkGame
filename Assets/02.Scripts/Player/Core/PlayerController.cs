using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, IPunObservable
{
    public PhotonView PhotonView { get; private set; }
    public CharacterController Controller { get; private set; }

    private Renderer[] _renderers;
    public PlayerStat Stat;
    public static event Action<PlayerController> OnLocalPlayerCreated;
    public static event Action<PlayerController> OnPlayerDied;
    public event Action<PlayerStat> OnStatChanged;
    public event Action OnDeath;
    public event Action OnReset;

    public bool IsDead => Stat.Health.Current <= 0f;
    private const int KillScore = 1000;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        Controller = GetComponent<CharacterController>();
        _renderers = GetComponentsInChildren<Renderer>();
        EnsureStat();
    }

    private void Start()
    {
        NotifyStatChanged();

        if (PhotonView.IsMine)
        {
            OnLocalPlayerCreated?.Invoke(this);
        }
    }
    
    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();
    
    public T GetAbility<T>() where T : PlayerAbility
    {
        var type = typeof(T);

        if (_abilitiesCache.TryGetValue(type, out PlayerAbility ability))
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

    public void SetHealth(float health)
    {
        EnsureStat();
        if (!Stat.SetHealth(health)) return;
        NotifyStatChanged();

        if (IsDead)
        {
            Controller.enabled = false;
            OnPlayerDied?.Invoke(this);
            OnDeath?.Invoke();
            if (PhotonView.IsMine)
            {
                CharacterSpawner.Instance.StartRespawn(this);
            }
        }
    }

    public void SetStamina(float stamina)
    {
        EnsureStat();
        if (!Stat.SetStamina(stamina)) return;
        NotifyStatChanged();
    }

    public void AddStamina(float delta)
    {
        EnsureStat();
        if (!Stat.AddStamina(delta)) return;
        NotifyStatChanged();
    }

    public bool TryUseStamina(float amount)
    {
        EnsureStat();
        float before = Stat.Stamina.Current;
        bool success = Stat.TryUseStamina(amount);
        if (success && !Mathf.Approximately(before, Stat.Stamina.Current))
        {
            NotifyStatChanged();
        }

        return success;
    }

    private void EnsureStat()
    {
        Stat ??= new PlayerStat();
        Stat.EnsureInitialized();
    }

    private void NotifyStatChanged()
    {
        OnStatChanged?.Invoke(Stat);
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            EnsureStat();
            stream.SendNext(Stat.Health.Current);
            stream.SendNext(Stat.Stamina.Current);
        }
        else if (stream.IsReading)
        {
            EnsureStat();
            float health = (float)stream.ReceiveNext();
            float stamina = (float)stream.ReceiveNext();

            bool hasChanged = false;
            hasChanged |= Stat.SetHealth(health);
            hasChanged |= Stat.SetStamina(stamina);

            if (hasChanged)
            {
                NotifyStatChanged();
            }
        }
    }
    
    public void SetVisible(bool visible)
    {
        foreach (var rd in _renderers)
        {
            rd.enabled = visible;
        }
        if (Controller != null) Controller.enabled = visible;
    }

    public void ResetPlayer(Vector3 spawnPosition)
    {
        PhotonView.RPC(nameof(RespawnRPC), RpcTarget.All, spawnPosition);
    }

    [PunRPC]
    private void RespawnRPC(Vector3 spawnPosition)
    {
        SetVisible(false);
        transform.position = spawnPosition;

        if (PhotonView.IsMine)
        {
            EnsureStat();
            Stat.Health.Reset();
            Stat.Stamina.Reset();
            NotifyStatChanged();
        }

        Controller.enabled = true;
        SetVisible(true);
        OnReset?.Invoke();
    }

    public void TakeDamage(float damage, int actorNumber)
    {
        if (IsDead) return;
        PhotonView.RPC(nameof(TakeDamageRPC), RpcTarget.All, damage, actorNumber);
    }

    [PunRPC]
    private void TakeDamageRPC(float damage, int actorNumber)
    {
        if (IsDead) return;
        SetHealth(Stat.Health.Current - damage);
        if (!IsDead) return;
        PhotonRoomManager.Instance.OnPlayerDeath(actorNumber, PhotonView.Owner.ActorNumber);
        ScoreManager.Instance.RequestAddScore(actorNumber, KillScore);
    }
}
