using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class WinPartyManager : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _confettiParticles;
    [SerializeField] private PlaySound _partySound;

    [SerializeField] private bool _playOnStart;
    [SerializeField] private float _delay;

    private void Start()
    {
        if (_playOnStart)
        {
            Invoke(nameof(StartWinParty), _delay);
        }
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void StartWinParty()
    {
        foreach (var particle in _confettiParticles)
        {
            particle.Play();
        }

        _partySound.SoundPlay();
    }
}
