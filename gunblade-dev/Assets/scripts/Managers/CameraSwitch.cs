using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera camera1, camera2;

    void Start()
    {
        camera2.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Player")
            {
                camera2.gameObject.SetActive(true);
                camera2.depth = 10;
            }
        }
    }
}
