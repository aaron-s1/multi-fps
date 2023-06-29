using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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
        SetDamage();
    }

    void SetDamage()
    {
        TextMeshProUGUI textDamage;

        try
        {
            textDamage = GetComponent<TextMeshProUGUI>();
        }
        catch (System.Exception)
        {
            textDamage = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (randomizeDamage)
        {
            missileDamageValue = UnityEngine.Random.Range(minRandomDamage, maxRandomDamage);
            textDamage.text = missileDamageValue.ToString();
        }
        else missileDamageValue = int.Parse(textDamage.text);
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
