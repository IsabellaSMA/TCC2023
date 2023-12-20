using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneHandler : MonoBehaviour
{
    private GameManagerScript gameManagerScript;

    [SerializeField] private int deadZoneDamage;
    
    void Start()
    {
        gameManagerScript = GameObject.FindObjectOfType<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.transform.position = gameManagerScript.FallPoint;
                
                collision.gameObject.GetComponent<PlayerCombat>().TakeDamage(deadZoneDamage, false);
            }
        }
    }
}
