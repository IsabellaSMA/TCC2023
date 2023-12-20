using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private PlayerController playerController;

    [Header("Clips")]
    [SerializeField] private AudioClip[] attack;
    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip steps;

    [Header("State")]
    private bool isRunning;
    private bool isJumping;
    private bool fallTrigger;
    private bool jumPlayed;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool isAttacking;
    private bool attackPlayed;
    private int attackIndex;
    private bool shotPlayed;
    private bool isTakingDamage;
    private bool isShooting;
    private bool isDying;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }


    void Update()
    {
        UpdateStates();
        
        if(isAttacking)
        {
            if(!attackPlayed)
            {
                audioSource.PlayOneShot(attack[1]);
                attackPlayed = true;
            }
        }

        else
        {
            attackPlayed = false;
        }

        if (isShooting)
        {
            if (!shotPlayed)
            {
                audioSource.PlayOneShot(shot);
                shotPlayed = true;
            }
        }

        else
        {
            shotPlayed = false;
        }

        if (isJumping)
        {
            if (!jumPlayed)
            {
                audioSource.PlayOneShot(jump);
                jumPlayed = true;
            }
        }

        else
        {
            jumPlayed = false;
        }

        // Se o jogador está correndo e não está pulando, toque o som dos passos
        if (isRunning && !isJumping)
        {
            if (!audioSource.isPlaying)
            {
                //audioSource.clip = steps;
               // audioSource.Play();
            }
        }
        else
        {
            // Se o jogador parou de correr, pare o som dos passos
            //audioSource.Stop();
        }

        // Se o jogador pulou, toque o som do pulo

    }

    private void UpdateStates()
    {
        // movimento

        isRunning = playerController.playerMovement.IsRunning;
        isJumping = playerController.playerMovement.IsJumping;
        isWallJumping = playerController.playerMovement.IsWallJumping;
        isWallSliding = playerController.playerMovement.IsWallSliding;
        fallTrigger = playerController.playerMovement.FallTrigger;
        jumPlayed = playerController.playerMovement.JumpTrigger;

        // ataque

        isAttacking = playerController.playerCombat.IsAttacking;
        isShooting = playerController.playerCombat.IsShooting;
    }
}