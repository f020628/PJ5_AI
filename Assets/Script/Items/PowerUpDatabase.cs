using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpItem
{
    public PowerUpType type; 
    public Sprite sprite;
}

[CreateAssetMenu(menuName = "PowerUp Database")]
public class PowerUpDatabase : ScriptableObject
{
    public List<PowerUpItem> allPowerUps;
}

