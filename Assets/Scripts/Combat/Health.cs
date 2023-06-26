using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] TextMeshProUGUI ratingText;      // "rating" = health

    void Awake() =>
        UpdateHealthText();


    public void TakeDamage(float damage)
    {
        Debug.Log("player took damage");
        var averagedPostDamageHealth = (health + damage) / 2f;
        var roundedHealth = (Mathf.Round(averagedPostDamageHealth * 100)) / 100.0f;
        
        health = roundedHealth;

        UpdateHealthText();
    }
    

    void UpdateHealthText() =>
        ratingText.text = health.ToString();
}