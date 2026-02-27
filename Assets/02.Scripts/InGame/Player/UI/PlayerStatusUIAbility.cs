using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUIAbility : PlayerAbility
{
    [SerializeField] private Image _healthGauge;
    [SerializeField] private Image _staminaGauge;

    private void OnEnable()
    {
        if (Owner == null) return;

        Owner.OnStatChanged += OnStatChanged;
        RefreshUI();
    }

    private void OnDisable()
    {
        if (Owner == null) return;

        Owner.OnStatChanged -= OnStatChanged;
    }

    private void RefreshUI()
    {
        OnStatChanged(Owner.Stat);
    }

    private void OnStatChanged(PlayerStat stat)
    {
        _healthGauge.fillAmount = stat.Health.Normalized;
        _staminaGauge.fillAmount = stat.Stamina.Normalized;
    }
}
