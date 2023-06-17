using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberFloorSpawner : MonoBehaviour
{

    [SerializeField] GameObject floorTrapPrefab;
    [SerializeField] float moveSpeed = 1f;    

    GameObject floorTrap;

    Rigidbody bossRigid;

    
    void Awake()
    {
        floorTrap = Instantiate(floorTrapPrefab, transform.position, transform.rotation);
    }

    void FixedUpdate()
    {
         floorTrap.transform.localPosition += transform.forward * moveSpeed * Time.deltaTime;        
    }
}
