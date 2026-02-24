using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, IPunObservable
{
    public PhotonView PhotonView { get; private set; }
    public PlayerStat Stat;
    public event Action<PlayerStat> OnStatChanged;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        EnsureStat();
    }

    private void Start()
    {
        NotifyStatChanged();
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
    
    [PunRPC]
    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage");
        SetHealth(Stat.Health.Current - damage);
    }
}
