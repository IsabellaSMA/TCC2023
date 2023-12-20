using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // aqui é para tentar fazer uma mudança entre menu de opcoes e padrão
    [SerializeField] private bool optionsOpen = false;
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject menuCanvas;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        //if (UnityEditor.EditorApplication.isPlaying) UnityEditor.EditorApplication.isPlaying = false;
        //else
        Application.Quit();
    }

    // metodo para isso
    public void OpenOptions()
    {
        optionsOpen = true;
        
        if(optionsOpen == true)
        {
            menuCanvas.SetActive(false); 
            optionsCanvas.SetActive(true);   
        }
    }

    // esse metodo é simplesmente para fazer voltar para o menu principal
    public void Back()
    {
        optionsOpen = false;

        if(optionsOpen == false)
        {
                menuCanvas.SetActive(true); 
                optionsCanvas.SetActive(false);                 
        }
    }
}
