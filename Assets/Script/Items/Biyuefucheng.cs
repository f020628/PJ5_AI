using UnityEngine;

public class Biyuefucheng : MonoBehaviour
{
    public float rotationSpeed = 30f; // 每秒的旋转度数
    private float rotationSum = 0f; // 当前的旋转总和
    private bool isRotatingLeft = true;
    private bool firstTime = true;
    public Collider2D Collider; // 敌人的碰撞器

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
                //水平翻转
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
