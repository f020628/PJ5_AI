using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xixingdafa : MonoBehaviour
{
    public float attractionStrength = 0.1f; // ������ǿ��

    private void OnTriggerStay2D(Collider2D collision)
    {
        // �����ײ�����Ƿ��ǵ���
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            // ����ӵ��˵���������ķ���
            Vector2 attractionDirection = (transform.position - collision.transform.position).normalized;
            // ʹ���˳������������ƶ�
            enemy.AttractTowards(attractionDirection, attractionStrength);
        }
    }
}

