using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Generator : MonoBehaviour, IInteractable_Shoot
{

    //Interact with generator
    //Fill a gauge
    //Gen(n) complete

    private int gaugeCurrent = 0;
    private int gaugeMax = 100;
    private int gaugeFillAmount = 10;
    [SerializeField] Transform gaugePosition;
    [SerializeField] SCR_EnemyTurret turretScript;
    public static int numOfGeneratorsPowered = 0;
    [SerializeField] bool bGeneratorsFilled = false;
    [SerializeField] bool bCanFill = true;
    [SerializeField] GameObject gauge;

    private float xValue = 0f;
    private float yValue = 0f;
    // Start is called before the first frame update
    void Start()
    {
        gaugePosition = gauge.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (numOfGeneratorsPowered == 2)
        {
            bGeneratorsFilled = true;
            //spawn enemies when back in main room - separate script maybe
            turretScript.bIsActive = true;
        }
    }

    public void Interact()
    {
        if (bCanFill)
        {
            gaugeCurrent = gaugeCurrent + gaugeFillAmount;
            Debug.Log("Gauge hit");
            //move or stretch gauge to fill up
            //Change gaugePosition to reflect new position so it can continue to move upwards
            gaugePosition.position = new Vector3(gauge.transform.position.x, (yValue+0.2f), gauge.transform.position.z);
            if (gaugeCurrent == gaugeMax / 2)
            {
                bCanFill = false;
                //wait for 2 enemies to die
            }

            if (gaugeCurrent == gaugeMax)
            {
                numOfGeneratorsPowered++;
            }
        }
        
    }
}
