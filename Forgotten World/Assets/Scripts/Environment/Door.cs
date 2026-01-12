using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Door : MonoBehaviour
{
    [SerializeField] private Camera _playerCam;
    [SerializeField] private float _interactionDistance = 5f;
    public TextMeshProUGUI _interactionText;

    private void Interact()
    {
        Debug.Log("Load The next Scene!");
    }

    private string GetInteractionPrompt()
    {
        return _interactionText.text;
    }

    private void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.F)) // If the player presses F...
        {
            TryInteract();
        }
    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(_playerCam.transform.position, _playerCam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
          //      _interactionText.text = interactable.GetInteractionPrompt();
                _interactionText.enabled = true;
            }
            else
            {
                _interactionText.enabled = false;
            }
        }
        else
        {
            _interactionText.enabled = false;
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(_playerCam.transform.position, _playerCam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set color for visibility
        Gizmos.DrawWireSphere(transform.position, _interactionDistance);
    }
}
