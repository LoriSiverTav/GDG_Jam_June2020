using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    public Canvas MainCanvas;
    public Canvas ControlsCanvas;

    public void StartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ControlsButtonPressed()
    {
        MainCanvas.enabled = false;
        ControlsCanvas.enabled = true;
    }

    public void BackButtonPressed()
    {
        MainCanvas.enabled = true;
        ControlsCanvas.enabled = false;
    }

    public void ExitButtonPressed()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		        Application.Quit();
#endif
    }
}
