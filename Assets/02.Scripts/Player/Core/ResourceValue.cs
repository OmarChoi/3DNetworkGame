using System;
using UnityEngine;

[Serializable]
public class ResourceValue
{
    [SerializeField] private float _current = 100f;
    [SerializeField] private float _max = 100f;

    public float Current => _current;
    public float Max => _max;
    public float Normalized => _max > 0f ? _current / _max : 0f;

    public bool SetCurrent(float value)
    {
        float clampedValue = Mathf.Clamp(value, 0f, _max);
        if (Mathf.Approximately(_current, clampedValue)) return false;
        _current = clampedValue;
        return true;
    }

    public bool Add(float delta)
    {
        return SetCurrent(_current + delta);
    }

    public void ClampCurrent()
    {
        _current = Mathf.Clamp(_current, 0f, _max);
    }
}
