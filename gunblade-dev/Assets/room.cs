using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour
{
    public GameObject VirtualCam;
        
    private void OntriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            VirtualCam.SetActive(true);

        }
    }
    private void OntriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            VirtualCam.SetActive(false);

        }
    }
}
