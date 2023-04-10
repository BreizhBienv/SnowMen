using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    [SerializeField] GameObject _interactButton;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _interactButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _interactButton.SetActive(false);
    }
}
