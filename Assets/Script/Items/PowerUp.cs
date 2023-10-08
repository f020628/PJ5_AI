using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    

    public PowerUpType type;
    public float existenceDuration = 15f;  // ���ߴ��ڵ�ʱ��
    private float spawnTime;

    public float moveSpeed = 0.2f;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
        moveDirection = Random.insideUnitCircle.normalized;  // ����һ������ĵ�λ��������Ϊ�ƶ�����
    }

    private void Update()
    {
        // ������ߴ��ڵ�ʱ�䳬����existenceDuration����������
        if (Time.time > spawnTime + existenceDuration)
        {
            Destroy(gameObject);
        }
        rb.velocity = moveDirection * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ������������������ʱ������
        Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
        rb.velocity = reflectedVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerController playerController = collider.GetComponent<PlayerController>();
            Debug.Log("PlayerController: " + playerController);
            
            if (playerController != null) // ȷ����ȡ��PlayerController�����Ϊnull
            {
                if (type == PowerUpType.Wanbiao)
                {
                    playerController.SwitchWeapon((int)WeaponType.Wanbiao, 8f); 
                }
                // ...�������ߵ��߼�

                Destroy(gameObject);  // ������Һ����ٵ���
            }
        }
    }
}
