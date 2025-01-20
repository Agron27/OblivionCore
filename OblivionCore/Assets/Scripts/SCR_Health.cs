using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class SCR_Health : MonoBehaviour
{
    public float healthPoints = 100f;
    private bool isDead = false;

    private SCR_Movement movementScript;

    private SCR_SecurityRoomPuzzle waveSpawner;
    private GameObject spawnerGameObject;
    void Start()
    {
        movementScript = GetComponent<SCR_Movement>();

        spawnerGameObject = GameObject.FindWithTag("Spawner");
        waveSpawner = spawnerGameObject.GetComponent<SCR_SecurityRoomPuzzle>();
    }

    public void TakeDamage(float damage)
    {
        healthPoints = Mathf.Max(healthPoints - damage, 0);
        Debug.Log(healthPoints);
        if (healthPoints == 0)
        {
            Die();
        }

    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        // disabel controls
        if (movementScript != null)
            movementScript.enabled = false;

        // animation logic

        // wave logic (NOT THE BEST)
        if (this.gameObject.tag != "Enemy")
        {
            waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft--;
            Debug.Log(waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft);
        }
    }
    public bool IsDead()
    {
        return isDead;
    }

    public void RestoreHealth(float amount)
    {
        healthPoints = Mathf.Min(healthPoints + amount, 100f);
    }
}
