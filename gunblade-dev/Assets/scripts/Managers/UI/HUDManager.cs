using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    //componentes
    [Header("Components")]
    private GameManagerScript gameManagerScript;
    private PlayerController playerController;

    //Lista de Cartas

    [Header("Hud Elements")]
    [SerializeField] private GameObject[] cardsIMG;
    [SerializeField] private GameObject[] damageCardsIMG;
    [SerializeField] private GameObject[] ammoIMG;
    [SerializeField] private Slider[] ammoSliders;
    //[SerializeField] private Slider reloadSlider;
    [SerializeField] private GameObject playerIMG;

    [SerializeField] private TextMeshProUGUI coinsText;

    private int coins;

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
        coins = gameManagerScript.Balance;

        if(!playerController.playerCombat.IsDead)
        {
            coinsText.text = coins.ToString();
            CardHandler();
            AmmoHandler();
        }
    }

    private void CardHandler()
    {
        int[] healthLimits = new int[] { 0, 10, 20, 30 };

        for (int i = 0; i < healthLimits.Length; i++)
        {
            if (playerController.playerCombat.Health <= healthLimits[i])
            {
                cardsIMG[i].SetActive(false);
                damageCardsIMG[i].SetActive(true);
                break;
            }
        }
    }

    private void AmmoHandler()
    {
        for (int i = 0; i < ammoIMG.Length; i++)
        {
            for (int j = 0; j < ammoIMG[i].transform.childCount; j++)
            {
                // Acessa o Image do filho
                Image img = ammoIMG[i].transform.GetChild(j).GetComponent<Image>();
                if (i < playerController.playerCombat.Ammo)
                {
                    // Restaura a cor original do Image
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1f); // Altera o alpha para 1
                }
                else
                {
                    // Altera a cor do Image para semi-transparente
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 0.25f); // Altera o alpha para 0.5
                }
            }
        }

        for (int i = 0; i < ammoSliders.Length; i++)
        {
            // Se a munição é menor ou igual ao índice do slider, começa a recarregar
            if (playerController.playerCombat.Ammo <= i)
            {
                ammoSliders[i].gameObject.SetActive(true);
                StartCoroutine(Reload(ammoSliders[i], playerController.playerCombat.ReloadTime));
            }
            else
            {
                ammoSliders[i].gameObject.SetActive(false);
            }
        }
    }

    IEnumerator Reload(Slider slider, float reloadTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < reloadTime)
        {
            slider.value = elapsedTime / reloadTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        slider.value = 1f;
    }
    
}
