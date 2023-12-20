using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follow : MonoBehaviour {

        [SerializeField] private PlayerController playerController;
        
        
        
        private GameObject wayPoint;
        private Vector3 wayPointPos;
		[SerializeField] private float Distance;
		[SerializeField] private float atkDist = 3;
        
        [SerializeField] private float speed = 5.0f;

        [SerializeField] private int  followEnemyDamage = 10;

        [SerializeField] private bool PlayerAttacked = false;
        [SerializeField] private float timerToAttack = 3f; //Random Timer.
        [SerializeField] private float time = 1f;
        private bool canAttack = false;


        [SerializeField] private GameObject player;
        public bool flip;
        public float speedFlip;
        
        


        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            //At the start of the game, the zombies will find the gameobject called wayPoint.
            wayPoint = GameObject.Find("wayPoint");
        }
    
        void Update()
        {
            Vector3 scale = transform.localScale;

            if(player.transform.position.x > transform.position.x)
            {
                scale.x = Mathf.Abs(scale.x) * -1 * (flip ? -1 : 1);
                transform.Translate(x: speedFlip * Time.deltaTime, y:0, z:0);
            }
            else 
            {
                scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
                transform.Translate(x: speed * Time.deltaTime * -1, y:0, z:0);
            }

            transform.localScale = scale;

            float plyDist = Vector2.Distance(transform.position, playerController.transform.position);
            
           
            if(plyDist < Distance && plyDist > atkDist)
            {

                transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position, speed * Time.deltaTime);
                
            }

        }
    

		void OnDrawGizmosSelected()
        {
        
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, Distance);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, atkDist);
        }
        
        
        void OnTriggerEnter2D (Collider2D coll)
        
        {
                if(coll.gameObject.CompareTag("Player"))
                {
                    playerController.GetComponent<PlayerCombat>().TakeDamage(followEnemyDamage,false);
                    PlayerAttacked = true;
                }
        }

         private void TimerToAttack()
        {
            if(PlayerAttacked)
            {
                if (timerToAttack > 0)
                {
                    canAttack = false;
                    timerToAttack -= time * Time.deltaTime;
                }
                    
                else
                {
                    canAttack = true;
                    timerToAttack = 3f;
                }
            }
        }


    //Chamar as Animacoes do Inimigo
}