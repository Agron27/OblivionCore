using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_GameManager : MonoBehaviour
{
    //Identifies the order to hit the symbol object
    [SerializeField] int symbolIdentifier;
    //The object that needs to be hit next
    private int currentSymbol = 0;
    //The object that was hit last
    private int priorSymbol = 0;
    [SerializeField] Material originalMaterial;
    [SerializeField] Material newMaterial;
    private Renderer rend;
    private GameObject puzzleSymbol;

    void Start()
    {
        puzzleSymbol = GameObject.FindWithTag("Symbol");
        rend = puzzleSymbol.GetComponent<Renderer>();
   }

    void OnTriggerEnter(Collider other)
    {
        
    }

   public void puzzleCompletion()
    {
       // if (gameObject.CompareTag("Player"))
       // {
           // if (rend != null)
            //{
                if (symbolIdentifier == currentSymbol && (priorSymbol == currentSymbol--))
                {
                    rend.material = newMaterial;
                    priorSymbol = symbolIdentifier;
                    currentSymbol++;
                }
                else if (symbolIdentifier != currentSymbol || priorSymbol != currentSymbol--)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        currentSymbol = i;
                        rend.material = originalMaterial;
                    }
                    currentSymbol = 0;
                }
            //}
       // }
    }
}
