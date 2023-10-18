    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 Speed = new Vector3(4f, 0f, 4f);
    [SerializeField]
    private float jumpForce = 5.0f;

    private Rigidbody rb;
    private Animator animator;
    private PlayerInput playerInput;

    private Vector3 moveDir;


    public float delay = 0.1f;
    private bool attackBlocked;

    //false si es modo hand y true si es modo shotgun
    private bool attackMode;


    public GameObject bulletPrefab;
    public Transform firePoint; // Este es el punto desde donde se dispara el proyectil.
    private Vector3 bulletDirection;


    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        
    }

    private void Start() 
    {
        DialogueManager.Instance.OnDialogueStart += OnDialogueStartDelegate;
        DialogueManager.Instance.OnDialogueFinish += OnDialogueFinishDelegate;
        animator.SetBool("IsHandAttacking", false);
        attackMode = false;
        
    }

    private void Update() 
    {
        moveDir.Normalize();
        
        rb.velocity = new Vector3(
            moveDir.x * Speed.x,
            rb.velocity.y,
            moveDir.y * Speed.z
        );

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

    }

    public void OnDialogueStartDelegate(Interaction interaction)
    {
        // Cambiar el Input Map al modo Dialogue
        playerInput.SwitchCurrentActionMap("Dialogue");
    }

    public void OnDialogueFinishDelegate()
    {
        // Cambiar el Input Map al modo Player
        playerInput.SwitchCurrentActionMap("Player");
    }

    private void OnMovement(InputValue value)
    {
        moveDir = value.Get<Vector2>();
        if (Mathf.Abs(moveDir.x) > Mathf.Epsilon || 
            Mathf.Abs(moveDir.y) > Mathf.Epsilon)
        {
            animator.SetBool("IsWalking", true);
            animator.SetFloat("Horizontal", moveDir.x);
            animator.SetFloat("Vertical", moveDir.y);
        }else
        {
            animator.SetBool("IsWalking", false);
        }
        if(moveDir.x != 0 || moveDir.y != 0)
        {
            bulletDirection = new Vector3(moveDir.x, 0, moveDir.y);
        
        }

       
        //bulletDirection = new Vector2(moveDir.x*2, moveDir.y*2);
        
    }

    private void OnNextInteraction(InputValue value)
    {
        if (value.isPressed)
        {
            // Siguiente Dialogo
            DialogueManager.Instance.NextDialogue();
        }
    }


    private void OnCollisionEnter(Collision other) 
    {
        Dialogue dialogue = other.collider.transform.GetComponent<Dialogue>();
        if (dialogue != null)
        {
            // Iniciar Sistema de Dialogos
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }


    public void OnAttack()
    {
        //If modo hand
        if (!attackMode)
        {
            OnHandAttack();
        }
        else //If modo shotgun
        {
            OnShotgunAttack();
        }
        
    }

    private void OnShotgunAttack()
    {
        // Crea el proyectil en la posición del firePoint y la dirección en la que el jugador está mirando.
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + new Vector3(0, 0.314f, 0), transform.rotation);

        // Actualiza el parámetro del animator del objeto bulletPrefab
        Animator bulletAnimator = bullet.GetComponent<Animator>();
        
        if (bulletDirection.z != 0)
        {
            bullet.transform.Rotate(new Vector3(0, 0, 90));
        }    
        bullet.GetComponent<Rigidbody>().AddForce(bulletDirection * 700f);

        // Destruye el proyectil después de un tiempo o cuando colisiona con algo.
        Destroy(bullet, 2f); // Cambia 2f por la duración deseada del proyectil.
    }

    private void OnHandAttack()
    {
        float handDamageAmount = 1f;
        animator.SetBool("IsHandAttacking", true);
        if (attackBlocked)
        {
            return;
        }
        attackBlocked=true;
        StartCoroutine(DelayHandAttack());
        //danio cuando colisiona

        // Detecta las colisiones con objetos etiquetados como "Enemy" durante el ataque.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f); // Ajusta el radio según tus necesidades.

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Boss"))
            {
                // Acción para dañar al enemigo.
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.Damage(handDamageAmount); // Reemplaza "tuCantidadDeDaño" por el valor adecuado.
                }
            }
        }

    }

    private IEnumerator DelayHandAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
        animator.SetBool("IsHandAttacking", false);
    }

    public void UpdateAttackMode(bool newAttackMode)
    {
        attackMode = newAttackMode;
        animator.SetBool("IsShotgunMode", newAttackMode);
    }


    void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector3(0, jumpForce, 0);
        }
    }
    bool IsGrounded()
    {
        // Verificar si el objeto está en contacto con el suelo
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }
}
