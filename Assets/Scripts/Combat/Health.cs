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
        UpdateHealth();


    public void TakeDamage(float damage)
    {
        var averagedPostDamageHealth = (health + damage) / 2f;
        var roundedHealth = (Mathf.Round(averagedPostDamageHealth * 100)) / 100.0f;
        
        health = roundedHealth;

        UpdateHealth();
    }

    void UpdateHealth() =>
        ratingText.text = health.ToString();
}
