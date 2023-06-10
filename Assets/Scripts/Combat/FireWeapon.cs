using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    // This gets filled in with WeaponList (scriptable object).
    [SerializeField] public WeaponList weaponList;

    [SerializeField] float weaponSwapCooldown = 0.5f;
    [Space(5)]
    [SerializeField] TextMeshProUGUI ammoLevelText;
    [SerializeField] TextMeshProUGUI armsWeaponNumber;
    // [SerializeField] TextMeshProUGUI weaponDescription;

    Weapon currentWeapon;    
    GameObject currentWeaponInstance;
    MonoBehaviour weaponInstanceFiringScript;

    int remainingAmmo;

    bool weaponIsCurrentlyFiring;


    void Awake() =>
        StartCoroutine(SwapWeapons(2));


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(SwapWeapons(1));
        if (Input.GetKeyDown(KeyCode.Alpha2))
            StartCoroutine(SwapWeapons(2));
        if (Input.GetKeyDown(KeyCode.Alpha3))
            StartCoroutine(SwapWeapons(3));


        if (Input.GetMouseButtonDown(0))
            Fire();
    }


    // Keep WeaponList[0] empty.
    IEnumerator SwapWeapons(int weaponSlot)
    {
        if (currentWeapon == weaponList.weapons[weaponSlot])
            yield break;
        
        if (weaponIsCurrentlyFiring)
            yield break;

        yield return new WaitForSeconds(weaponSwapCooldown);

        currentWeapon = null;
        
        if (currentWeaponInstance != null)
            Destroy(currentWeaponInstance);

        currentWeapon = weaponList.weapons[weaponSlot];        
        currentWeaponInstance = Instantiate(currentWeapon.equippedPrefab, transform.position, transform.rotation);
        currentWeaponInstance.SetActive(true);

        weaponInstanceFiringScript = FindFiringScriptOfNewWeapon(currentWeaponInstance);

        remainingAmmo = currentWeapon.ammoCapacity;
        yield return UpdateWeaponsUI(true);

        weaponIsCurrentlyFiring = false;
    }

    IEnumerator UpdateWeaponsUI(bool weaponWasSwapped = false)
    {        
        ammoLevelText.text = remainingAmmo.ToString();

        if (weaponWasSwapped)
            armsWeaponNumber.text = System.Array.IndexOf(weaponList.weapons, currentWeapon).ToString();

        yield return null;
    }


    // The created Game Object instance of a Weapon sets its own conditions on if it actually fires.
    public void Fire()
    {        
        if (weaponIsCurrentlyFiring)
            return;

        if (remainingAmmo <= 0)
            return;
        
        remainingAmmo--;        

        StartCoroutine(StartFireCooldown());
        
        ammoLevelText.text = remainingAmmo.ToString();

        ((IFireable)weaponInstanceFiringScript).Fire(currentWeaponInstance);
    }

    IEnumerator StartFireCooldown()
    {
        weaponIsCurrentlyFiring = true;

        yield return UpdateWeaponsUI();

        float t = 0f;
        while (t < currentWeapon.cooldown)
        {
            Debug.Log("current weapon on cooldown");
            t += Time.deltaTime;
            yield return null;
        }

        remainingAmmo++;

        yield return UpdateWeaponsUI();

        weaponIsCurrentlyFiring = false;

        yield break;
    }



    // Find the weapon's script, regardless of that script's name, that implements IFireable.
    MonoBehaviour FindFiringScriptOfNewWeapon(GameObject weaponInstance)
    {
        MonoBehaviour[] scripts = weaponInstance.GetComponentsInChildren<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script is IFireable)
                return script;
        }

        return null;
    }
}
