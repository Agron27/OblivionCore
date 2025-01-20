using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SCR_Interact;

public class PuzzleRoom3 : MonoBehaviour, IInteractable_Shoot
{
    [SerializeField] int pairNumber; // Each symbol has a pair number
    [SerializeField] Material originalMaterial;
    [SerializeField] Material newMaterial; // Material when correctly hit
    [SerializeField] Material failMaterial; // Material when the wrong symbol is hit

    private Renderer rend;

    private static int currentPair = -1; // Tracks the current active pair
    private static int completedPairs = 0; // Number of completed pairs

    private bool puzzleCompleted = false;

    private static HashSet<int> completedPairNumbers = new HashSet<int>(); // Track completed pairs

    [SerializeField] SCR_PuzzleLaser laserCube1;
    [SerializeField] SCR_PuzzleLaser laserCube2;
    [SerializeField] SCR_PuzzleLaser laserCube3;

    [SerializeField] SCR_WallExplosion WallExplosion;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void Interact()
    {
        if (puzzleCompleted)
        {          
            return;
        }

        if (completedPairNumbers.Contains(pairNumber))
        {
            Debug.Log("This pair is already completed.");
            return;
        }

        if (currentPair == -1)
        {
            // First symbol in the pair is hit
            currentPair = pairNumber;
            rend.material = newMaterial;
            Debug.Log("First symbol of pair " + pairNumber + " hit.");
        }
        else if (currentPair == pairNumber)
        {
            // Correct pair completed
            rend.material = newMaterial;
            completedPairs++;
            completedPairNumbers.Add(pairNumber);
            Debug.Log("Pair " + pairNumber + " completed. Completed pairs: " + completedPairs);

            if (completedPairs == 4)
            {
                Debug.Log("Puzzle completed successfully!");
                OnPuzzleComplete();
            }

            currentPair = -1; // Reset for the next pair
        }
        else
        {
            // Wrong symbol hit
            Debug.Log("Wrong symbol hit! Puzzle failed.");
            StartCoroutine(OnPuzzleFail());
        }
    }

    private IEnumerator OnPuzzleFail()
    {
        
        rend.material = failMaterial;
        Debug.Log("Puzzle failed. Resetting...");

        // Wait for 2 seconds to show failure
        yield return new WaitForSeconds(2f);

        ResetPuzzle();
    }

    private void ResetPuzzle()
    {
        // Reset everything
        foreach (PuzzleRoom3 obj in FindObjectsOfType<PuzzleRoom3>())
        {
            obj.rend.material = obj.originalMaterial;
        }

        currentPair = -1;       
        completedPairs = 0;
        completedPairNumbers.Clear();

        Debug.Log("Puzzle reset.");
    }

    private void OnPuzzleComplete()
    {
        WallExplosion.TriggerExplosion();
        puzzleCompleted = true;
    }

    private void ActivateLasers()
    {
        laserCube1.ActivateLaser();
        laserCube2.ActivateLaser();
        laserCube3.ActivateLaser();
    }
}
