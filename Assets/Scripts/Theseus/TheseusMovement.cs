using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TheseusMovement : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private InputAction move;
    private InputAction reload;
    private InputAction wait;
    private InputAction quit;
    private bool isMoving;
    [SerializeField] private LayerMask mazeLayer;

    [SerializeField] private float speed;

    public delegate void FinishedTheseusMovement();
    public static FinishedTheseusMovement finishedMovement;

    public delegate void TheseusWaited();
    public static TheseusWaited theseusWaited;

    public Vector2 nextMovement;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        MinotaurMovement.finishedMovement += UnblockTheseusMovement;
        EndLevelScript.playerReachedExit += DisableMovement;
        MinotaurMovement.minotaurAteTheseus += DisableMovement;

        move = playerInputActions.Player.Move;
        move.Enable();

        move.performed += AddMovement;
        move.performed += StartMovement;

        reload = playerInputActions.Player.Reload;
        reload.performed += ReloadActiveScene;
        reload.Enable();

        wait = playerInputActions.Player.Wait;
        wait.performed += DoWait;
        wait.Enable();

        // quit = playerInputActions.Player.Quit;
        // quit.performed += DoQuit;
        // quit.Enable();
    }


    private void OnDisable()
    {
        MinotaurMovement.finishedMovement -= UnblockTheseusMovement;
        EndLevelScript.playerReachedExit -= DisableMovement;
        MinotaurMovement.minotaurAteTheseus -= DisableMovement;
        move.performed -= AddMovement;


        move.Disable();
        reload.Disable();
        wait.Disable();
        //quit.Disable();
    }


    private void StartMovement(InputAction.CallbackContext obj)
    {

        StartCoroutine(MoveTheseus(move.ReadValue<Vector2>()));
    }

    private void AddMovement(InputAction.CallbackContext obj)
    {
        nextMovement = obj.ReadValue<Vector2>();
        Debug.Log(nextMovement);
    }

    // private void StartSecondMovement(InputAction.CallbackContext obj)
    // {

    // }


    IEnumerator MoveTheseus(Vector2 movement)
    {
        if (isMoving == false)
        {
            nextMovement = Vector2.zero;
            // if (movement == Vector2.zero)
            // {
            //     movement = move.ReadValue<Vector2>();
            // }

            // check if its possible to move to the next tile before move
            RaycastHit2D hit = Physics2D.Raycast(transform.position, movement, 1f, mazeLayer);
            if (hit.collider == null)
            {
                isMoving = true;
                Vector3 targetPos = gameObject.transform.position + (Vector3)movement;

                while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
                {
                    // not doing this movement inside update or fixed update,
                    // because of this I'm not using Time.deltaTime or Time.fixedDeltaTime
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, speed / 60f);
                    yield return new WaitForSeconds(1 / 60f);
                }

                gameObject.transform.position = targetPos;
                finishedMovement?.Invoke();
            }
        }
    }


    private void UnblockTheseusMovement()
    {
        isMoving = false;
        if (nextMovement != Vector2.zero)
        {
            StartCoroutine(MoveTheseus(nextMovement));
        }

        nextMovement = Vector2.zero;
    }

    private void DisableMovement()
    {
        move.Disable();
    }

    void ReloadActiveScene(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void DoWait(InputAction.CallbackContext obj)
    {
        isMoving = true;
        theseusWaited?.Invoke();
    }

    private void DoQuit(InputAction.CallbackContext obj)
    {
        Debug.Log("quit");
        Application.Quit();
    }

}
