using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SCR_CrystalsGravity : MonoBehaviour
{
    private ConstantForce constantForce;   

    List<GameObject> crystalPieces = new List<GameObject>();
    List<GameObject> piecesToRemove = new List<GameObject>();
    // Start is called before the first frame update

    private void Awake()
    {      
        for (int i = 0; i < transform.childCount; i++)
        {
            crystalPieces.Add(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {               

        foreach (GameObject piece in crystalPieces)
        {
            if (piece.transform.position.y < 0)
            {
                piecesToRemove.Add(piece);
            } else if (piece != null)
            {
                constantForce = piece.GetComponent<ConstantForce>();
                                            
                constantForce.force = new Vector3(Random.Range(-5f, 5f), Random.Range(-4f, 6f), Random.Range(-5f, 5f));
                                
            }

        }

        foreach (GameObject piece in piecesToRemove)
        {
            crystalPieces.Remove(piece);
            Destroy(piece);
        }
        
    }


}
