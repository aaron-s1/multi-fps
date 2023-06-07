using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    [SerializeField] FirstPersonMovement FPS_Controller;

    [SerializeField] public Weapon currentWeapon;
    GameObject currentWeaponInstance;

    bool canFire;

    public TextMeshProUGUI ammoLevel;

    // This gets filled in with WeaponList (scriptable object).
    [SerializeField] public WeaponList weaponList;

    MonoBehaviour weaponsFiringScript;

    [SerializeField] public TextMeshProUGUI armsWeaponNumber;

    // [SerializeField] TextMeshProUGUI weaponDescription;


        [SerializeField] float weaponSwapCooldown = 0.5f;
        [SerializeField] float timeBetweenAttacks = 1f;

        // public Animator handsAnimator;

    int remainingAmmo;

        // float timeSinceLastAttack = Mathf.Infinity;


    void Awake()
    {
        FPS_Controller = GetComponent<FirstPersonMovement>();        
        StartCoroutine(SwapToWeapon(1));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(SwapToWeapon(1));
        if (Input.GetKeyDown(KeyCode.Alpha2))
            StartCoroutine(SwapToWeapon(2));
        if (Input.GetKeyDown(KeyCode.Alpha3))
            StartCoroutine(SwapToWeapon(3));


        if (Input.GetMouseButtonDown(0) && canFire)
            Fire();
    }


    // Keep 0 (first array) empty.
    IEnumerator SwapToWeapon(int weaponSlot)
    {
        if (currentWeapon == weaponList.weapons[weaponSlot])
            yield break;

        canFire = false;
        
        // add later: if current weapon is on cooldown, yield break.
        
        currentWeapon = null;

        if (currentWeaponInstance != null)
            Destroy(currentWeaponInstance);

        yield return new WaitForSeconds(weaponSwapCooldown);

        currentWeapon = weaponList.weapons[weaponSlot];
        currentWeaponInstance = Instantiate(currentWeapon.equippedPrefab, transform.position, transform.rotation);
        weaponsFiringScript = FindFiringScriptOfNewWeapon(currentWeaponInstance);

        currentWeaponInstance.SetActive(true);  // just in case
        armsWeaponNumber.text = weaponSlot.ToString();

        canFire = true;
    }



    public void Fire() =>
        ((IFireable)weaponsFiringScript).Fire(currentWeaponInstance);



    // Find the weapon's script, regardless of that script's name, that implements IFireable.
    MonoBehaviour FindFiringScriptOfNewWeapon(GameObject weaponInstance)
    {
        MonoBehaviour[] scripts = weaponInstance.GetComponentsInChildren<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script is IFireable)
            {
                // weaponsFiringScript = script;
                return script;
            }
        }

        return null;
    }
}
