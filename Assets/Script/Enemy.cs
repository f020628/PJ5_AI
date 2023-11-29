using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 10f;
    public float speed = 0.5f;
    public float attackDamage = 5f;

    private bool movable = false;
    private bool die = false;
    public Animator animator; // 敌人的动画控制器

    public Collider2D enemyCollider; // 敌人的碰撞器
    public Transform spriteTransform; // 用于控制敌人Sprite的旋转
    private Transform player;
    private PlayerController playerController; // 假设玩家的脚本名为 PlayerController
    public GameObject CoinPrefab;

    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRenderer2;

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
        speed += Random.Range(-0.1f, 0.2f);
        Stronger();

    }

    private void Update()
    {
       if(movable)
   {
       Vector2 direction = (player.position - transform.position).normalized;

       // 计算敌人Sprite朝向玩家的角度
       float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

       // 只修改spriteTransform的旋转，而不是整个敌人物体的旋转
       spriteTransform.rotation = Quaternion.Euler(0, 0, angle);

       transform.Translate(direction * speed * Time.deltaTime);
   } 

        
    }

    public void OnSpawnAnimationComplete()
    {
        if(die == false){
            enemyCollider.enabled = true;
            spriteRenderer.enabled = true;
            spriteRenderer2.enabled = false;
            
            movable = true;
        }
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
        spriteRenderer.color = new Color(1f, 1f, 0f, 1f);
        Invoke("ResumeMovement", stoptime);
    }

    public void Die()
    {   movable = false;    
        die = true;
        enemyCollider.enabled = false;
        //有概率生成金币
        if(Random.Range(0,10)>5)
        {
            Instantiate(CoinPrefab, transform.position, Quaternion.identity);
        }
        GameManager.Instance.enemyKills++;
        spriteRenderer.enabled = false; 
        spriteRenderer2.enabled = true;
        // 播放死亡动画
        //animator.speed = -1f;
       // animator.Play("FadeIn", -1, 1f); // Start the fade-in animation from its end

        //StartCoroutine(WaitAndDestroy(animator.GetCurrentAnimatorStateInfo(0).length));
        animator.Play("FadeOut");
    }
    
    
    IEnumerator WaitAndDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&movable)
        {
            playerController.TakeDamage(attackDamage); // 假设PlayerController有一个TakeDamage方法
            Die(); // 碰撞后敌人消失
        }
    }

    public void AttractTowards(Vector2 direction, float strength)
    {
        transform.Translate(direction * strength * Time.deltaTime);
    }
    public void ResumeMovement()
    {
        movable = true;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    private void Stronger()
    {
        int count = GameManager.Instance.enemyKills;
        int add_health = count/10 ;
        if (add_health>100)
        {
            add_health = 100;
        }
        health += add_health;
    }


}

