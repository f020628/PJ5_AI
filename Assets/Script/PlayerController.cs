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
        if (currentWeapon == WeaponType.Wanbiao)
        {
            RotateGun();
            HandleAutomaticShooting();
        }
        HandleMovement();
        HandleSprint();
        HandleRotation();
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }
    private void HandleRotation()
    {
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); // 顺时针旋转
    }
    private void RotateGun()
    {
        gunTransform.Rotate(0, 0, -gunRotationSpeed * Time.deltaTime); // 旋转枪
    }
    private void HandleMovement()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput.Normalize();
        // 添加惯性的逻辑...
    }

    private void HandleAutomaticShooting()
    {
        if (currentWeapon == WeaponType.Wanbiao && Time.time > nextShootTime)
        {
            ShootWanbiao();
            nextShootTime = Time.time + shootInterval;
        }
    }

    private void InitializeWeaponDictionaries()
    {
        weaponActivationMethods = new Dictionary<WeaponType, System.Action>
        {
            { WeaponType.Basic, ActivateBlades },
            { WeaponType.Wanbiao, DeactivateBlades },
            { WeaponType.Weapon2, DeactivateBlades },
            { WeaponType.Weapon3, DeactivateBlades },
            { WeaponType.Weapon4, DeactivateBlades },
            { WeaponType.Weapon5, DeactivateBlades }
        };

        // 如果每种武器的停用方法不同，可以在此为 weaponDeactivationMethods 字典分配函数
        weaponDeactivationMethods = new Dictionary<WeaponType, System.Action>
        {
            { WeaponType.Basic, null },  // No deactivation method for Basic
            { WeaponType.Wanbiao, null },
            { WeaponType.Weapon2, null },
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
