using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    void Start()
    {
        //ѭ�����Ž�Ҷ���
        animator.Play("CoinR");
        //15s����������
        Destroy(gameObject, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�������ʱ��������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //�������1
            GameManager.Instance.coins++;

            Destroy(gameObject);
        }
    }
}
