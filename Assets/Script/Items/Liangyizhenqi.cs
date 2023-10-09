using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liangyizhenqi : MonoBehaviour
{

    public float slowDownFactor = 0.5f; // �������ӣ�0.5��ʾ�ٶȼ���

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.speed *= slowDownFactor; // ����
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.speed /= slowDownFactor; // �ָ��ٶ�
        }
    }

}
