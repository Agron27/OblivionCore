using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_LaserGunPickUp : MonoBehaviour, IInteractable
{
    
        [SerializeField ]public GameObject laserGun;
    [SerializeField] public bool bIsActive = false;
        

        public void Interact()
        {
            laserGun.SetActive(true);    
        bIsActive = true;
        }

    

}
