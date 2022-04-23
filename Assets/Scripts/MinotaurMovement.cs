using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurMovement : MonoBehaviour
{
    [SerializeField] private GameObject theseus;
    [SerializeField] private float timeToMoveBetweenTiles;

    private int movementCounter;

    public delegate void FinishedMinotaurMovement();
    public static FinishedMinotaurMovement finishedMovement;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        TheseusMovement.finishedMovement += StartMovement;
    }

    private void OnDisable()
    {
        TheseusMovement.finishedMovement -= StartMovement;
    }

    void StartMovement()
    {
        // Debug.Log("Minotaur started walking");

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

        // Debug.Log(distanceToMove);

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

        Vector3 targetPos = gameObject.transform.position + (Vector3)distanceToMove;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            //Debug.Log((targetPos - transform.position).sqrMagnitude);

            // not doing this movement inside update or fixed update,
            // because of this I'm not using Time.deltaTime or Time.fixedDeltaTime
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 1 / (timeToMoveBetweenTiles * 60));
            yield return new WaitForSeconds(1 / (timeToMoveBetweenTiles * 60));
        }

        gameObject.transform.position = targetPos;

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
