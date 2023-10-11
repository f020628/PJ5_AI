using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    

    public PowerUpType type;
    public float existenceDuration = 15f;  // 道具存在的时间
    private float spawnTime;

    public float moveSpeed = 0.2f;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
        moveDirection = Random.insideUnitCircle.normalized;  // 生成一个随机的单位向量，作为移动方向
    }

    private void Update()
    {
        // 如果道具存在的时间超过了existenceDuration，就销毁它
        if (Time.time > spawnTime + existenceDuration)
        {
            Destroy(gameObject);
        }
        rb.velocity = moveDirection * moveSpeed;
    }

       private void OnCollisionEnter2D(Collision2D collision)
    {
        // 当道具碰到其他物体时，选择一个新的随机方向
        Debug.Log("Collided with " + collision.gameObject.name);
        
        // 选择一个新的随机方向
        moveDirection = Random.insideUnitCircle.normalized;
        rb.velocity = moveDirection * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerController playerController = collider.GetComponent<PlayerController>();
            
            if (playerController != null) // 确保获取的PlayerController组件不为null
            {   
                if (type == PowerUpType.Wanbiao)
                {
                    playerController.SwitchWeapon((int)WeaponType.Wanbiao, 8f); 
                    Debug.Log("wanbiao");
                }
                else if (type == PowerUpType.Wanjian)
                {
                    playerController.SwitchWeapon((int)WeaponType.Wanjian, 8f); 
                    Debug.Log("wanjian");
                }
                else if (type == PowerUpType.Dianxueshou)
                {
                    playerController.SwitchWeapon((int)WeaponType.Dianxueshou, 8f);
                    Debug.Log("dianxueshou");
                }
                else if (type == PowerUpType.Jingangzhou)
                {
                    playerController.ActiveJingangzhou(10f);
                    Debug.Log("jingangzhou");
                }

                if (type == PowerUpType.Rulaishenzhang)
                {
                    ActivateRulaishenzhangEffect();
                    Debug.Log("rulaishenzhang");
                }
                else if (type == PowerUpType.Xixingdafa)
                {
                    ActivateXixingdafaEffect();
                    Debug.Log("xixingdafa");
                }
                else if (type == PowerUpType.Biyuefucheng)
                {
                    ActivateBiyuefuchengEffect();
                    Debug.Log("biyuefucheng");
                }
                else if (type == PowerUpType.Liangyizhenqi)
                {
                    ActivateLiangyizhenqiEffect();
                    Debug.Log("liangyizhenqi");
                }
                else if (type == PowerUpType.Xuechi)
                {
                    ActivateXuechiEffect();
                    Debug.Log("xuechi");
                }
                else if (type == PowerUpType.Tianleizhan)
                {
                    ActiveTianleizhanEffect();
                    Debug.Log("tianleizhan");
                }
                

                // ...其他道具的逻辑

                Destroy(gameObject);  // 触碰玩家后，销毁道具
            }
        }
    }
    private void ActivateRulaishenzhangEffect()
    {
        GameObject relai = Instantiate(WeaponManager.Instance.RulaishenzhangPrefab,transform.position, Quaternion.identity);
        
        // 在1秒后销毁
        Destroy(relai, 3f);
    }
    private void ActivateXixingdafaEffect()
    {
        GameObject xixing = Instantiate(WeaponManager.Instance.XixingdafaPrefab,transform.position, Quaternion.identity);
        
        // 在1秒后销毁
        Destroy(xixing, 6f);
    }

    private void ActivateBiyuefuchengEffect()
    {
        GameObject biyue = Instantiate(WeaponManager.Instance.BiyuefuchengPrefab,transform.position, Quaternion.identity);
        
        // 在1秒后销毁
        Destroy(biyue, 6f);
    }


    private void ActivateXuechiEffect()
    {
        GameObject xuechi = Instantiate(WeaponManager.Instance.XuechiPrefab,transform.position, Quaternion.identity);
        
        // 在1秒后销毁
        Destroy(xuechi, 6f);
    }
    
     private void ActivateLiangyizhenqiEffect()
    {
        GameObject liangyi = Instantiate(WeaponManager.Instance.LiangyizhenqiPrefab,transform.position, Quaternion.identity);
        
        // 在1秒后销毁
        Destroy(liangyi, 6f);
    }

    private void ActiveTianleizhanEffect()
    {
        Camera mainCam = Camera.main;
        Vector3 centerPosition = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCam.nearClipPlane + 1));
        GameObject tianlei = Instantiate(WeaponManager.Instance.TianleizhanPrefab,centerPosition, Quaternion.identity);
        
        // 在1秒后销毁
        Destroy(tianlei, 10f);
    }



}
