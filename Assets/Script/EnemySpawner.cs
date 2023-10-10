using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpawnPattern
{
   /*  public GameObject enemyPrefab;
    public GameObject BigenemyPrefab; */

    public int rowCount;
    public int colCount;

    public float spawnInterval = 0f;
    public Vector2 spawnOffset = new Vector2(0.6f, 0.6f);
    public SpawnLayout layout;
    public SpawnDirection spawnDirection;

}
[System.Serializable]
public class Wave
{
    public List<SpawnPattern> spawnPatterns;
    public List<Transform> spawnLocations;
    public float waveInterval = 5f;  // 每个Wave之间的时间间隔
    public float patternInterval = 2f;
}

public class EnemySpawner : MonoBehaviour
{
    public List<Wave> waves;
    public List<Transform> potentialSpawnLocations;  // 所有Wave共用的生成位置列表
    private int currentWave = 0;
    
    private int maxWave = 10;
    
    public Rect gameArea = new Rect(-8f, -4.5f, 16, 9);// 假设场地是一个15x18的区域，中心位于(0,0)

    public GameObject enemyPrefab;
    public GameObject BigenemyPrefab;

    public int minRowCount = 3;
    public int maxRowCount = 6;
    public int minColCount = 3;
    public int maxColCount = 6;

    public float waveInterval = 3f;  // 每个Wave之间的时间间隔
    public float patternInterval = 1.5f;

    private void Start()
    {
        StartSpawning();
    }

    void OnDrawGizmos()
{
    Gizmos.color = Color.red;  // 设置Gizmo的颜色为红色
    Gizmos.DrawWireCube(gameArea.center, gameArea.size);  // 绘制一个边界框，表示游戏区域
}
    public void StartSpawning()
    {
        StartCoroutine(SpawnWaveRoutine());
    }

    public void checkSpawn(GameObject enemy)
    {
        if(!gameArea.Contains(enemy.transform.position))
        {
            Destroy(enemy);
        }

    }

   private IEnumerator SpawnWaveRoutine()
{
    while(currentWave < maxWave)
    {
        for (int i = 0; i < 25; i++)
        {
            SpawnPattern pattern = new SpawnPattern();
            // Randomize row and col count for this pattern
            pattern.rowCount = Random.Range(minRowCount, maxRowCount + 1);
            pattern.colCount = Random.Range(minColCount, maxColCount + 1); 

            Transform randomSpawnLocation = potentialSpawnLocations[Random.Range(0, potentialSpawnLocations.Count)];
            yield return StartCoroutine(SpawnPatternRoutine(pattern, randomSpawnLocation));
            yield return new WaitForSeconds(patternInterval);  // 等待下一组敌人
        }

        // 清除场上所有敌人
        ClearAllEnemies();

        yield return new WaitForSeconds(waveInterval - patternInterval);  // 减去最后一组和下一波之间的间隔
        currentWave++;
        int increaseValue = Mathf.CeilToInt(currentWave / 5.0f);
        minRowCount += increaseValue;
        maxRowCount += increaseValue;
        minColCount += increaseValue;
        maxColCount += increaseValue;
    }
}


   private IEnumerator SpawnPatternRoutine(SpawnPattern pattern, Transform spawnLocation)
{
    
    SpawnLayout randomLayout = (SpawnLayout)Random.Range(0, System.Enum.GetValues(typeof(SpawnLayout)).Length);
    pattern.spawnDirection = (SpawnDirection)Random.Range(0, System.Enum.GetValues(typeof(SpawnDirection)).Length);
    switch (randomLayout)
    {
        case SpawnLayout.Rectangle:
            yield return StartCoroutine(SpawnRectangle(pattern, spawnLocation));
            break;
        case SpawnLayout.Circle:
            yield return StartCoroutine(SpawnCircle(pattern, spawnLocation));
            break;
        case SpawnLayout.Trapezoid:
            yield return StartCoroutine(SpawnTrapezoid(pattern, spawnLocation));
            break;
        case SpawnLayout.Cross:
            yield return StartCoroutine(SpawnCross(pattern, spawnLocation));
            break;
    }
}

private IEnumerator SpawnRectangle(SpawnPattern pattern, Transform spawnLocation)
{
    for (int row = 0; row < pattern.rowCount; row++)
    {
        for (int col = 0; col < pattern.colCount; col++)
        {
            Vector2 spawnPosition = spawnLocation.position;
            
            switch(pattern.spawnDirection)
            {
                case SpawnDirection.Up:
                    spawnPosition.x += col * pattern.spawnOffset.x;
                    spawnPosition.y += row * pattern.spawnOffset.y;
                    break;
                case SpawnDirection.Down:
                    spawnPosition.x += col * pattern.spawnOffset.x;
                    spawnPosition.y -= row * pattern.spawnOffset.y;
                    break;
                case SpawnDirection.Left:
                    spawnPosition.x -= row * pattern.spawnOffset.x;
                    spawnPosition.y += col * pattern.spawnOffset.y;
                    break;
                case SpawnDirection.Right:
                    spawnPosition.x += row * pattern.spawnOffset.x;
                    spawnPosition.y += col * pattern.spawnOffset.y;
                    break;
            }

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            checkSpawn(enemy);
            yield return new WaitForSeconds(pattern.spawnInterval);
        }
    }
}

