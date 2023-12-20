using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Rendering.Universal;

public class PlayerCombat : MonoBehaviour
{
    [Header("Components")]
    private PlayerController playerController;
    private Rigidbody2D playerRB;
    private CapsuleCollider2D playerCol;
    private SpriteRenderer playerSR;
    public bool debug;

    [Header("Hero Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool isDead = false;
    private int facingDirection;

    [Header("Gun")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float recoilForce;
    [SerializeField] private int ammo = 2;
    [SerializeField] float shotCD;
    [SerializeField] float dashDuration;
    [SerializeField] private float reloadTime;
    private bool isShooting = false;
    private bool canShoot = true;
    private Vector2 firePoint;
    private Vector2 fireDirection;
    private float rotation = 0;
    public bool shotTrigger;


    [Header("Blade")]
    [SerializeField] private Vector2 attackPoint;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] float attackDuration;
    [SerializeField] float attackCD;
    private bool isAttacking = false;
    private bool canAttack = true;
   
    public bool attackTrigger;

    [HideInInspector] public int Ammo { get { return ammo; } }
    [HideInInspector] public int Health { get { return currentHealth; } }
    [HideInInspector] public bool IsDead { get { return isDead; } }
    [HideInInspector] public bool IsAttacking { get { return isAttacking; } }
    [HideInInspector] public bool IsShooting { get { return isShooting; } }
    [HideInInspector] public bool CanShoot { get { return canShoot; } }
    [HideInInspector] public float ReloadTime { get { return reloadTime; } }
    [HideInInspector] public float ShotCD { get { return shotCD; } }
    [HideInInspector] public float AttackCD { get { return attackCD; } }
    [HideInInspector] public float Rotation { get { return rotation; } }



    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerRB = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CapsuleCollider2D>();
        playerSR = GetComponent<SpriteRenderer>();
        

        currentHealth = maxHealth;

        firePoint = new Vector2(1, 0);
    }

    void Update()
    {
        if (debug) ShowDebug();

        if (ammo > 2) ammo = 2;

        if(transform.localScale.x > 0) facingDirection = 1;
        else if (transform.localScale.x < 0) facingDirection= -1;


        firePoint = new Vector2(transform.position.x, transform.position.y) + fireDirection * 4;

        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }

    public void CombatUpdate()
    {
        // Pra testar a vida do bixo se ele ta morrendo e afins.

        //if (Input.GetKeyDown(KeyCode.K)) TakeDamage(currentHealth, false);
        //if (Input.GetKeyDown(KeyCode.L)) TakeDamage(10, true);

        // Checa inputs pra setar o firePoint

        if (Input.GetKeyDown(KeyCode.W))
        {
            rotation = 90;
            fireDirection = new Vector2(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            rotation = 180;
            fireDirection = new Vector2(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            rotation = 270;
            fireDirection = new Vector2(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            rotation = 0;
            fireDirection = new Vector2(1, 0);
        }

        // Checa inputs e condicoes pra performar ataque e disparo

        if (Input.GetKeyDown(KeyCode.J))
        {
            if(canAttack)
            {
                attackTrigger = true;
                StartCoroutine(Attack());
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if(canShoot && ammo > 0)
            {
                StartCoroutine(ShootDash());
 
            }
        }
    }

    // transformei o ataque em uma corrotina, pra gerenciar melhor o tempo de ataque e cooldown, pra facilitar a vida nas animações.
    IEnumerator Attack()
    {
        isAttacking = true;
        canAttack = false;
        

        Vector2 translatedAttackPoint = new Vector2(transform.position.x, transform.position.y) + attackPoint * facingDirection;
        DrawCircle(translatedAttackPoint, attackRange, Color.red);

        isAttacking = true;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(translatedAttackPoint, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Enemy>() != null)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage, true);
            }
        }
        //Debug.Log("Atacou " + hitEnemies.Length + " inimigos");
        

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;

        yield return new WaitForSeconds(attackCD);

        canAttack = true;
    }

    // mais ou menos o que fiz com o ataque só que no dash.

    IEnumerator ShootDash()
    {
        isShooting = true;
        canShoot = false;
        shotTrigger = true;
        playerController.playerMovement.CanMove = false;

        ammo--;
        StartCoroutine(AmmoCD());

        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint, Quaternion.Euler(0, 0, rotation));

        playerRB.velocity = playerRB.velocity + -fireDirection * recoilForce;

        yield return new WaitForSeconds(dashDuration);

        playerController.playerMovement.CanMove = true;
        isShooting = false;

        yield return new WaitForSeconds(shotCD);

        canShoot = true;
    }

    // adiciona uma munição quando passar o reloadtime
    IEnumerator AmmoCD ()
    {
        yield return new WaitForSeconds(reloadTime);
        ammo++;
    }

    void ShowDebug() // pra quando algo der errado e vc precisar de confirmação visual.
    {
        DrawCircle(transform.position, 4, Color.cyan); // firepoint circle
        Vector2 translatedAttackPoint = new Vector2(transform.position.x, transform.position.y) + attackPoint * facingDirection;
        DrawCircle(translatedAttackPoint, attackRange, Color.red); // attackpoint
        DrawCircle(firePoint, attackRange, Color.green); // firepoint

        if(isAttacking)
        {
            Debug.Log("Deveria ter atacado");
        }
        if(IsShooting)
        {
            Debug.Log("Deveria ter atirado");
        }
    }

    public void TakeDamage(int damage, bool knockBack)
    {
        currentHealth -= damage;
        StartCoroutine(Flash());

        if (knockBack) StartCoroutine(KnockBack());
    }

    IEnumerator Flash()
    {
        Color originalColor = playerSR.color;
        Color flashColor = new Color(255, 0, 0);

        playerSR.color = flashColor;
        yield return new WaitForSeconds(.1f);
        playerSR.color = originalColor;
        yield return new WaitForSeconds(.1f);
        playerSR.color = flashColor;
        yield return new WaitForSeconds(.1f);
        playerSR.color = originalColor;
    }

    IEnumerator KnockBack()
    {
        playerController.playerMovement.CanMove = false;
        playerRB.AddForce(new Vector2(-facingDirection, 1) * knockBackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockBackDuration);
        playerController.playerMovement.CanMove = true;
    }

    void DrawCircle(Vector3 position, float radius, Color color)
    {
        int segments = 360;
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            Vector3 point1 = new Vector3(x, y, 0) + position;
            angle += 360f / segments;
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            Vector3 point2 = new Vector3(x, y, 0) + position;
            Debug.DrawLine(point1, point2, color);
        }
    }

    public void SetHealth(int health)
    {
        currentHealth = health;
    }

    public void Heal(int health)
    {
        currentHealth += health;
    }
}