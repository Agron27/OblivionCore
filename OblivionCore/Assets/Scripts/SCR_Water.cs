using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Water : MonoBehaviour
{
    [SerializeField] SCR_Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth = other.gameObject.GetComponent<SCR_Health>();
            playerHealth.TakeDamage(10);
            print("Hit player");
            //StartCoroutine(DamagePlayer() );

        }
    }

    /*IEnumerator DamagePlayer()
    {
        playerHealth = other.gameObject.GetComponent<SCR_Health>();
        playerHealth.TakeDamage(10);
        yield return new WaitForSeconds(0.5f);
        print("Hit player");
    }
    */
}
