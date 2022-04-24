using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TheseusMovement : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private InputAction move;
    private InputAction reload;
    private InputAction wait;
    private bool isMoving;
    [SerializeField] private LayerMask mazeLayer;


    [SerializeField] private float timeToMoveBetweenTiles;

    public delegate void FinishedTheseusMovement();
    public static FinishedTheseusMovement finishedMovement;

    public delegate void TheseusWaited();
    public static TheseusWaited theseusWaited;

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

        move.performed += StartMovement;

        reload = playerInputActions.Player.Reload;
        reload.performed += ReloadActiveScene;
        reload.Enable();

        wait = playerInputActions.Player.Wait;
        wait.performed += DoWait;
        wait.Enable();
    }



    private void OnDisable()
    {
        MinotaurMovement.finishedMovement -= UnblockTheseusMovement;
        EndLevelScript.playerReachedExit -= DisableMovement;
        MinotaurMovement.minotaurAteTheseus -= DisableMovement;


        move.Disable();
        reload.Disable();
        wait.Disable();
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

    void ReloadActiveScene(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void DoWait(InputAction.CallbackContext obj)
    {
        isMoving = true;
        theseusWaited?.Invoke();
    }
}
