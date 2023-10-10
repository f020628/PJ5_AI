using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    void Start()
    {
        //循环播放金币动画
        animator.Play("CoinR");
        //15s后销毁自身
        Destroy(gameObject, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //碰到玩家时销毁自身
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //金币数加1
            GameManager.Instance.coins++;

            Destroy(gameObject);
        }
    }
}
