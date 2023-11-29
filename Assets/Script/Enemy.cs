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
    public Animator animator; // ���˵Ķ���������

    public Collider2D enemyCollider; // ���˵���ײ��
    public Transform spriteTransform; // ���ڿ��Ƶ���Sprite����ת
    private Transform player;
    private PlayerController playerController; // ������ҵĽű���Ϊ PlayerController
    public GameObject CoinPrefab;

    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRenderer2;

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
        speed += Random.Range(-0.1f, 0.2f);
        Stronger();

    }

    private void Update()
    {
       if(movable)
   {
       Vector2 direction = (player.position - transform.position).normalized;

       // �������Sprite������ҵĽǶ�
       float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

       // ֻ�޸�spriteTransform����ת�����������������������ת
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
        spriteRenderer.color = new Color(1f, 1f, 0f, 1f);
        Invoke("ResumeMovement", stoptime);
    }

    public void Die()
    {   movable = false;    
        die = true;
        enemyCollider.enabled = false;
        //�и������ɽ��
        if(Random.Range(0,10)>5)
        {
            Instantiate(CoinPrefab, transform.position, Quaternion.identity);
        }
        GameManager.Instance.enemyKills++;
        spriteRenderer.enabled = false; 
        spriteRenderer2.enabled = true;
        // ������������
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
            playerController.TakeDamage(attackDamage); // ����PlayerController��һ��TakeDamage����
            Die(); // ��ײ�������ʧ
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

