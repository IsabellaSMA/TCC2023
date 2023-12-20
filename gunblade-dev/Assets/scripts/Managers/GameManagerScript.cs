using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private PlayerController playerController;
    //private PlayerCombat playerCombat;

    [Header("Stats")]
    private int balance = 0;
    private int retryCost = 10;

    public int Balance { get { return balance; } set { if (value <= 0) balance = 0; else balance = value; } }
    public int RetryCost { get { return retryCost; } set { if (value > retryCost) retryCost = value; else if (retryCost <= 0) retryCost = 0; } }

    private Vector2 fallPoint = new Vector2(0, 0);
    public Vector2 FallPoint { get { return fallPoint; } set { fallPoint = value; } }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu") Destroy(gameObject);
        else
        {
            if (Time.timeScale <= 0) Time.timeScale = 1;
            playerController = FindObjectOfType<PlayerController>();
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            Balance += 100;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LastScene(float time)
    {
        StartCoroutine(LastSceneCoroutine(time));
    }

    IEnumerator LastSceneCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Acabou");
    }
}