using UnityEngine;
using UnityEngine.UI;

public class UI_BearHealth : BearAbility
{
    [SerializeField] private Image _healthGauge;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        UpdateHealthGauge();
    }

    private void OnEnable()
    {
        Owner.OnHealthChanged += UpdateHealthGauge;
    }

    private void OnDisable()
    {
        Owner.OnHealthChanged -= UpdateHealthGauge;
    }

    private void Update()
    {
        transform.forward = _camera.transform.forward;
    }

    private void UpdateHealthGauge()
    {
        _healthGauge.fillAmount = Owner.Stats.Health.Normalized;
    }
}
