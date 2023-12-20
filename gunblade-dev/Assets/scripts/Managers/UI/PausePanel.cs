using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public bool IsPauseOn = false;

    public bool IsMenuOpen = false;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject OptionsMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsMenuOpen == false)
        {
            if(IsPauseOn)
            {
                Resume();    
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPauseOn = false;

    }

    public void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPauseOn = true;
    }

    public void OpenOptions()
    {
        IsMenuOpen = true;
        OptionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);    
    }

    public void Back()
    {
        IsMenuOpen = false;

        if(IsMenuOpen == false)
        {
                pauseMenuUI.SetActive(true); 
                OptionsMenuUI.SetActive(false);                 
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
