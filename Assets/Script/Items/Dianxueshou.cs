using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dianxueshou : MonoBehaviour
{
    public float speed = 40f;
    public float stoptime = 5f; // 这是基础伤害，可以受到玩家攻击力的影响
    public float lifeDuration = 5f; // 子弹的生存时长
    private Vector2 moveDirection; // 子弹的移动方向
    
    private void Start()
    {
        // 设置子弹的移动方向为其初始向上的方向
        //moveDirection = transform.up;
        // 10秒后销毁子弹
        Destroy(gameObject, lifeDuration);
    }
    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime); // 每帧移动
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        // 我们假设敌人的tag为"Enemy"，你还可以添加其他标签，如"Obstacle"等
        if (collision.CompareTag("Enemy") || collision.CompareTag("Obstacle"))
        {   
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                float Stoptime = stoptime; // 这里可以添加玩家攻击力的影响
                enemy.Stopstill(stoptime);
            }
            if (collision.CompareTag("Obstacle"))
            {
                Destroy(gameObject); // 销毁子弹
            }
            //Destroy(gameObject); // 销毁子弹
        }
    }

    public void SetMoveDirection(Vector2 dir)
    {
    moveDirection = dir;
    }
}
