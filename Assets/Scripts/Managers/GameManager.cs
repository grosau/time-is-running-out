using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Corridor,
        Arena,
        PowerUpSelection,
        GameOver
    }

    public GameState currentGameState;

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(InitializeGame());
    }

    IEnumerator InitializeGame()
    {
        yield return null;
        ChangeState(GameState.MainMenu);
    }

    public delegate void GameStateChanged(GameState newState);
    public static event GameStateChanged OnGameStateChanged;

    public void ChangeState(GameState newState)
    {
        currentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public void StartGame()
    {
        Debug.Log("StartGame called");
        ChangeState(GameState.Corridor);
    }

    public void RetryGame()
    {
        ChangeState(GameState.Corridor);
    }

    public void GoToMainMenu()
    {
        ChangeState(GameState.MainMenu);
    }
}