    private IEnumerator SpawnCircle(SpawnPattern pattern, Transform spawnLocation)
    {
        float angleStep = 360f / pattern.rowCount;
        float radius = pattern.spawnOffset.x;  // 使用spawnOffset.x作为半径

        for (int i = 0; i < pattern.rowCount; i++)
        {
            float angle = i * angleStep;
            Vector2 positionOffset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
            GameObject enemy = Instantiate(enemyPrefab, (Vector2)spawnLocation.position + positionOffset, Quaternion.identity);
            checkSpawn(enemy);
            yield return new WaitForSeconds(pattern.spawnInterval);
        }
    }

    private IEnumerator SpawnTrapezoid(SpawnPattern pattern, Transform spawnLocation)
    {
        int baseEnemies = 2;

        for (int row = 0; row < pattern.rowCount; row++)
        {
            int enemiesThisRow = baseEnemies + row;

            for (int col = 0; col < enemiesThisRow; col++)
            {
                Vector2 positionOffset = Vector2.zero;

                switch(pattern.spawnDirection)
                {
                    case SpawnDirection.Up:
                        positionOffset = new Vector2(col * pattern.spawnOffset.x - (enemiesThisRow - 1) * pattern.spawnOffset.x / 2, row * pattern.spawnOffset.y);
                        break;
                    case SpawnDirection.Down:
                        positionOffset = new Vector2(col * pattern.spawnOffset.x - (enemiesThisRow - 1) * pattern.spawnOffset.x / 2, -row * pattern.spawnOffset.y);
                        break;
                    case SpawnDirection.Left:
                        positionOffset = new Vector2(-row * pattern.spawnOffset.x, col * pattern.spawnOffset.y - (enemiesThisRow - 1) * pattern.spawnOffset.y / 2);
                        break;
                    case SpawnDirection.Right:
                        positionOffset = new Vector2(row * pattern.spawnOffset.x, col * pattern.spawnOffset.y - (enemiesThisRow - 1) * pattern.spawnOffset.y / 2);
                        break;
                }

                GameObject enemy = Instantiate(enemyPrefab, (Vector2)spawnLocation.position + positionOffset, Quaternion.identity);
                checkSpawn(enemy);
                
                yield return new WaitForSeconds(pattern.spawnInterval);
            }
        }
    }


    private IEnumerator SpawnCross(SpawnPattern pattern, Transform spawnLocation)
    {
        // Center enemy
        Instantiate(enemyPrefab, spawnLocation.position, Quaternion.identity);
        yield return new WaitForSeconds(pattern.spawnInterval);
        // Horizontal and vertical enemies
        for (int i = 1; i <= pattern.rowCount / 2; i++)  // 假设count是偶数，例如4表示上下左右各生成一个敌人
        {
            GameObject enemy1 = Instantiate(enemyPrefab, spawnLocation.position + new Vector3(i * pattern.spawnOffset.x, 0, 0), Quaternion.identity);
            checkSpawn(enemy1);
            GameObject enemy2 =Instantiate(enemyPrefab, spawnLocation.position - new Vector3(i * pattern.spawnOffset.x, 0, 0), Quaternion.identity);
            checkSpawn(enemy2);
            GameObject enemy3 =Instantiate(enemyPrefab, spawnLocation.position + new Vector3(0, i * pattern.spawnOffset.y, 0), Quaternion.identity);
            checkSpawn(enemy3);
            GameObject enemy4 =Instantiate(enemyPrefab, spawnLocation.position - new Vector3(0, i * pattern.spawnOffset.y, 0), Quaternion.identity);
            checkSpawn(enemy4);
            yield return new WaitForSeconds(pattern.spawnInterval);
        }
    }




    private void ClearAllEnemies()
{
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    foreach (GameObject enemy in enemies)
    {
        Destroy(enemy);
    }
}
}

