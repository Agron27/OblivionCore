using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WallExplosion : MonoBehaviour
{
    public Transform explosionCentre; 
    public float explosionForce = 10f; 
    public float explosionRadius = 1f; 
    public float upwardModifier = 1f; 
    private GameObject[] wallPieces; 

    private void Awake()
    {
        wallPieces = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            wallPieces[i] = transform.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
        // Change Later!!!
        if (Input.GetKey(KeyCode.K)) TriggerExplosion() ;
    }

    public void TriggerExplosion()
    {
        foreach (GameObject piece in wallPieces)
        {
            Rigidbody rb = piece.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Make Rigidbody dynamic
                rb.isKinematic = false;
                rb.AddExplosionForce(explosionForce, explosionCentre.position, explosionRadius, upwardModifier, ForceMode.Impulse);

                StartCoroutine(DestroyObjectAfterTime());
            }
        }
    }

    IEnumerator DestroyObjectAfterTime()
    {
        yield return new WaitForSeconds(7f);
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(explosionCentre.transform.position, explosionRadius);
    }
}
