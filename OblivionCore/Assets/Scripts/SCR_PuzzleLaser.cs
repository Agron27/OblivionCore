using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PuzzleLaser : MonoBehaviour
{
    [SerializeField] LineRenderer laserLine;
    [SerializeField] Transform laserOrigin;
    
    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
    }

    public void ActivateLaser()
    {
        laserLine.enabled = true;
        laserLine.SetPosition(1, laserOrigin.position + laserOrigin.transform.up);
    }
}
