using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private Transform oldTransform;
    private Health unitHealth;
    private int speed = 15;

    private void Start()
    {
        unitHealth = GetComponent<Health>();
        oldTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        ChangeFillOrigin();
        float health = unitHealth.HealthValue();
        float maxHealth = unitHealth.MaxHealthValue();

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, Time.deltaTime * speed);    
    }

    private void ChangeFillOrigin()
    {
        if(oldTransform.localScale.x < 0 && gameObject.CompareTag("Enemy"))
        {
            healthBar.fillOrigin = 1;
        }
        else if (oldTransform.localScale.x > 0 && gameObject.CompareTag("Enemy"))
        {
            healthBar.fillOrigin = 0;
        }
    }
}
