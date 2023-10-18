using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(SphereCollider))]
public class Rasho : MonoBehaviour
{
    public Vector3 Direction;

    [SerializeField]
    private float Speed = 4f;
    [SerializeField]
    private float timeToDestroy = 7f;

    private Rigidbody rb;
    private float timer = 0f;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void Start() 
    {
        
    }

    private void Update() 
    {
        rb.velocity = Direction.normalized * Speed;
        
        timer += Time.deltaTime;
        if (timer >= timeToDestroy)
        {
            StoneDestroy();
        }  
    }

    private void StoneDestroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other) 
    {
        // Hay una colision
        // TODO: Falta hacerle danho al jugador
        Debug.Log(other.collider.name);
        if(other.collider.name == "Player")
        {
            Debug.Log("Rayasho");
            PlayerHealth.Instance.TakeDamage(5f);
            StoneDestroy();
        }
    }
}
