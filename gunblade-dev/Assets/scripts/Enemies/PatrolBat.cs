using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBat : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    //Dano do inimigo
    [Header("Enemy Damage")]
    [SerializeField] private int patrolEnemyDamage;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    void Start()
    {
    }

    private void Awake()
    {
        initScale = enemy.localScale;
    }
    private void OnDisable()
    {
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    private void DirectionChange()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;

        //Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
    //Adaptei do codigo de DeadZone
    private void OnTriggerEnter2D (Collider2D collider)
    {

        if (collider.gameObject.CompareTag("Player"))
        {   
            collider.gameObject.GetComponent<PlayerCombat>().TakeDamage(patrolEnemyDamage, false);
            StartCoroutine(Attacked());       
        }  
    }
    IEnumerator Attacked()
    {
        yield return new WaitForSeconds(2f);
    }
}