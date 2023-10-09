using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Xuechi : MonoBehaviour
{
    public float timeToGainHealth = 1f; // 敌人需要在Xuechi上停留的时间

    private Dictionary<Enemy, float> enemiesInXuechi = new Dictionary<Enemy, float>();

    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();

    foreach (var enemy in enemiesInXuechi.Keys.ToList())
    {
        enemiesInXuechi[enemy] += Time.deltaTime;

        if (enemiesInXuechi[enemy] >= timeToGainHealth)
        {
            playerController.IncreaseHealth(1);
            enemiesToRemove.Add(enemy);
        }
    }

    foreach (var enemy in enemiesToRemove)
    {
        enemiesInXuechi.Remove(enemy);
    }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !enemiesInXuechi.ContainsKey(enemy))
        {
            enemiesInXuechi.Add(enemy, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemiesInXuechi.Remove(enemy);
        }
    }
}

