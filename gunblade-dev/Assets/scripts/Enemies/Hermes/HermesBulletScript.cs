using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermesBulletScript : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private GameObject player;
    private Rigidbody2D rb;
    public float force;
    private float timer;
    [SerializeField] private int DeggerDamage = 10;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, rot + 180, rot - 90);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.gameObject.CompareTag("Player")) 
        {
            playerController.GetComponent<PlayerCombat>().TakeDamage(DeggerDamage, false);
            Destroy(gameObject);    
        }
    }
}
