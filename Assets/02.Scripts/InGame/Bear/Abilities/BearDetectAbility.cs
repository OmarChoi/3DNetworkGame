using System;
using System.Collections.Generic;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;

public class BearDetectAbility : BearAbility
{
    private readonly List<PlayerController> _playersInRange = new List<PlayerController>();

    private void ClearList() => _playersInRange.RemoveAll(pc => pc == null || pc.IsDead);
    
    private void Start()
    {
        PlayerController.OnPlayerDied += RemoveDeathPlayer;
        PhotonRoomManager.Instance.OnPlayerLeft += RemoveLeftPlayer;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDied -= RemoveDeathPlayer;
        if (PhotonRoomManager.Instance == null) return;
        PhotonRoomManager.Instance.OnPlayerLeft -= RemoveLeftPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerController player)) return;
        if (_playersInRange.Contains(player)) return;

        _playersInRange.Add(player);

        if (Owner.Target == null)
        {
            Owner.SetTarget(GetNearestTarget());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out PlayerController player)) return;

        bool wasTarget = other.transform == Owner.Target;
        _playersInRange.Remove(player);

        if (wasTarget)
        {
            Owner.SetTarget(GetNearestTarget());
        }
    }

    private void RemoveDeathPlayer(PlayerController player)
    {
        ClearList();
        bool wasTarget = player.transform == Owner.Target;
        _playersInRange.Remove(player);

        if (wasTarget)
        {
            Owner.SetTarget(GetNearestTarget());
        }
    }

    private void RemoveLeftPlayer(Player photonPlayer)
    {
        ClearList();
        if (photonPlayer == null) return;
        
        PlayerController found = null;

        foreach (PlayerController pc in _playersInRange)
        {
            if (pc != null && pc.PhotonView.Owner.ActorNumber != photonPlayer.ActorNumber) continue;
            found = pc;
            break;
        }

        if (found == null) return;
        bool wasTarget = (found.transform == Owner.Target);
        _playersInRange.Remove(found);

        if (wasTarget)
        {
            Owner.SetTarget(GetNearestTarget());
        }
    }

    private Transform GetNearestTarget()
    {
        ClearList();
        if (_playersInRange.Count == 0) return null;

        float minDist = float.MaxValue;
        PlayerController nearest = null;
        Vector3 ownerPos = Owner.transform.position;

        for (int i = 0; i < _playersInRange.Count; i++)
        {
            float dist = Vector3.Distance(ownerPos, _playersInRange[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = _playersInRange[i];
            }
        }

        return nearest != null ? nearest.transform : null;
    }
}
