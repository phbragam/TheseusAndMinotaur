using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TheseusMovement : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private InputAction move;


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerInputActions.Player.Move;
        move.Enable();

        move.performed += MovePlayer;
    }

    private void MovePlayer(InputAction.CallbackContext obj)
    {
        Debug.Log("Movement Values " + move.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        move.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // IEnumerator TileMovement(){

    // }



    public void MovementTest()
    {
        Debug.Log("movement");
        Debug.Log("Movement Values " + move.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        // if (move.ReadValue<Vector2>().x != 0 || move.ReadValue<Vector2>().y != 0)
        //     Debug.Log("Movement Values " + move.ReadValue<Vector2>());
    }
}
