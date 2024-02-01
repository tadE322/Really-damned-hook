using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fill : MonoBehaviour
{
    private Health health;
    private Image healthBarFill;

    private void Start()
    {
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
        healthBarFill = GetComponent<Image>();
    }

    private void Update()
    {
        if(health.HealthValue() == 0.0f)
        {
            healthBarFill.fillAmount = 0;
        }
    }
}
