using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpSpawner : MonoBehaviour
{
    public List<GameObject> powerUps;  // 所有道具的列表
    public float spawnInterval = 10f;  // 生成间隔
    public Rect gameArea = new Rect(-8f, -4.5f, 16, 9);  // 游戏区域
    public GameObject powerUpPrefab;  // 道具预制体

    public PowerUpDatabase powerUpDatabase;  // 道具数据库
    private List<PowerUpItem> currentPowerUps;  // 当前关卡的道具列表

    private void Start()
    {
        currentPowerUps = new List<PowerUpItem>();
        while (currentPowerUps.Count < 10)
        {
            PowerUpItem randomItem = powerUpDatabase.allPowerUps[Random.Range(0, powerUpDatabase.allPowerUps.Count)];
            if (!currentPowerUps.Contains(randomItem))
            {
                currentPowerUps.Add(randomItem);
            }
        }
        InvokeRepeating("SpawnPowerUp", spawnInterval, spawnInterval);  // 每隔spawnInterval秒生成一个道具
    }

    private void SpawnPowerUp()
    {
        Vector2 spawnPosition = new Vector2(
            Random.Range(gameArea.xMin, gameArea.xMax),
            Random.Range(gameArea.yMin, gameArea.yMax)
        );
        PowerUpItem randomPowerUpItem = currentPowerUps[Random.Range(0, currentPowerUps.Count)];
        GameObject spawnedPowerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        spawnedPowerUp.GetComponent<SpriteRenderer>().sprite = randomPowerUpItem.sprite;
        spawnedPowerUp.GetComponent<PowerUp>().type = randomPowerUpItem.type;
    }
}

