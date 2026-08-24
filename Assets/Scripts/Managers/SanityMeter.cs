using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityMeter : MonoBehaviour
{
    [SerializeField] float maxSanity;
    public float CurrentSanity { get; private set; }
    public float MaxSanity { get; private set; }

    bool depleteSanity = false;
    [SerializeField] float sanityDepletionRate;
    private bool extremeSanityTriggered = false;

    public delegate void SanityChanged(float currentSanity);
    public static event SanityChanged _onSanityChanged;

    public delegate void SanityDepleted();
    public static event SanityDepleted OnExtremeSanity;

    void Start()
    {
        GameManager.OnGameStateChanged += HandleStateChange;
        CurrentSanity = maxSanity;
        MaxSanity = maxSanity;
    }

    void Update()
    {
        if (depleteSanity == true)
        {
            CurrentSanity -= sanityDepletionRate * Time.deltaTime;
            _onSanityChanged?.Invoke(CurrentSanity);

            if (CurrentSanity <= 0)
            {
                CurrentSanity = 0;
                if (!extremeSanityTriggered)
                {
                    extremeSanityTriggered = true;
                    OnExtremeSanity?.Invoke();
                }
            }
            else
            {
                extremeSanityTriggered = false;
            }
        }
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleStateChange;
    }

    void HandleStateChange(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.MainMenu:
                depleteSanity = false;
                break;
            case GameManager.GameState.Arena:
                depleteSanity = true;
                break;
            case GameManager.GameState.Corridor:
                depleteSanity = false;
                break;
            case GameManager.GameState.PowerUpSelection:
                depleteSanity = false;
                break;
            case GameManager.GameState.GameOver:
                depleteSanity = false;
                break;
        }
    }

    public void RestoreSanity(float amount)
    {
        CurrentSanity = Mathf.Min(CurrentSanity + amount, MaxSanity);
    }
}
