using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameOverPanel gameOverPanel;
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private PausePanel pausePanel;

    private PlayerController playerController;
    private GameManagerScript gameManagerScript;

    private void Awake()
    {

    }

    void Start()
    {
        gameManagerScript = GameObject.FindObjectOfType<GameManagerScript>();
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Escape) && !playerController.playerCombat.IsDead )
        {
            if (Time.timeScale > 0) Time.timeScale = 0;
            else Time.timeScale = 1;
            pausePanel.gameObject.SetActive(!pausePanel.gameObject.activeSelf);
        }


        if (playerController.playerCombat.IsDead)
        {
            if (pausePanel.gameObject.activeSelf) pausePanel.gameObject.SetActive(false);
            
            if (Time.timeScale > 0) Time.timeScale = 0; 
            gameOverPanel.gameObject.SetActive(true);
        }

        else if (!playerController.playerCombat.IsDead && gameOverPanel.gameObject.activeSelf)
        {
            gameOverPanel.gameObject.SetActive(false);
        }
    }
}
