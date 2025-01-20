using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCR_Hints : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText = null;
    [SerializeField] private string hintMessage;
    private bool bHasLaser = false;

    void Update()
    {
        if (GameObject.Find("BackPack").GetComponent<SCR_LaserGunPickUp>().bIsActive)
        {
            if (!bHasLaser)
            {
                hintMessage = "What's that symbol on the wall?";
                StartCoroutine(Message());
            }
            bHasLaser = true;
            StopCoroutine(Message());

        }
    }

    void OnTriggerEnter()
    {
        StartCoroutine(Message());
    }

    private IEnumerator Message()
    {
        hintText.gameObject.SetActive(true);
        hintText.text = "<#373737> You: </color> <#FFFFFF>" + hintMessage;
        yield return new WaitForSeconds(2f);
        hintText.gameObject.SetActive(false);
    }
}
