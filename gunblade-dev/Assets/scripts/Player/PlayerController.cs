using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{

    //componentes
    [Header("Components")]
    private GameManagerScript gameManager;

    [Header("Player")]
    public PlayerCombat playerCombat;
    public PlayerMovement playerMovement;

    private void Awake()
    {
        int NumberOfCoins = PlayerPrefs.GetInt("NumberOfCoins", 0);

        playerCombat = GetComponent<PlayerCombat>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {

    }

    // melhorei a comunicação entre os scripts, TUDO fala com o playercontroller, que fala com cada um dos playerpenis. assim evitando uma teia de aranha terrivel nossa que sono.

    void Update()
    {
        if (!playerCombat.IsDead)
        {
            playerCombat.CombatUpdate();
            playerMovement.MovementUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (!playerCombat.IsDead && playerMovement.CanMove)
        {
            playerMovement.MovementFixedUpdate();
        }
    }
}