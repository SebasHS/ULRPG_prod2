using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 25f;
    [SerializeField] public GameObject HealthBarBoss;
    private float currentHealth;
    public Slider slider;

    void Start()
    {
        currentHealth = maxHealth;   
        HealthBarBoss = GameObject.Find("HealthBarBoss");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(currentHealth);
        slider.value = currentHealth; 
        if (currentHealth <= 0)
        {
            Die();

        }
        void Die()
        {
            Destroy(gameObject);
        }
    }
}
