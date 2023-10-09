using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 玩家属性
    public float health = 100;
    public int coins = 0;
    public float attackPower = 10f;
    public float moveSpeed = 5f;
    public int enemyKillCount = 0;
    public float sprintDuration = 1f; // 冲刺持续时间
    public float sprintCooldown = 3f; // 冲刺冷却时间
    public float sprintSpeedMultiplier = 2f; // 冲刺速度倍数
    public float rotationSpeed = 60f; // 每秒旋转的度数
    public float shootInterval = 0.05f; // 每0.5秒射击一次

    private float nextShootTime = 0f;

    public GameObject blade1;
    public GameObject blade2;
    

    public Transform gunTransform; // 枪口的Transform组件
    public float gunRotationSpeed = 30f; // 枪口旋转的度数

    private float sprintEndTime;
    private float sprintNextAvailableTime;


    // 移动和冲刺
    private Rigidbody2D rb;
    private Vector2 moveInput;

    // 子弹和攻击
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;

    public Transform facingDirectionForFirePoint1;
    public Transform facingDirectionForFirePoint2;
    public Transform facingDirectionForFirePoint3;
    void OnDrawGizmos()
{
    DrawGizmoForTransform(firePoint1, Color.red);
    DrawGizmoForTransform(firePoint2, Color.green);
    DrawGizmoForTransform(firePoint3, Color.blue);
    DrawGizmoForTransform(facingDirectionForFirePoint1, Color.yellow);
    DrawGizmoForTransform(facingDirectionForFirePoint2, Color.magenta);
    DrawGizmoForTransform(facingDirectionForFirePoint3, Color.cyan);
}

