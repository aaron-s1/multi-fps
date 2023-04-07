using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AssignRandomDamageValue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageNumberText;

    int randomNumber;
    
    void Awake()
    {
        randomNumber = Random.Range(1, 9);
        damageNumberText.text = randomNumber.ToString();         
    }

    public int GetValue()
    {
        return randomNumber;
    }
}
