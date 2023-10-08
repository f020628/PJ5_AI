using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    

    public PowerUpType type;
    public float existenceDuration = 15f;  // 道具存在的时间
    private float spawnTime;

    public float moveSpeed = 0.2f;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
        moveDirection = Random.insideUnitCircle.normalized;  // 生成一个随机的单位向量，作为移动方向
    }

    private void Update()
    {
        // 如果道具存在的时间超过了existenceDuration，就销毁它
        if (Time.time > spawnTime + existenceDuration)
        {
            Destroy(gameObject);
        }
        rb.velocity = moveDirection * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 当道具碰到其他物体时，反弹
        Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
        rb.velocity = reflectedVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerController playerController = collider.GetComponent<PlayerController>();
            Debug.Log("PlayerController: " + playerController);
            
            if (playerController != null) // 确保获取的PlayerController组件不为null
            {
                if (type == PowerUpType.Wanbiao)
                {
                    playerController.SwitchWeapon((int)WeaponType.Wanbiao, 8f); 
                }
                // ...其他道具的逻辑

                Destroy(gameObject);  // 触碰玩家后，销毁道具
            }
        }
    }
}
