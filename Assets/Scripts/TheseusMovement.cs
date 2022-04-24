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
    [SerializeField] private LayerMask mazeLayer;


    [SerializeField] private float timeToMoveBetweenTiles;

    public delegate void FinishedTheseusMovement();
    public static FinishedTheseusMovement finishedMovement;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        MinotaurMovement.finishedMovement += UnblockTheseusMovement;
        EndLevelScript.playerReachedExit += DisableMovement;

        move = playerInputActions.Player.Move;
        move.Enable();

        move.performed += StartMovement;
    }


    private void OnDisable()
    {
        MinotaurMovement.finishedMovement -= UnblockTheseusMovement;
        EndLevelScript.playerReachedExit -= DisableMovement;

        move.Disable();
    }


    private void StartMovement(InputAction.CallbackContext obj)
    {
        StartCoroutine("MoveTheseus");
    }


    IEnumerator MoveTheseus()
    {
        if (isMoving == false)
        {
            Vector2 moveDirection = move.ReadValue<Vector2>();
            // check if its possible to move to the next tile before move
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 1f, mazeLayer);
            if (hit.collider == null)
            {
                isMoving = true;
                Vector3 targetPos = gameObject.transform.position + (Vector3)moveDirection;

                while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
                {
                    // not doing this movement inside update or fixed update,
                    // because of this I'm not using Time.deltaTime or Time.fixedDeltaTime
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, 1 / (timeToMoveBetweenTiles * 60));
                    yield return new WaitForSeconds(1 / (timeToMoveBetweenTiles * 60));
                }

                gameObject.transform.position = targetPos;
                finishedMovement?.Invoke();
            }
        }
    }

    private void UnblockTheseusMovement()
    {
        isMoving = false;
    }

    private void DisableMovement()
    {
        move.Disable();
    }
}
