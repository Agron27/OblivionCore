using UnityEngine;
using static SCR_Interact;

public class SCR_PuzzleRoom1 : MonoBehaviour, IInteractable_Shoot
{

    [SerializeField] SCR_WallExplosion WallExplosion;

    public void Interact()
    {
        WallExplosion.TriggerExplosion();
    }
}
