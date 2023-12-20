using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPoint : MonoBehaviour
{
    private GameManagerScript gameManagerScript;
    
    void Start()
    {
        gameManagerScript = GameObject.FindObjectOfType<GameManagerScript>();
        GetComponent<SpriteRenderer>().enabled = false;
        
        
    }

    void Update()
    {
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Player")
            {
                gameManagerScript.FallPoint = transform.position;
                
                
            }
        }
    }
    
}
