using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SCR_Interact;
public class SCR_RemoteSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject on;
    [SerializeField] GameObject off;
    [SerializeField] Light light_PuzzleRoom_1;

    [SerializeField] Material purpleMaterial;
    [SerializeField] Material originalMaterial;


    bool isOff = true;
    public void Interact()
    {
        Debug.Log("INTERACTED");
        if (isOff)
        {
            on.SetActive(true);
            off.SetActive(false);
            light_PuzzleRoom_1.color = new Color(0.3800626f, 0f, 1f, 1f);

            MaterialOn();
            
        } else
        {
            on.SetActive(false);
            off.SetActive(true);

            MaterialOff();
        }

        isOff = !isOff;
    }


    void MaterialOn()
    {
        foreach (GameObject symbol in GameObject.FindGameObjectsWithTag("Symbol"))
        {
            symbol.GetComponent<Renderer>().material = purpleMaterial;
        }
    }

    void MaterialOff()
    {
        foreach (GameObject symbol in GameObject.FindGameObjectsWithTag("Symbol"))
        {
            symbol.GetComponent<Renderer>().material = originalMaterial;
        }
    }

}
