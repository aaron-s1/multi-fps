using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    [SerializeField] GameObject weapon1;
    [SerializeField] GameObject weapon2;
    [SerializeField] GameObject weapon3;

    GameObject currentWeapon;

    void Awake()
    {
        currentWeapon = weapon1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwapToWeapon(weapon2);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwapToWeapon(weapon2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwapToWeapon(weapon3);


        if (Input.GetMouseButtonDown(0))
            FireCurrentWeapon();
    }

    void FireCurrentWeapon()
    {
        // if not on cooldown
    }

    void SwapToWeapon(GameObject newWeapon)
    {
        // put away other weapons
        currentWeapon = newWeapon;
    }
}
