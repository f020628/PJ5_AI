using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SpriteSequence : MonoBehaviour
{
    private GameObject[] spriteObjects;

    void Start()
    {
        // ��ȡ�����Ӷ���
        spriteObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spriteObjects[i] = transform.GetChild(i).gameObject;
        }
        StartCoroutine(ShowSpritesInSequence());
    }

    private IEnumerator ShowSpritesInSequence()
    {
        for (int i = 0; i < spriteObjects.Length; i++)
        {
            spriteObjects[i].SetActive(true); // ��ʾ��ǰsprite����
            yield return new WaitForSeconds(0.5f); // �ȴ�1��

            // ����������һ��sprite���ͼ�����ʾ��һ��
            if (i != spriteObjects.Length - 1)
            {
                spriteObjects[i + 1].SetActive(true);
            }

            // ����ʾ��һ��sprite��������ص�ǰsprite����
            yield return new WaitForSeconds(0.5f);
            spriteObjects[i].SetActive(false);
        }
    }
}



