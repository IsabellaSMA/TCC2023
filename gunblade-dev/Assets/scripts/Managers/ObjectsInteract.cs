using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInteract : MonoBehaviour
{
    [SerializeField] private GameObject TextPlaca;

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            TextPlaca.SetActive(true);
        }
    }
    
    void OnTriggerExit2D(Collider2D coll)
    {
        TextPlaca.SetActive(false);
    }
}
