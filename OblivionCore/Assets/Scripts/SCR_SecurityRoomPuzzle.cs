using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SecurityRoomPuzzle : MonoBehaviour
{

    [SerializeField] private float countdown;

    public Wave[] waves;
    public int currentWaveIndex = 0;

    private bool readyToCountDown;
    private void Start()
    {
        readyToCountDown = true;

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }
    void Update()
    {
        if (currentWaveIndex >= waves.Length)
        {
            // DISPLAY RED NUMBER FOR THE CODE



            return;
        }

        if (readyToCountDown == true)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0)
        {
            readyToCountDown = false;

            countdown = waves[currentWaveIndex].timeToNextWave;

            StartCoroutine(SpawnWave());
        }

        if (waves[currentWaveIndex].enemiesLeft == 0)
        {
            readyToCountDown = true;

            currentWaveIndex++;
        }
    }
    private IEnumerator SpawnWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
            {
                SCR_EnemyAI enemy = Instantiate(waves[currentWaveIndex].enemies[i], waves[currentWaveIndex].spawnPoints[i].transform.position, waves[currentWaveIndex].spawnPoints[i].transform.rotation);

                yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
            }
        }
    }
}

[System.Serializable]
public class Wave
{
    public GameObject[] spawnPoints;
    public SCR_EnemyAI[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}

