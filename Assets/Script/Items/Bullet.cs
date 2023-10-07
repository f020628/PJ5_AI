using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 40f;
    public float damage = 10f; // ���ǻ����˺��������ܵ���ҹ�������Ӱ��
    public float lifeDuration = 5f; // �ӵ�������ʱ��
    private Vector2 moveDirection; // �ӵ����ƶ�����
    private void Start()
    {
        // �����ӵ����ƶ�����Ϊ���ʼ���ϵķ���
        //moveDirection = transform.up;
        // 10��������ӵ�
        Destroy(gameObject, lifeDuration);
    }
    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime); // ÿ֡�ƶ�
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���Ǽ�����˵�tagΪ"Enemy"���㻹�������������ǩ����"Obstacle"��
        if (collision.CompareTag("Enemy") || collision.CompareTag("Obstacle"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                float totalDamage = damage; // ������������ҹ�������Ӱ��
                enemy.TakeDamage(totalDamage);
            }
            Destroy(gameObject); // �����ӵ�
        }
    }

    public void SetMoveDirection(Vector2 dir)
    {
    moveDirection = dir;
    }
}
