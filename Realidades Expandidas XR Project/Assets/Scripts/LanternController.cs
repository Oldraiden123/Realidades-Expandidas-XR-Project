using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LanternController : MonoBehaviour
{
    [SerializeField] private float _maxCharge;
    [SerializeField] private float _chargeAmount;
    [SerializeField] private float _chargeDecay;

    [SerializeField] private float _currentCharge;

    public float CurrentCharge {
        get => _currentCharge;
        set
        {
            if (value < 0) _currentCharge = 0;
            else if (value > _maxCharge) _currentCharge = _maxCharge;
            else _currentCharge = value;

            UpdateLightIntensity();
        }
    }

    [SerializeField] private Light _spotLight;
    [SerializeField] private float _lightMaxIntensity;

    [SerializeField] private bool _inUse = false;

    public bool InUse {
        get => _inUse; 
        set
        {
            if (value == false) CurrentCharge = 0;

            _inUse = value;
        } 
    }

    private void OnActivate()
    {
        if (_inUse) DoCharge();
        Debug.Log("Winning");
    }

    private void Update()
    {
        if (_inUse)
        {
            DoDecay();
        }
    }

    private void DoDecay()
    {
        CurrentCharge -= _chargeDecay;
    }

    private void DoCharge()
    {
        CurrentCharge += _chargeAmount;
    }

    private void UpdateLightIntensity()
    {
        float chargeRatio = _currentCharge/_maxCharge;

        _spotLight.intensity = Mathf.Lerp(0, _lightMaxIntensity, chargeRatio);
    }

}
