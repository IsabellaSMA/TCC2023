using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    [Header("Enemy Stats")]
    [SerializeField] protected float maxHealth;
    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private int coinAmount;
    [SerializeField] public float currentHealth;
    public bool isDead = false;
    protected int facingDirection;
    private Color originalColor;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject explosionParticle;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        originalColor = sr.color;
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        if (transform.localScale.x > 0) facingDirection = 1;
        else if (transform.localScale.x < 0) facingDirection = -1;

        if(currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    public void Die()
    {
        GameObject blood = Instantiate(explosionParticle, transform.position, Quaternion.identity);

        for (int i = 0; i < coinAmount; i++)
        {
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coin.GetComponent<Rigidbody2D>().AddForce(direction * 10, ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }

    public void TakeDamage(float damage, bool knockBack)
    {
        currentHealth -= damage;
        StartCoroutine(Flash());
        if (knockBack) StartCoroutine(KnockBack());
    }

    IEnumerator Flash()
    {
        Color flashColor = new Color(255, 0, 0);

        sr.color = flashColor;
        yield return new WaitForSeconds(.1f);
        sr.color = originalColor;
        yield return new WaitForSeconds(.1f);
        sr.color = flashColor;
        yield return new WaitForSeconds(.1f);
        sr.color = originalColor;
        yield return new WaitForSeconds(.1f);
        sr.color = flashColor;
        yield return new WaitForSeconds(.1f);
        sr.color = originalColor;
    }

    IEnumerator KnockBack()
    {
        rb.AddForce(new Vector2(-facingDirection, 1) * knockBackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockBackDuration);
    }
}
