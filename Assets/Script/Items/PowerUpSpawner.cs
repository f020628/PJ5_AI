using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpSpawner : MonoBehaviour
{
    public List<GameObject> powerUps;  // ���е��ߵ��б�
    public Rect gameArea = new Rect(-8f, -4.5f, 16, 9);  // ��Ϸ����
    public GameObject powerUpPrefab;  // ����Ԥ����

    public PowerUpDatabase powerUpDatabase;  // �������ݿ�
    private List<PowerUpItem> currentPowerUps;  // ��ǰ�ؿ��ĵ����б�

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
       SpawnPowerUpWithInterval();
    }

    private void SpawnPowerUpWithInterval()
    {
        float spawnInterval = Random.Range(8, 12); // ����ʱ����8-12֮�����
        SpawnPowerUp();
        Invoke("SpawnPowerUpWithInterval", spawnInterval); // ʹ��Invoke����InvokeRepeating
    }
    
    
    
    private void SpawnPowerUp()
    {
        int numberOfPowerUps = Random.Range(1, 4); // �������1-3������
        for (int i = 0; i < numberOfPowerUps; i++)
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
}    
/* private void SpawnPowerUp()
    {
        Vector2 spawnPosition = new Vector2(
            Random.Range(gameArea.xMin, gameArea.xMax),
            Random.Range(gameArea.yMin, gameArea.yMax)
        );
        PowerUpItem randomPowerUpItem = currentPowerUps[Random.Range(0, currentPowerUps.Count)];
        GameObject spawnedPowerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        spawnedPowerUp.GetComponent<SpriteRenderer>().sprite = randomPowerUpItem.sprite;
        spawnedPowerUp.GetComponent<PowerUp>().type = randomPowerUpItem.type;
    } */


