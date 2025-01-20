using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Projectile : MonoBehaviour
{
    public float lifetime = 1f;
    public float damage = 10f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SCR_Health playerHealth = other.GetComponent<SCR_Health>();
            playerHealth.TakeDamage(damage);

            Destroy(gameObject);
        }
    }

}
