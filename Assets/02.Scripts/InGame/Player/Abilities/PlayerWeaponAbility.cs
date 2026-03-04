using Photon.Pun;
using UnityEngine;

public class PlayerWeaponAbility : PlayerAbility
{
    [SerializeField] private float _scaleAmount = 0.1f;
    [SerializeField] private int _scorePerScaleIncrease  = 1000;

    private void Start()
    {
        ScoreManager.OnDataChanged += UpdateScale;
    }

    private void OnDestroy()
    {
        ScoreManager.OnDataChanged -= UpdateScale;
    }

    private void UpdateScale(int changedActorNumber)
    {
        int actorNumber = Owner.PhotonView.OwnerActorNr;
        if (changedActorNumber != actorNumber) return;
        if (!ScoreManager.Instance.Scores.TryGetValue(actorNumber, out ScoreData score)) return;
        int increaseAmount = score.Score / _scorePerScaleIncrease;
        Vector3 scale = Vector3.one + Vector3.one * (_scaleAmount * increaseAmount);
        transform.localScale = scale;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!Owner.PhotonView.IsMine) return;
        if (other.transform == Owner.transform) return;
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(Owner.Stat.Damage, actorNumber);
        }
    }
}