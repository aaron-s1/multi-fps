using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForPlayer : MonoBehaviour
{
    [SerializeField] GameObject numberBulletPrefab;
    [SerializeField] float fireRate = 1f;
    [SerializeField] int bulletPoolSize = 10;

    [Space(10)]
    [SerializeField] Transform playerPos;
    [SerializeField] float viewDistance;
    [SerializeField] float viewAngle;


    List<GameObject> bulletPool;

    bool canFire = true;



    void Start() =>
        CreateNewBulletPool();
    
    void Update() =>
        FindPlayer();

    
    void FindPlayer()
    {
        if (!canFire)
            return;

        if (Vector3.Distance(transform.position, playerPos.position) < viewDistance)
        {
            Vector3 directionToPlayer = (playerPos.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleBetweenGuardAndPlayer < viewAngle / 2)
            {
                if (Physics.Linecast(transform.position, playerPos.position))
                    StartCoroutine(StartFiring());
            }
        }
    }


    IEnumerator StartFiring()
    {
        canFire = false;

        GameObject bullet = Instantiate(GetBulletFromPool());
        bullet.transform.position = transform.position;
        bullet.SetActive(true);

        yield return new WaitForSeconds(fireRate);

        canFire = true;

        yield break;
    }


    void CreateNewBulletPool()
    {
        bulletPool = new List<GameObject>();

        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(numberBulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    GameObject GetBulletFromPool()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
                return bulletPool[i];
        }
        return null;
    }
}
