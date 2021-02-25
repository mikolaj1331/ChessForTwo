using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    [SerializeField] GameObject popUpMenu;
    SceneLoader loader;

    public bool gameIsPaused = false;
    private void Start()
    {
        loader = GetComponent<SceneLoader>();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameIsPaused = true;
        popUpMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
        popUpMenu.SetActive(false);
    } 
}
