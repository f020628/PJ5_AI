using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject wanbiaoPrefab;
    public GameObject wanjianPrefab;
    public GameObject dianxueshouPrefab;
    // 其他的武器预制体...

    private static WeaponManager _instance;

    public static WeaponManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WeaponManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        // 可选：保证在加载新场景时不被销毁
        // DontDestroyOnLoad(gameObject);
    }
}

