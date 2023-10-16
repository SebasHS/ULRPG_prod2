using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
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
