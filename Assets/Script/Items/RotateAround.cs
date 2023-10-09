using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{

    public Transform bigCircleCenter; // 大圆的中心
    public float rotationSpeed = 30f; // 每秒旋转的角度

    void Update()
    {
        // 使小圆绕大圆的中心旋转
        transform.RotateAround(bigCircleCenter.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }


}
