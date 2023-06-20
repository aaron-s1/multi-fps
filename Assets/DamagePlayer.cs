using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePlayer : MonoBehaviour
{
    GameObject player;
    float textValue;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform.GetChild(0).gameObject;
        textValue = int.Parse(GetComponent<TextMeshPro>().text);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log(gameObject + " did " + textValue + " to " + player);
            player.GetComponent<Health>().TakeDamage(textValue);
        }
    }
}
