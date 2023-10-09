using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tianleizhan : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            // ÏûÃðµÐÈË
            enemy.Die();    
        }

    }
}
