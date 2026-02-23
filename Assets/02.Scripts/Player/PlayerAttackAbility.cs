using System;
using Photon.Pun;
using UnityEngine;

public class PlayerAttackAbility : MonoBehaviour
{
    public static Action OnAttack;
    private bool _isMine = true;

    private void Start()
    {
        PhotonView view = gameObject.GetComponent<PhotonView>();
        _isMine = view.IsMine;
    }
    
    private void Update()
    {
        if (!_isMine) return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnAttack?.Invoke();
        }
    }
}
