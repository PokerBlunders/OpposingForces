using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    public GameObject interactableObject;
    public GameObject playerObjectCopy;
    public GameObject interactionText;
    public GameObject exit;

    private bool canInteract = false;

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.Space))
        {
            PerformInteraction();
        }
    }

    void PerformInteraction()
    {
        interactableObject.SetActive(false);
        playerObjectCopy.SetActive(true);
        exit.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            canInteract = true;
            interactionText.SetActive(true);
        }

        if (other.CompareTag("Exit"))
        {
            Debug.Log("Game Done");
            SceneManager.LoadScene("Win");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            canInteract = false;
            interactionText.SetActive(false);
        }
    }
}
