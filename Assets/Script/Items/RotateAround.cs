using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{

    public Transform bigCircleCenter; // ��Բ������
    public float rotationSpeed = 30f; // ÿ����ת�ĽǶ�

    void Update()
    {
        // ʹСԲ�ƴ�Բ��������ת
        transform.RotateAround(bigCircleCenter.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }


}
