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
                else if (type == PowerUpType.Wanjian)
                {
                    playerController.SwitchWeapon((int)WeaponType.Wanjian, 12f); 
                }
                else if (type == PowerUpType.Dianxueshou)
                {
                    playerController.SwitchWeapon((int)WeaponType.Dianxueshou, 8f);
                }
                else if (type == PowerUpType.Jingangzhou)
                {
                    playerController.ActiveJingangzhou(10f);
                }

                if (type == PowerUpType.Rulaishenzhang)
                {
                    ActivateRulaishenzhangEffect();
                }
                else if (type == PowerUpType.Xixingdafa)
                {
                    ActivateXixingdafaEffect();
                }
                else if (type == PowerUpType.Biyuefucheng)
                {
                    ActivateBiyuefuchengEffect();
                }
                else if (type == PowerUpType.Liangyizhenqi)
                {
                    ActivateLiangyizhenqiEffect();
                }
                else if (type == PowerUpType.Xuechi)
                {
                    ActivateXuechiEffect();
                }
                else if (type == PowerUpType.Tianleizhan)
                {
                    ActiveTianleizhanEffect();
                }
                

                // ...�������ߵ��߼�

                Destroy(gameObject);  // ������Һ����ٵ���
            }
        }
    }
    private void ActivateRulaishenzhangEffect()
    {
        GameObject relai = Instantiate(WeaponManager.Instance.RulaishenzhangPrefab,transform.position, Quaternion.identity);
        
        // ��1�������
        Destroy(relai, 3f);
    }
    private void ActivateXixingdafaEffect()
    {
        GameObject xixing = Instantiate(WeaponManager.Instance.XixingdafaPrefab,transform.position, Quaternion.identity);
        
        // ��1�������
        Destroy(xixing, 6f);
    }

    private void ActivateBiyuefuchengEffect()
    {
        GameObject biyue = Instantiate(WeaponManager.Instance.BiyuefuchengPrefab,transform.position, Quaternion.identity);
        
        // ��1�������
        Destroy(biyue, 6f);
    }


    private void ActivateXuechiEffect()
    {
        GameObject xuechi = Instantiate(WeaponManager.Instance.XuechiPrefab,transform.position, Quaternion.identity);
        
        // ��1�������
        Destroy(xuechi, 6f);
    }
    
     private void ActivateLiangyizhenqiEffect()
    {
        GameObject liangyi = Instantiate(WeaponManager.Instance.LiangyizhenqiPrefab,transform.position, Quaternion.identity);
        
        // ��1�������
        Destroy(liangyi, 6f);
    }

    private void ActiveTianleizhanEffect()
    {
        Camera mainCam = Camera.main;
        Vector3 centerPosition = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCam.nearClipPlane + 1));
        GameObject tianlei = Instantiate(WeaponManager.Instance.TianleizhanPrefab,centerPosition, Quaternion.identity);
        
        // ��1�������
        Destroy(tianlei, 10f);
    }



}
