using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hermes : Enemy
{
    private GameObject player;

    [Header("Components")]
    private CapsuleCollider2D col;
    private Animator animator;
    [SerializeField] private Slider HealthBarSlider;
    GameManagerScript gameManagerScript;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletFirePos;
    [SerializeField] private float shootCD;
    [SerializeField] private float shootCdCounter;
    [SerializeField] private int shootAmount;
    private int shootCount;

    [Header("Dash")]
    //[SerializeField] private float dashAntecipation; // tempo para tocar a animacao de antecipacao do dash
    //[SerializeField] private float dashEnd; // tempo para tocar a animacao de fim do dash
    [SerializeField] private int dashDamage;
    [SerializeField] private float dashDuration; // tempo de dira��o do dash dash dashando e afins.
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCD;
    [SerializeField] private float dashCollisionCheckDistance;
    [SerializeField] LayerMask playerLayerMask;
    private int dashCount;

    [Header("Idle")]
    [SerializeField] private float idleDuration;
    private float idleTimeCount;

    [Header("States")]
    private string[] states = new string[] { "Shooting", "Idle", "Dashing" };
    private string currentState;
    private int currentStateIndex;

    [Header("Stats")]
    private bool dashing;
    private bool dashAntecipating;
    private bool dashEnding;
    private bool canDash;
    private bool dashInCD;
    private bool dashHit;
    private bool end = false;

    [Header("End Of Fight")]
    [SerializeField] public string nextSceneName;
    public GameObject bossObject;

    [Header("Animation")]
    private bool dashTriggered;
    private bool dashAntecipationTriggered;
    private bool dashEndTriggered;

    protected override void Awake()
    {
        base.Awake();
    }

    protected  override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        col = GetComponent<CapsuleCollider2D>();  
        animator = GetComponent<Animator>();
        gameManagerScript = GameObject.FindObjectOfType<GameManagerScript>();

        currentState = states[0];
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (currentHealth <= 0)
        {
            gameManagerScript.LastScene(2f); // se o hermes estiver morto, chama uma funcao no gamemanager que vai trocar pra cena final no tempo passado (coloquei 2s) pq ficava estranho o bgl cortando instantaneo.
        }

        base.Update();

        if (!canDash && !dashInCD)
        {
            StartCoroutine(DashCooldown());
        }

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= 40)
        {
            switch (currentState)
            {
                case "Idle":
                    bool doneIdle = IdleState();

                    if (doneIdle)
                    {
                        NextState();
                    }

                    break;

                case "Dashing":
                    bool doneDashing = DashingState();

                    if (doneDashing)
                    {
                        NextState();
                    }

                    break;

                case "Shooting":
                    bool doneShooting = ShootingState();

                    if (doneShooting)
                    {
                        NextState();

                    }

                    break;

                default:
                    break;
            }

            HealthBarHandler();
        }

        else if (HealthBarSlider.gameObject.activeSelf)
        {
            HealthBarSlider.gameObject.SetActive(false);
        }

        FlipHandler();
        //AnimationHandler();
    }

    void HealthBarHandler()
    {
        if (!HealthBarSlider.gameObject.activeSelf) HealthBarSlider.gameObject.SetActive(true);

        float minValue = HealthBarSlider.minValue;
        float maxValue = HealthBarSlider.maxValue;

        float value = currentHealth / maxHealth * maxValue;

        HealthBarSlider.value = value;
    }

    void NextState()
    {
        currentStateIndex = (currentStateIndex + 1) % states.Length;
        currentState = states[currentStateIndex];
    }

    bool IdleState() // Determina o estado do boss. quando retornar  "TRUE" � por que acabou de fazer o que tinha que fazer e pode passar para o proximo estado
    {
        idleTimeCount += Time.deltaTime;

        //animator.SetBool("Idle", true);

        if (idleTimeCount >= idleDuration)
        {
            //Debug.Log("Acabou idle");
            idleTimeCount = 0;
            return true;
        }

        else
        {
            //animator.SetBool("idle", false);
            return false;
        }
    }

    bool ShootingState ()
    {
        if (shootCount < shootAmount)
        {
            shootCdCounter += Time.deltaTime;

            if (shootCdCounter >= shootCD)
            {
                shootCount++;
                shootCdCounter = 0;

                animator.SetTrigger("shoot");

                Instantiate(bulletPrefab, bulletFirePos.position, Quaternion.identity);
            }

            return false;
        }

        else 
        {
            shootCount = 0;
            return true; 
        }
    }

    bool DashingState()
    {
        Debug.DrawRay(transform.position, transform.right * dashCollisionCheckDistance * facingDirection, Color.green);
        bool collide = Physics2D.Raycast(transform.position, transform.right * facingDirection, dashCollisionCheckDistance, playerLayerMask);

        if (collide && !dashHit)
        {
            dashHit = true;
            player.GetComponent<PlayerController>().playerCombat.TakeDamage(dashDamage, true);
        }

        if (dashCount < 2 && canDash)
        {
            dashCount++;
            animator.SetTrigger("dash");
            StartCoroutine(DashCoroutine());
            return false;
        }

        else if (dashCount >= 2 && !dashing)
        {
            //Debug.Log("deu 2 dash cabo");
            dashCount = 0;
            dashHit = false;
            return true;
        }

        else
        {
            return false;
        }
    }

    IEnumerator DashCoroutine()
    {
        canDash = false;
        dashing = true;

        dashAntecipating = true;

        //yield return new WaitForSeconds(dashAntecipation);

        dashAntecipating = false;

        rb.velocity = new Vector2(dashSpeed * facingDirection, rb.velocity.y);

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = new Vector2(0, rb.velocity.y);

        dashEnding = true;

        //yield return new WaitForSeconds(dashEnd);

        dashEnding = false;

        dashing = false;
    }

    IEnumerator DashCooldown()
    {
        dashInCD = true;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
        dashInCD = false;
    }

    void FlipHandler()
    {
        if (!dashing)
        {
            if (player.transform.position.x - transform.position.x > 0 && facingDirection == -1)
            {
                facingDirection = 1;
                transform.localScale = new Vector3(1, 1, 1);
            }

            else if (player.transform.position.x - transform.position.x < 0 && facingDirection == 1)
            {
                facingDirection = -1;
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
