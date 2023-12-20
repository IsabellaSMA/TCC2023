using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanGo : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private Collider2D canGoCollider;

    [SerializeField] private GameObject background;


    void Start()
    {
        canGoCollider = GetComponent<Collider2D>();
        canGoCollider.isTrigger = true;   
    }

    void Update()
    {
        
    }


    void OnTriggerExit2D(Collider2D coll) 

    {
        if (coll.gameObject.CompareTag("Player"))
        {
            canGoCollider.isTrigger = false;
            Destroy(background);
        }
    }

}
