using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �������
    public float health = 100;
    public int coins = 0;
    public float attackPower = 10f;
    public float moveSpeed = 5f;
    public int enemyKillCount = 0;
    public float sprintDuration = 1f; // ��̳���ʱ��
    public float sprintCooldown = 3f; // �����ȴʱ��
    public float sprintSpeedMultiplier = 2f; // ����ٶȱ���
    public float rotationSpeed = 60f; // ÿ����ת�Ķ���
    public float shootInterval = 0.05f; // ÿ0.5�����һ��

    private float nextShootTime = 0f;

    public GameObject blade1;
    public GameObject blade2;
    

    public Transform gunTransform; // ǹ�ڵ�Transform���
    public float gunRotationSpeed = 30f; // ǹ����ת�Ķ���

    private float sprintEndTime;
    private float sprintNextAvailableTime;


    // �ƶ��ͳ��
    private Rigidbody2D rb;
    private Vector2 moveInput;

    // �ӵ��͹���
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;

    private Dictionary<WeaponType, System.Action> weaponActivationMethods;
    private Dictionary<WeaponType, System.Action> weaponDeactivationMethods;

    // ���๥���������...

     
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
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); // ˳ʱ����ת
    }
    private void RotateGun()
    {
        gunTransform.Rotate(0, 0, -gunRotationSpeed * Time.deltaTime); // ��תǹ
    }
    private void HandleMovement()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput.Normalize();
        // ��ӹ��Ե��߼�...
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

        // ���ÿ��������ͣ�÷�����ͬ�������ڴ�Ϊ weaponDeactivationMethods �ֵ���亯��
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
        
       /*  float elapsedTimeSinceLastShot = Time.time - nextShootTime + shootInterval; // ��ȡ���ϴ������������������ʱ��
        float estimatedRotation = rotationSpeed * elapsedTimeSinceLastShot *2; // �������ת�Ƕȣ�����2ʹ����ӽ���ҵ�ǰ����ת״̬
        Vector2 estimatedDirection = Quaternion.Euler(0, 0, estimatedRotation) * transform.up; // ����ķ��䷽�� */
        Vector2 currentDirection = gunTransform.up;

        Wanbiao wanbiao1 = Instantiate(WeaponManager.Instance.wanbiaoPrefab, firePoint1.position, firePoint1.rotation).GetComponent<Wanbiao>();
        Wanbiao wanbiao2 = Instantiate(WeaponManager.Instance.wanbiaoPrefab, firePoint2.position, firePoint2.rotation).GetComponent<Wanbiao>();

        wanbiao1.SetMoveDirection(currentDirection); // ��һ���ӵ��ع��㷽��
        wanbiao2.SetMoveDirection(-currentDirection); // �ڶ����ӵ����෴����
        //Debug.Log("Shooting at: " + Time.time);
        // �����߼�...

        // �ӵ����ԡ�������������߼�...
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
            // ����ڼ���߼�...
            yield return null;
        }

        moveSpeed = originalSpeed;
        sprintNextAvailableTime = Time.time + sprintCooldown;
    }

    private IEnumerator WeaponDurationCountdown()
    {
        yield return new WaitForSeconds(weaponDuration);
        currentWeapon = WeaponType.Basic;
        ActivateBlades();  // ����Ч���������ٴ����õ�
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
        // ������������߼�...
    }
    // ������Ҽ񵽵��ߡ����ˡ��������߼�...

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
