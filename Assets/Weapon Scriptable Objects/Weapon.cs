using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]

    public class Weapon : ScriptableObject
    {
        [SerializeField] public GameObject equippedPrefab = null;

        [SerializeField] public TextMeshProUGUI ammoLevel;
        // [SerializeField] public TextMeshProUGUI armsWeaponNumber;
        [SerializeField] TextMeshProUGUI weaponDescription;

        public float cooldown;

        public int ammoCapacity;

        // public float GetDamage() => flatBonusDamageModifier;
        // public float GetPercentageBonus() => totalBonusDamageModifier;
        // public float GetRange() => weaponRange;
    }
