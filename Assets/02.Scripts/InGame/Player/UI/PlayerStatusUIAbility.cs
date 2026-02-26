using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUIAbility : PlayerAbility
{
    [SerializeField] private Image _healthGauge;
    [SerializeField] private Image _staminaGauge;

    private void OnEnable()
    {
        if (_owner == null) return;

        _owner.OnStatChanged += OnStatChanged;
        RefreshUI();
    }

    private void OnDisable()
    {
        if (_owner == null) return;

        _owner.OnStatChanged -= OnStatChanged;
    }

    private void RefreshUI()
    {
        OnStatChanged(_owner.Stat);
    }

    private void OnStatChanged(PlayerStat stat)
    {
        _healthGauge.fillAmount = stat.Health.Normalized;
        _staminaGauge.fillAmount = stat.Stamina.Normalized;
    }
}
