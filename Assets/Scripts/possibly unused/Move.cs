using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Move : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 playerPosAtInstantiation;
    [SerializeField] float playerPos_Y_Offset;
    [SerializeField] float moveSpeed;

    [SerializeField] int missileDamageValue;

    float distanceToDestination;

    
    void Awake()
    {
        player = GameObject.Find("Player").transform.GetChild(0).gameObject;
        playerPosAtInstantiation = player.transform.position;

        playerPosAtInstantiation = new Vector3 (playerPosAtInstantiation.x,
                                                playerPosAtInstantiation.y + playerPos_Y_Offset,
                                                playerPosAtInstantiation.z);
    }

    void Start()
    {
        missileDamageValue = GetComponent<AssignRandomDamageValue>().GetValue();
    }

    void FixedUpdate()
    {
        var step =  moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, playerPosAtInstantiation, step);
        // transform.position += transform.forward * step;

        
        distanceToDestination = Vector3.Distance(transform.position, playerPosAtInstantiation);

        if (distanceToDestination < 0.04f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("hit player");
            player.GetComponent<Health>().TakeDamage(missileDamageValue);
        }
    }
}
