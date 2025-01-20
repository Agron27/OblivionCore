using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_HealthPickUp : MonoBehaviour, IInteractable
{
    float restoreHealth = 10f;

    private GameObject player;
    SCR_Health playerHealth;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<SCR_Health>();
    }

    public void Interact()
    {
        playerHealth.RestoreHealth(restoreHealth);
        Debug.Log("Health restored by:" + restoreHealth);
    }

}

