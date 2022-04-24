using UnityEngine;

public class EndLevelScript : MonoBehaviour
{
    public delegate void PlayerReachedExit();
    public static PlayerReachedExit playerReachedExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != 3)
        {
            Debug.Log("end");
            playerReachedExit?.Invoke();
        }
    }

}
