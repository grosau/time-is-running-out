using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityMeter : MonoBehaviour
{
    [SerializeField] float maxSanity;
    public float CurrentSanity { get; private set; }

    bool depleteSanity = false;
    [SerializeField] float sanityDepletionRate;

    void Start()
    {
        GameManager.OnGameStateChanged += HandleStateChange;
        CurrentSanity = maxSanity;
    }

    void Update()
    {
        if (depleteSanity == true)
        {
            CurrentSanity -= sanityDepletionRate * Time.deltaTime;
            if (CurrentSanity <= 0)
            {
                CurrentSanity = 0;
                GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
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
}
