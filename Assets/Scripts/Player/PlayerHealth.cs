using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }
    [SerializeField] public GameObject HealthBarPlayer;
    public float health = 100;
    public Slider slider;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        HealthBarPlayer = GameObject.Find("HealthBarPlayer");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        slider.value = health;
        if (health <= 0)
        {
            Debug.Log("Te moriste");
            Destroy(gameObject);
        }
    }
}