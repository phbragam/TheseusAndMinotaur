using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float timeToNextLevel;
    [SerializeField] private float timeToReloadLevel;

    void OnEnable()
    {
        EndLevelScript.playerReachedExit += NextLevel;
        MinotaurMovement.minotaurAteTheseus += ReloadLevel;
    }

    void OnDisable()
    {
        EndLevelScript.playerReachedExit -= NextLevel;
        MinotaurMovement.minotaurAteTheseus -= ReloadLevel;
    }

    void NextLevel()
    {
        Invoke("LoadNextScene", timeToNextLevel);
    }

    void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().name != "LevelCredits")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Application.Quit();
        }
    }

    void ReloadLevel()
    {
        Invoke("ReloadActiveScene", timeToReloadLevel);
    }

    void ReloadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
