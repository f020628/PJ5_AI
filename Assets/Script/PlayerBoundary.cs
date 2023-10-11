using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerBoundary : MonoBehaviour
{
    public float minX = -8f;
    public float maxX = 16f;
    public float minY = -4.5f;
    public float maxY = 9f;

    void Update()
    {
        // 限制玩家的位置
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}

