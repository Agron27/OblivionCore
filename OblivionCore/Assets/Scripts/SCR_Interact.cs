using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_Interact : MonoBehaviour
{
    private PlayerControls playerControls;
    private InputAction interactAction;

    public Transform interactSource;
    public float interactRange = 2f;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        interactAction = playerControls.Player.Interact;
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    void Update()
    {
        if (interactAction.triggered)
        {
            Ray ray = new Ray(interactSource.position, interactSource.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObject))
                {
                    interactObject.Interact();
                }
            }
        }
    }
}
    public interface IInteractable
    {
        void Interact();
    }

    public interface IInteractable_Shoot
    {
        void Interact();
    }

