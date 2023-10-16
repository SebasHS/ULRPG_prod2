using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 Speed = new Vector3(4f, 0f, 4f);

    private Rigidbody rb;
    private Animator animator;
    private PlayerInput playerInput;

    private Vector2 moveDir;


    public float delay = 0.3f;
    private bool attackBlocked;

    //false si es modo hand y true si es modo shotgun
    private bool attackMode;



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
        animator.SetBool("IsAttacking", false);
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
            animator.SetBool("IsHandAttacking", true);
            if (attackBlocked)
            {
                return;
            }
            attackBlocked=true;
            StartCoroutine(DelayHandAttack());
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
    }
    

}
