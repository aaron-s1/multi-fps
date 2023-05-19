using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] Transform player;

    Vector3 direction;

    void Awake() => player = GameObject.Find("Player").transform.GetChild(0);

    void FixedUpdate()
    {
        direction = transform.position - player.position;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}
