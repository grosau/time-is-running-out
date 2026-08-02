using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public int currentWave;
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] List<Transform> spawnPoints;
    private List<GameObject> activeEnemies;
    [SerializeField] int enemiesPerWave;


    void Start()
    {
        activeEnemies = new List<GameObject>();
        GameManager.OnGameStateChanged += HandleStateChange;
    }

    void HandleStateChange(GameManager.GameState newState)
    {
        switch (newState)
        {

            case GameManager.GameState.Arena:
                SpawnWave();
                break;
            case GameManager.GameState.Corridor:
            case GameManager.GameState.PowerUpSelection:
            case GameManager.GameState.GameOver:
                // stop spawning
                break;
        }
    }


    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleStateChange;
    }

    void SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];
            GameObject enemy = Instantiate(enemyPrefabs[0], spawnPoint.position, Quaternion.identity);
            activeEnemies.Add(enemy);
        }
        currentWave++;
    }
}
