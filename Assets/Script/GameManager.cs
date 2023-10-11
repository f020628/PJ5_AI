using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // <- Don't forget to add this for TextMeshPro

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public PlayerController playerController;

    public int coins;
    public int enemyKills;

    public TextMeshProUGUI coinText;  // Reference to the TextMeshProUGUI component for coins
    public TextMeshProUGUI killText;  // Reference to the TextMeshProUGUI component for enemyKills
    public TextMeshProUGUI healthText;  // Reference to the TextMeshProUGUI component for health

    public TextMeshProUGUI timerText;  // Reference to the TextMeshProUGUI component for timer


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        float health = playerController.ShowHealth();
        // Update the text of the TextMeshPro components
        coinText.text = "" + coins;
        killText.text = "" + enemyKills;
        healthText.text = "" + Mathf.RoundToInt(health);
        timerText.text = "" + Time.time;
    }
}
