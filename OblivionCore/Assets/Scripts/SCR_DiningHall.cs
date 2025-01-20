using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DiningHall : MonoBehaviour, IInteractable_Shoot { 

    //[SerializeField] bool bAllFrozenOver = false;
    [SerializeField] GameObject water;
    [SerializeField] private float movementTime;
    [SerializeField] private Transform target;
    [SerializeField] private int numberOfPipesToFreeze;
    public static int numberOfPipesFrozen;
    [SerializeField] GameObject terminal;
    [SerializeField] Material terminalNewMaterial;
    private Renderer rend;
    [SerializeField] SCR_Shoot shootScript;
    [SerializeField] GameObject laser;

    // Start is called before the first frame update
    void Start()
    {
        shootScript = laser.GetComponent<SCR_Shoot>();
        if (shootScript.currentMode == LaserMode.STASIS)
        {
            print("Stasis active");
        }else if (shootScript.currentMode == LaserMode.PLASMA)
        {
            print("Plasma active");
        }
        rend = terminal.GetComponent<Renderer>();
        numberOfPipesToFreeze = 1;
        numberOfPipesFrozen = 1;
        PuzzleComplete();
    }

    void PuzzleComplete()
    {
        if (numberOfPipesFrozen == numberOfPipesToFreeze)    
        {
            print("All frozen");
            StartCoroutine(DrainWater(transform, target.position, movementTime));
        }

    }

    public void Interact()
    {
        if (shootScript.currentMode == LaserMode.STASIS)
        {
            numberOfPipesFrozen++;
            if (numberOfPipesFrozen == numberOfPipesToFreeze)
            {
                //bAllFrozenOver = true;
                PuzzleComplete();
            }
        }
        
    }


    //Moves the water object underneath the level to give the effect of draining away
    private IEnumerator DrainWater(Transform transform, Vector3 position, float timeToMove)
    {
        print("Coroutine started");
        var currentPos = water.transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            water.transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }

        //Once water is "drained", swap the terminal material to one displaying the correct keypad number
        if (water.transform.position == position)
        {
            rend.material = terminalNewMaterial;
        }
    }


}

