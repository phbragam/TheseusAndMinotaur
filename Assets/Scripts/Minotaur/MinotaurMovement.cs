using System.Collections;
using UnityEngine;

public class MinotaurMovement : MonoBehaviour
{
    [SerializeField] private GameObject theseus;
    [SerializeField] private float speed;

    private int movementCounter;

    public delegate void FinishedMinotaurMovement();
    public static FinishedMinotaurMovement finishedMovement;

    public delegate void MinotaurAteTheseus();
    public static MinotaurAteTheseus minotaurAteTheseus;

    private void Awake()
    {
        theseus = GameObject.FindObjectOfType<TheseusMovement>().gameObject;
    }
    private void OnEnable()
    {
        TheseusMovement.finishedMovement += StartMovement;
        TheseusMovement.theseusWaited += StartMovement;
    }

    private void OnDisable()
    {
        TheseusMovement.finishedMovement -= StartMovement;
        TheseusMovement.theseusWaited -= StartMovement;
    }

    void StartMovement()
    {
        StartCoroutine("MoveMinotaur");
    }

    IEnumerator MoveMinotaur()
    {
        Vector2 distanceToMove = Vector2.zero;

        // horizontal movement first
        distanceToMove.x = theseus.transform.position.x - transform.position.x;
        if (distanceToMove.x > 0)
        {
            distanceToMove.x = 1;
        }
        else if (distanceToMove.x < 0)
        {
            distanceToMove.x = -1;
        }

        // check if there is a wall blocking the horizontal movement
        if (distanceToMove.x != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, distanceToMove, 1f);
            if (hit.collider != null)
            {
                distanceToMove.x = 0;
            }
        }

        // try vertical movement if can't horizontal movement
        if (distanceToMove.x == 0)
        {
            distanceToMove.y = theseus.transform.position.y - transform.position.y;
            if (distanceToMove.y > 0)
            {
                distanceToMove.y = 1;
            }
            else if (distanceToMove.y < 0)
            {
                distanceToMove.y = -1;
            }
        }

        // check if there is a wall blocking the vertical movement
        if (distanceToMove.y != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, distanceToMove, 1f);
            if (hit.collider != null)
            {
                distanceToMove.y = 0;
            }
        }

        Vector3 targetPos = gameObject.transform.position + (Vector3)distanceToMove;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // not doing this movement inside update or fixed update,
            // because of this I'm not using Time.deltaTime or Time.fixedDeltaTime
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed / 60f);
            yield return new WaitForSeconds(1 / 60f);
        }

        gameObject.transform.position = targetPos;

        if ((gameObject.transform.position - theseus.transform.position).sqrMagnitude < Mathf.Epsilon)
        {
            minotaurAteTheseus?.Invoke();
        }

        if (movementCounter == 0)
        {
            movementCounter++;
            StartCoroutine("MoveMinotaur");
        }
        else
        {
            movementCounter = 0;
            finishedMovement?.Invoke();
        }
    }
}

