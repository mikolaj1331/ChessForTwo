using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public bool gameIsPaused = false;

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
    } 

    public void OpenWindow(GameObject go)
    {
        go.SetActive(true);
    }

    public void CloseWindow(GameObject go)
    {
        go.SetActive(false);
    }

    public void ChangeText(TextMeshProUGUI textBox, string text)
    {
        textBox.text = text;
    }
}
