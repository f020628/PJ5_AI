using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xixingdafa : MonoBehaviour
{
    public float attractionStrength = 0.1f; // 吸引力强度

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 检查碰撞物体是否是敌人
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            // 计算从敌人到吸引物体的方向
            Vector2 attractionDirection = (transform.position - collision.transform.position).normalized;
            // 使敌人朝向吸引物体移动
            enemy.AttractTowards(attractionDirection, attractionStrength);
        }
    }
}

