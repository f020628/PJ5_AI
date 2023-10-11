using UnityEngine;

public class Biyuefucheng : MonoBehaviour
{
    public float rotationSpeed = 30f; // ÿ�����ת����
    private float rotationSum = 0f; // ��ǰ����ת�ܺ�
    private bool isRotatingLeft = true;
    private bool firstTime = true;
    public Collider2D Collider; // ���˵���ײ��

    private void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        if (isRotatingLeft)
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, 0f, rotationThisFrame);
            rotationSum += rotationThisFrame;

            if (rotationSum >= 90f&&firstTime == true)
            {   
                isRotatingLeft = false;
                rotationSum = 0f;
                //ˮƽ��ת
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;

            }
            else if(rotationSum >= 180f)
            {
                isRotatingLeft = false;
                rotationSum = 0f;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                  
            }
        }
        else if (!isRotatingLeft)
        {
            float rotationThisFrame = -rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, 0f, rotationThisFrame);
            rotationSum += -rotationThisFrame;

            if (rotationSum >= 180f)
            {
                isRotatingLeft = true;
                rotationSum = 0f;
                firstTime = false;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
    }
    
}
