using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("BATUE");

        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindObjectOfType<GameManagerScript>().Balance += coinValue;
            Destroy(gameObject);
        }
    }
}
