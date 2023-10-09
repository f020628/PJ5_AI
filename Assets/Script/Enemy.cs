using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 10f;
    public float speed = 0.5f;
    public float attackDamage = 20f;

    private bool movable = false;
    public Animator animator; // 敌人的动画控制器

    public Collider2D enemyCollider; // 敌人的碰撞器
    public Transform spriteTransform; // 用于控制敌人Sprite的旋转
    private Transform player;
    private PlayerController playerController; // 假设玩家的脚本名为 PlayerController

    private void Start()
    {
        // 找到玩家对象
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();    
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        enemyCollider.enabled = false; 
        // 播放出生动画
        animator.Play("FadeIn");
    }

    private void Update()
    {
       if(movable)
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // 计算敌人朝向玩家的角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        spriteTransform.rotation = Quaternion.Euler(0, 0, angle);

        transform.Translate(direction * speed * Time.deltaTime);
    }  

        
    }

    public void OnSpawnAnimationComplete()
    {
        enemyCollider.enabled = true;
        movable = true;
        // 执行移动或其他逻辑
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
        // 播放死亡动画
        animator.Play("FadeOut");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.TakeDamage(attackDamage); // 假设PlayerController有一个TakeDamage方法
            Die(); // 碰撞后敌人消失
        }
    }

    public void AttractTowards(Vector2 direction, float strength)
    {
        transform.Translate(direction * strength * Time.deltaTime);
    }

}

