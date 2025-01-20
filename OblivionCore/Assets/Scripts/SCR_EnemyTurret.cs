using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EnemyTurret : MonoBehaviour
{
    private GameObject player; //if player in range then shoot
    public Vector3 playerLocation;
    public Vector3 turretLocation;
    private Vector3 turretRotation; //track player
    [SerializeField] LineRenderer laserLine;
    [SerializeField] Transform laserOrigin;
    [SerializeField] bool bIsIce; //Will this turret drop the ice module
    public bool bIsActive;
    //[SerializeField] float rotateSpeed = 5.0f;
    [SerializeField] GameObject pivotPoint;
    //public int lengthOfLineRenderer = 2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        laserLine = GetComponent<LineRenderer>();
        turretLocation = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsActive)
        {
            StartCoroutine(followPlayer());
        }
        
        /*playerLocation = player.transform.position;

        if (Vector3.Distance(playerLocation, turretLocation) < 5.0f)
        {
            Debug.Log("Player in range");

            ActivateLaser();

            pivotPoint.transform.LookAt(playerLocation);

            laserLine.SetPosition(0, laserOrigin.position);
                   
            Ray hit = new Ray(laserOrigin.position, laserOrigin.forward);
            if (Physics.Raycast(hit, out RaycastHit hitInfo))
            {
                laserLine.SetPosition(1, hitInfo.point);
                if (hitInfo.collider.gameObject.TryGetComponent(out SCR_Health playerHealth)){

                    
                    playerHealth.TakeDamage(10f);

                }
            }
            else
            {
                laserLine.SetPosition(1, laserOrigin.position + laserOrigin.forward * 50f);
            }
            
        }
        else
        {
            laserLine.enabled = false;
        }
        */
    }

    public void ActivateLaser()
    {
        laserLine.enabled = true;        
    }

    IEnumerator followPlayer()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Turret paused");

        playerLocation = player.transform.position;

        if (Vector3.Distance(playerLocation, turretLocation) < 5.0f)
        {
            Debug.Log("Player in range");

            ActivateLaser();

            pivotPoint.transform.LookAt(playerLocation);

            laserLine.SetPosition(0, laserOrigin.position);

            Ray hit = new Ray(laserOrigin.position, laserOrigin.forward);
            if (Physics.Raycast(hit, out RaycastHit hitInfo))
            {
                laserLine.SetPosition(1, hitInfo.point);
                if (hitInfo.collider.gameObject.TryGetComponent(out SCR_Health playerHealth))
                {


                    playerHealth.TakeDamage(0.5f);

                }
            }
            else
            {
                laserLine.SetPosition(1, laserOrigin.position + laserOrigin.forward * 50f);
            }

        }
        else
        {
            laserLine.enabled = false;
        }

        
    }
}
