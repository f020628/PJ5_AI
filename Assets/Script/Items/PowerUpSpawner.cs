using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpSpawner : MonoBehaviour
{
    public List<GameObject> powerUps;  // ���е��ߵ��б�
    public float spawnInterval = 10f;  // ���ɼ��
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
        InvokeRepeating("SpawnPowerUp", spawnInterval, spawnInterval);  // ÿ��spawnInterval������һ������
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

