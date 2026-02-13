using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using NaughtyAttributes;

public class LanternController : MonoBehaviour
{
    [SerializeField] private float _effectRange;
    [SerializeField] private float _maxCharge;
    [SerializeField] private float _chargeAmount;
    [SerializeField] private float _chargeDecay;
    [SerializeField] private PlaySound _chargeSound;

    [SerializeField, ReadOnly] private float _currentCharge;

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

    private XRGrabInteractable _grabInteractable;

    private void Start()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();

        // Subribe to events to turn on and off the lantern
        _grabInteractable.selectEntered.AddListener((_) => InUse = true);
        _grabInteractable.selectExited.AddListener((_) => InUse = false);

        // Subrice to the event to charge the lantern
        _grabInteractable.activated.AddListener((_) => DoCharge());

        InUse = false;
    }

    private void Update()
    {
        if (InUse && _spotLight.intensity > 0) DoShine();
    }
    private void FixedUpdate()
    {
        DoDecay();
    }

    private void DoDecay()
    {
        if (_inUse) CurrentCharge -= _chargeDecay;
    }

    private void DoCharge()
    {
        if (_inUse)
        {
            CurrentCharge += _chargeAmount;
            _chargeSound.SoundPlay();
        }
    }

    private void UpdateLightIntensity()
    {
        float chargeRatio = _currentCharge/_maxCharge;

        _spotLight.intensity = Mathf.Lerp(0, _lightMaxIntensity, chargeRatio);
    }

    private void DoShine()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, transform.forward, out hit, _effectRange);

        if (hit.collider.TryGetComponent<IShineable>(out IShineable shineable))
        {
            shineable.Shine();
        }
    }

}
