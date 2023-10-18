using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CapsuleCollider))]
public class BossController : MonoBehaviour
{
    #region States
    public IdleStateBoss IdleStateBoss;
    public FollowStateBoss FollowStateBoss;
    private StateBoss currentState;
    #endregion

    #region Parameters
    public Transform Player;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 3f;
    public float Speed = 1f;
    public GameObject prefabRasho;
    public Transform FirePoint;
    public float CoolDownTime = 1.0f;
    private float timer = 0f;
    private bool timerReached = false;
    #endregion

    #region Readonly Properties
    public Rigidbody rb { private set; get; }
    public Animator animator { private set; get; }
    #endregion


    public static BossController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        IdleStateBoss = new IdleStateBoss(this);
        FollowStateBoss = new FollowStateBoss(this);

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Seteamos el estado inicial
        currentState = IdleStateBoss;
    }

    private void Start()
    {
        currentState.OnStart();
    }

    private void Update()
    {
        foreach (var transition in currentState.Transitions)
        {
            if (transition.IsValid())
            {
                // Ejecutar Transicion
                currentState.OnFinish();
                currentState = transition.GetNextState();
                currentState.OnStart();
                break;
            }
        }
        currentState.OnUpdate();

    }

    public void Fire()
    {
        UpdateAttackMode();
        GameObject rasho = Instantiate(prefabRasho, FirePoint.position, Quaternion.identity);
        rasho.GetComponent<Rasho>().Direction =
            Player.position - transform.position;
        Debug.Log("El Rasho");
    }

    public void UpdateAttackMode()
    {
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsWalking", false);
    }

    public void UpdateFollowMode()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    public void UpdateIdleMode()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", false);
    }

    public void BossDies()
    {
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("Dies", true);
        Destroy(gameObject);
    }
    /*public void Attack()
    {

    }*/

}