using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForPlayer : MonoBehaviour
{
    [SerializeField] GameObject numberMissile;
    [SerializeField] Transform playerPos;
    [SerializeField] float viewDistance;
    [SerializeField] float viewAngle;

    Coroutine fireMissile;
    bool canFire = true;
    bool firingAlreadyPrimed;
    float fireRate = 1f;


    void Start()
    {
        // Prime firing.
        StartCoroutine(StartFiring());
    }
    
    void Update()
    {
        Search();
        //     Fire();
        // else
        //     Debug.Log("player not found");
    }

    // void Fire()
    // {
    //     Debug.Log("player found");
    //     Instantiate(numberMissile, transform.position, transform.rotation);
    // }

    void Search()
     {
        if (Vector3.Distance(transform.position, playerPos.position) < viewDistance)
        {
            Vector3 directionToPlayer = (playerPos.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2)
            {
                // canFire = true;
                
                // Debug.Log("viewAngle " + viewAngle);
                if (Physics.Linecast(transform.position, playerPos.position))
                {
                    if (canFire)
                        StartCoroutine(StartFiring());
                }
            }
        }

        // canFire = false;
        // return false;
     }

    IEnumerator StartFiring()
    {
        // Debug.Log("StartFiring()");
        // if (canFire)
        // {
            // Debug.Log("StartF/iring() is firing");
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        Instantiate(numberMissile, transform.position, transform.rotation);
        
        // }
        canFire = true;

        Debug.Log("StartFiring() returned");
        yield return null;        
    }
}
