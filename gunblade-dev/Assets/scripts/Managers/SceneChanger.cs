using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private string sceneName;
    [SerializeField] private bool trigger;

    // Update is called once per frame
    void Update()
    {
        if(!trigger)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(trigger)
        {
            if (collision != null)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    SceneManager.LoadScene(sceneName);
                }
            }
        }
    }
}