void DrawGizmoForTransform(Transform trans, Color color)
{
    if (trans)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(trans.position, 0.1f);
        Gizmos.DrawLine(trans.position, trans.position + (Vector3)trans.right);
    }
}
    private Vector2 lastMoveDirection = Vector2.up; // 默认为右方向



    private Dictionary<WeaponType, System.Action> weaponActivationMethods;
    private Dictionary<WeaponType, System.Action> weaponDeactivationMethods;

    // 更多攻击相关属性...

     
    public WeaponType currentWeapon = WeaponType.Basic;
    private float weaponDuration;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InitializeWeaponDictionaries();
    }

    private void Update()
    {
        HandleMovement();
        HandleSprint();
        if (currentWeapon == WeaponType.Wanbiao)
        {
            RotateGun();
            HandleAutomaticShooting();
            HandleRotation();
        }
        else if (currentWeapon == WeaponType.Wanjian)
        {
            SetFacingDirectionBasedOnMovement();
            HandleAutomaticShooting();
        }
        else
        {
            HandleRotation();
        }
    }
   private void SetFacingDirectionBasedOnMovement()
{
    if (moveInput != Vector2.zero)
    {
        float angle = Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        facingDirectionForFirePoint1.rotation = Quaternion.Euler(0, 0, angle);
        facingDirectionForFirePoint2.rotation = Quaternion.Euler(0, 0, angle); //稍微偏转
        facingDirectionForFirePoint3.rotation = Quaternion.Euler(0, 0, angle); //稍微偏转
    }
}




    private void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }
    private void HandleRotation()
    {
    if (currentWeapon != WeaponType.Wanjian)
    {
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); // 顺时针旋转
    }
    else
    {
        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f); 
            facingDirectionForFirePoint1.localPosition = moveInput; // 这会将指示器移动到玩家的面朝方向
        }
    }
    }



    private void RotateGun()
    {   if (currentWeapon == WeaponType.Wanbiao)
        {
        gunTransform.Rotate(0, 0, -gunRotationSpeed * Time.deltaTime); // 旋转枪
        }
        
    }
    private void HandleMovement()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput.Normalize();
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
            Debug.Log("lastMoveDirection: " + lastMoveDirection+"moveInput: "+moveInput+"Time: "+Time.time);
        }
        // 添加惯性的逻辑...
    }

    private void HandleAutomaticShooting()
    {
        if (currentWeapon == WeaponType.Wanbiao && Time.time > nextShootTime)
        {
            ShootWanbiao();
            nextShootTime = Time.time + shootInterval;
        }
        if (currentWeapon == WeaponType.Wanjian && Time.time > nextShootTime)
        {
            ShootWanjian();
            nextShootTime = Time.time + shootInterval;
        }
    }

    private void InitializeWeaponDictionaries()
    {
        weaponActivationMethods = new Dictionary<WeaponType, System.Action>
        {
            { WeaponType.Basic, ActivateBlades },
            { WeaponType.Wanbiao, DeactivateBlades },
            { WeaponType.Wanjian, DeactivateBlades },
            { WeaponType.Weapon3, DeactivateBlades },
            { WeaponType.Weapon4, DeactivateBlades },
            { WeaponType.Weapon5, DeactivateBlades }
        };

        // 如果每种武器的停用方法不同，可以在此为 weaponDeactivationMethods 字典分配函数
        weaponDeactivationMethods = new Dictionary<WeaponType, System.Action>
        {
            { WeaponType.Basic, null },  // No deactivation method for Basic
            { WeaponType.Wanbiao, null },
            { WeaponType.Wanjian, null },
            { WeaponType.Weapon3, null },
            { WeaponType.Weapon4, null },
            { WeaponType.Weapon5, null }
        };
    }


    private void ShootWanbiao()
    {
        
       /*  float elapsedTimeSinceLastShot = Time.time - nextShootTime + shootInterval; // 获取从上次射击到现在所经过的时间
        float estimatedRotation = rotationSpeed * elapsedTimeSinceLastShot *2; // 估算的旋转角度，除以2使其更接近玩家当前的旋转状态
        Vector2 estimatedDirection = Quaternion.Euler(0, 0, estimatedRotation) * transform.up; // 估算的发射方向 */
        Vector2 currentDirection = gunTransform.up;

        Wanbiao wanbiao1 = Instantiate(WeaponManager.Instance.wanbiaoPrefab, firePoint1.position, firePoint1.rotation).GetComponent<Wanbiao>();
        Wanbiao wanbiao2 = Instantiate(WeaponManager.Instance.wanbiaoPrefab, firePoint2.position, firePoint2.rotation).GetComponent<Wanbiao>();

        wanbiao1.SetMoveDirection(currentDirection); // 第一颗子弹沿估算方向
        wanbiao2.SetMoveDirection(-currentDirection); // 第二颗子弹沿相反方向
        //Debug.Log("Shooting at: " + Time.time);
        // 其他逻辑...

        // 子弹属性、数量、方向的逻辑...
    }
    private void ShootWanjian()
    {
        FireFromPoint(WeaponManager.Instance.wanjianPrefab, firePoint1, facingDirectionForFirePoint1);
        FireFromPoint(WeaponManager.Instance.wanjianPrefab, firePoint2, facingDirectionForFirePoint2);
        FireFromPoint(WeaponManager.Instance.wanjianPrefab, firePoint3, facingDirectionForFirePoint3);
    }


    private void FireFromPoint(GameObject bulletPrefab, Transform firePoint, Transform facingDirection)
    {
        Vector2 direction = facingDirection.right;
        Wanjian wanjianBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, facingDirection.eulerAngles.z)).GetComponent<Wanjian>();
        wanjianBullet.SetMoveDirection(direction);
    }







    private void HandleSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > sprintNextAvailableTime)
        {
            StartCoroutine(Sprint());
        }
    }

    private IEnumerator Sprint()
    {
        float originalSpeed = moveSpeed;
        moveSpeed *= sprintSpeedMultiplier;
        sprintEndTime = Time.time + sprintDuration;

        while (Time.time < sprintEndTime)
        {
            // 冲刺期间的逻辑...
            yield return null;
        }

        moveSpeed = originalSpeed;
        sprintNextAvailableTime = Time.time + sprintCooldown;
    }

    private IEnumerator WeaponDurationCountdown()
    {
        yield return new WaitForSeconds(weaponDuration);
        currentWeapon = WeaponType.Basic;
        ActivateBlades();  // 武器效果结束后再次启用刀
    }

    private void ActivateBlades()
    {
        blade1.SetActive(true);
        blade2.SetActive(true);
    }

    private void DeactivateBlades()
    {
        blade1.SetActive(false);
        blade2.SetActive(false);
    }

     public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // 处理玩家死亡逻辑...
    }
    // 处理玩家捡到道具、受伤、攻击等逻辑...

     public void SwitchWeapon(int weaponNumber, float duration)
    {
        WeaponType selectedWeapon = (WeaponType)weaponNumber;

        // Deactivate the current weapon
        if (weaponDeactivationMethods[currentWeapon] != null)
        {
            weaponDeactivationMethods[currentWeapon].Invoke();
        }

        // Activate the selected weapon
        if (weaponActivationMethods[selectedWeapon] != null)
        {
            weaponActivationMethods[selectedWeapon].Invoke();
        }

        currentWeapon = selectedWeapon;
        weaponDuration = duration;
        StartCoroutine(WeaponDurationCountdown());
    }

}
