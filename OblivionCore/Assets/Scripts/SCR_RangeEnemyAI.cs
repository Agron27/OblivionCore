using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SCR_RangeEnemyAI : SCR_EnemyAI
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float attackCooldown = 1f;

    [SerializeField] Light spotLight;

    protected override void Attack()
    {
        if (timeSinceLastAttack > attackCooldown)
        {
            FireProjectile();
            timeSinceLastAttack = 0f;
        }
    }

    private void FireProjectile()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectileObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }
    }

    protected override void ChaseBehaviour()
    {
        base.ChaseBehaviour();

        spotLight.color = Color.red;      
    }

    protected override void PatrolBehaviour()
    {
        base.PatrolBehaviour();

        spotLight.color = new Color(0.9215687f, 0.9137256f, 0.6705883f);
    }

}
