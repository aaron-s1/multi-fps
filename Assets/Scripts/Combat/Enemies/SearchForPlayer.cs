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
    [SerializeField] Vector3 playerPosOffset;
    [SerializeField] float viewDistance;
    [SerializeField] float viewAngle;


    List<GameObject> bulletPool;

    bool canFire = true;



    void Start() =>
        CreateNewBulletPool();
    
    void FixedUpdate() =>
        FindPlayer();


    
    

    void FindPlayer()
    {
        if (!canFire)
            return;

        canFire = false;

        // playerPos.position = playerPos.position + new Vector3(playerPosOffset.x, playerPosOffset.y, playerPosOffset.z);

        if (Vector3.Distance(transform.position, playerPos.position) < viewDistance)
        {
            Vector3 directionToPlayer = (playerPos.position - transform.position).normalized;
            float angleBetweenSelfAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleBetweenSelfAndPlayer < viewAngle / 2)
            {
                if (Physics.Linecast(transform.position, playerPos.position))
                    StartCoroutine(StartFiring());
            }
        }
    }


    IEnumerator StartFiring()
    {
        canFire = false;

        GameObject bullet = GetBulletFromPool();


        if (bullet != null)
        {
            var bulletTextPosition = bullet.transform.GetChild(0).position;
            bullet.transform.position = transform.position;
            bullet.transform.GetChild(0).position = transform.position;

            bullet.SetActive(true);
        }

        else
            Debug.Log("Bullet pool returned no bullet.");


        yield return new WaitForSeconds(fireRate);
        
        canFire = true;
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
}
