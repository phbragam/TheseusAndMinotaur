using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] GameObject winText;
    [SerializeField] GameObject looseText;

    void Awake()
    {
        winText.SetActive(false);
        looseText.SetActive(false);
    }

    void OnEnable()
    {
        EndLevelScript.playerReachedExit += ShowWinText;
        MinotaurMovement.minotaurAteTheseus += ShowLooseText;
    }

    void OnDisable()
    {
        EndLevelScript.playerReachedExit -= ShowWinText;
        MinotaurMovement.minotaurAteTheseus -= ShowLooseText;
    }

    void ShowWinText()
    {
        winText.SetActive(true);
        looseText.SetActive(false);
    }

    void ShowLooseText()
    {
        winText.SetActive(false);
        looseText.SetActive(true);
    }
}
