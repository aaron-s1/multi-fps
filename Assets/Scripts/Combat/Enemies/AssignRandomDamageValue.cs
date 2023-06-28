using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AssignRandomDamageValue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageNumberText;

    int randomNumber;
    

    public int GetRandomValue(int minRange, int maxRange)
    {
        randomNumber = Random.Range(minRange, maxRange);
        damageNumberText.text = randomNumber.ToString(); 
        return randomNumber;
    }
}
