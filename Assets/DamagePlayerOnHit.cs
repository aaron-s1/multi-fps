using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePlayerOnHit : MonoBehaviour
{
    [Tooltip("Randomize damage number? If so, enter damage range.")]
    [SerializeField] bool randomizeDamage;
    [SerializeField] int minRandomDamage;
    [SerializeField] int maxRandomDamage;
    
    [SerializeField] bool disableSelfOnHit;

    GameObject player;

    int missileDamageValue;


    void Start()
    {
        player = GameObject.Find("Player").transform.GetChild(0).gameObject;
        
        if (randomizeDamage)
        {
            if (!gameObject.GetComponent<AssignRandomDamageValue>())
                gameObject.AddComponent<AssignRandomDamageValue>();

            // missileDamageValue = GetComponent<AssignRandomDamageValue>().GetRandomValue(randomizationRange);
            GetComponentInChildren<TextMeshPro>().text = missileDamageValue.ToString();

            return;
        }

        missileDamageValue = int.Parse(GetComponentInChildren<TextMeshPro>().text);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            if (disableSelfOnHit)
                transform.parent.gameObject.SetActive(false);

            player.GetComponent<Health>().TakeDamage(missileDamageValue);
        }
    }
}
