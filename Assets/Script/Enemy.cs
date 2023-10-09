using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 10f;
    public float speed = 0.5f;
    public float attackDamage = 20f;

    private bool movable = false;
    public Animator animator; // ���˵Ķ���������

    public Collider2D enemyCollider; // ���˵���ײ��
    public Transform spriteTransform; // ���ڿ��Ƶ���Sprite����ת
    private Transform player;
    private PlayerController playerController; // ������ҵĽű���Ϊ PlayerController

    private void Start()
    {
        // �ҵ���Ҷ���
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();    
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        enemyCollider.enabled = false; 
        // ���ų�������
        animator.Play("FadeIn");
    }

    private void Update()
    {
       if(movable)
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // ������˳�����ҵĽǶ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        spriteTransform.rotation = Quaternion.Euler(0, 0, angle);

        transform.Translate(direction * speed * Time.deltaTime);
    }  

        
    }

    public void OnSpawnAnimationComplete()
    {
        enemyCollider.enabled = true;
        movable = true;
        // ִ���ƶ��������߼�
    }

    public void OnDeathAnimationComplete()
    {
        Destroy(gameObject);
    }


    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        //Debug.Log("Enemy health: " + health);
    }
    public void Stopstill(float stoptime)
    {
        movable = false;
        Invoke("movable",stoptime);
    }

    private void Die()
    {   movable = false;    
        enemyCollider.enabled = false;
        // ������������
        animator.Play("FadeOut");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.TakeDamage(attackDamage); // ����PlayerController��һ��TakeDamage����
            Die(); // ��ײ�������ʧ
        }
    }

    public void AttractTowards(Vector2 direction, float strength)
    {
        transform.Translate(direction * strength * Time.deltaTime);
    }

}

