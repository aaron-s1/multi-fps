using UnityEngine;

[CreateAssetMenu(fileName = "Weapon List", menuName = "Weapons/Make New Weapon List", order = 1)]
    public class WeaponList : ScriptableObject
    {
        public Weapon[] weapons;
    }
