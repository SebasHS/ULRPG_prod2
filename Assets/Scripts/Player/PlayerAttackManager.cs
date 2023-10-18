using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackManager : MonoBehaviour
{
    public GameObject hand;
    public GameObject shotgun;
    private bool isHandActive = true;
    private PlayerInput playerInput;
    public PlayerMovement playerMovement;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    // Start is called before the first frame update
    void Start()
    {
        hand.SetActive(isHandActive);
        shotgun.SetActive(!isHandActive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnChangeWeapon(InputValue value)
    {
        Debug.Log("Change weapon");
        isHandActive = !isHandActive;
        // Activa o desactiva los elementos según el estado actual
        hand.SetActive(isHandActive);
        shotgun.SetActive(!isHandActive);

        if (!isHandActive)
        {
            // Actualizar la variable attackMode de PlayerMovement
            playerMovement.UpdateAttackMode(true); // Donde playerMovementScript es una referencia al script PlayerMovement
        }else{
            playerMovement.UpdateAttackMode(false);
        }
    }
}
