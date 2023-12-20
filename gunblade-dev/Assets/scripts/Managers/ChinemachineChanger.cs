 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChinemachineChanger : MonoBehaviour
{
    //Objeto para transição

    [SerializeField] private GameObject TransitionPoint;   
    
    // Ajustes para collisão e Transição
    [SerializeField] private float timer;
    
    [SerializeField] private bool trigger;
    //Partes da Camera
    private bool PlayerCamera = true;

    [SerializeField] private CinemachineVirtualCamera playerCam;

    [SerializeField] private CinemachineVirtualCamera CorridorCam;

    [SerializeField] private CinemachineVirtualCamera bossCam;


    
    void Start()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchPriority();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!trigger)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SwitchPriority();
            }
        }
        
    }
    private void SwitchPriority()
    {
      if(PlayerCamera)
      {
        playerCam.Priority = 0;
        bossCam.Priority = 1;

      }
      else 
      {
        playerCam.Priority = 1;
        bossCam.Priority = 0;
      } 
      PlayerCamera = !PlayerCamera;
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(trigger)
        {
            if (collision != null)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    SwitchPriority();
                }
            }
        }
    }
}
