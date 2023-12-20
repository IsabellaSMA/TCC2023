using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D bulletRB;
    private CapsuleCollider2D bulletCol;

    [Header("Stats")]
    [SerializeField] private float explosionDamage;
    [SerializeField] private float explosionRange;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifetime;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] GameObject explosionParticle;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody2D>(); 
        bulletCol = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        Destroy(gameObject, 5f);
        bulletRB.velocity = transform.right * bulletSpeed;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag != "Ignore")
            {
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRange, enemyLayer);
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(explosionDamage, true);
                }
                //Debug.Log("Pegou " + hitEnemies.Length + " inimigos");

                Instantiate(explosionParticle, transform.position, Quaternion.identity);

                //Debug.Log("bateu");
                DestroyBullet();
            }
        }
    }   

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
