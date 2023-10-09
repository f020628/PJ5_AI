using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SpriteSequence : MonoBehaviour
{
    private GameObject[] spriteObjects;

    void Start()
    {
        // 获取所有子对象
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
            spriteObjects[i].SetActive(true); // 显示当前sprite物体
            yield return new WaitForSeconds(0.5f); // 等待1秒

            // 如果不是最后一个sprite，就继续显示下一个
            if (i != spriteObjects.Length - 1)
            {
                spriteObjects[i + 1].SetActive(true);
            }

            // 在显示下一个sprite物体后，隐藏当前sprite物体
            yield return new WaitForSeconds(0.5f);
            spriteObjects[i].SetActive(false);
        }
    }
}



