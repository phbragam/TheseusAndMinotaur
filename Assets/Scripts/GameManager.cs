using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
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
        Invoke("LoadNextScene", 2f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void ReloadLevel()
    {
        Invoke("ReloadActiveScene", 2f);
    }

    void ReloadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
