using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TheseusMovement : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private InputAction move;
    private bool isMoving;
    [SerializeField] private float timeToMoveBetweenTiles;


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerInputActions.Player.Move;
        move.Enable();

        move.performed += StartMovement;
    }

    private void StartMovement(InputAction.CallbackContext obj)
    {
        StartCoroutine("MovePlayer");
    }


    IEnumerator MovePlayer()
    {
        if (isMoving == false)
        {
            isMoving = true;
            Vector3 targetPos = gameObject.transform.position + (Vector3)move.ReadValue<Vector2>();

            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                //Debug.Log((targetPos - transform.position).sqrMagnitude);

                // not doing this movement inside update or fixed update,
                // because of this I'm not using Time.deltaTime or Time.fixedDeltaTime
                transform.position = Vector3.MoveTowards(transform.position, targetPos, 1 / (timeToMoveBetweenTiles * 60));
                yield return new WaitForSeconds(1 / (timeToMoveBetweenTiles * 60));
            }

            gameObject.transform.position = targetPos;
            isMoving = false;
        }
    }


    private void OnDisable()
    {
        move.Disable();
    }

}
