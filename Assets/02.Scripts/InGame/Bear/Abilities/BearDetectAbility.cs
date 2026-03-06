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
        RemovePlayerAndRetarget(player);
    }

    private void RemoveDeathPlayer(PlayerController player)
    {
        ClearList();
        RemovePlayerAndRetarget(player);
    }

    private void RemoveLeftPlayer(Player photonPlayer)
    {
        ClearList();
        if (photonPlayer == null) return;

        PlayerController found = _playersInRange.Find(
            pc => pc != null && pc.PhotonView.Owner.ActorNumber == photonPlayer.ActorNumber);

        if (found == null) return;
        RemovePlayerAndRetarget(found);
    }

    private void RemovePlayerAndRetarget(PlayerController player)
    {
        bool wasTarget = player.transform == Owner.Target;
        _playersInRange.Remove(player);

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
            float dist = (ownerPos - _playersInRange[i].transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                nearest = _playersInRange[i];
            }
        }

        return nearest != null ? nearest.transform : null;
    }
}
