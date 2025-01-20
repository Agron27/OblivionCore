using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SCR_Interact;

public class SCR_PuzzleRoom2 : MonoBehaviour, IInteractable_Shoot
{
    //Identifies the order to hit the symbol object
    [SerializeField] int symbolIdentifier;

    //The object that needs to be hit next
    public static int currentSymbol = 0;

    
    private float numOfSymbols = 5;

    bool isPuzzleComplete;

    [SerializeField] Material originalMaterial;
    [SerializeField] Material newMaterial;
    [SerializeField] Material resetMaterial;
    private Renderer rend;

    [SerializeField] SCR_WallExplosion WallExplosion;   
    

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void Interact()
    {
        if (isPuzzleComplete) return;

        if (rend != null)
        {
            Debug.Log("Called Interact");
            if (currentSymbol == symbolIdentifier) 
            {
                Debug.Log("Symbol " + currentSymbol);
                rend.material = newMaterial;

                Mathf.Max(numOfSymbols, currentSymbol++);
            }
            else
            {
                if (currentSymbol < symbolIdentifier)
                {
                    StartCoroutine(PuzzleMaterialReset());
                }
            }
        }

        // Puzzle Completion
        if (currentSymbol == numOfSymbols)
        {           
            WallExplosion.TriggerExplosion();
            isPuzzleComplete = true;
        }
    }

    IEnumerator PuzzleMaterialReset()
    {

        foreach (SCR_PuzzleRoom2 puzzleObject in FindObjectsOfType<SCR_PuzzleRoom2>())
        {
            puzzleObject.rend.material = resetMaterial;
        }
        yield return new WaitForSeconds(2f);

        foreach (SCR_PuzzleRoom2 puzzleObject in FindObjectsOfType<SCR_PuzzleRoom2>())
        {
            puzzleObject.rend.material = originalMaterial;
        }
        currentSymbol = 0;
    }
}
