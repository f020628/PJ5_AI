using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liangyizhenqi : MonoBehaviour
{

    public float slowDownFactor = 0.5f; // 减速因子，0.5表示速度减半

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.speed *= slowDownFactor; // 减速
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.speed /= slowDownFactor; // 恢复速度
        }
    }

}
