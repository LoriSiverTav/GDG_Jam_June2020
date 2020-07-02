using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public Canvas PauseCanvas;
    public Canvas ControlsCanvas;
    public Canvas InventoryCanvas;
    public Canvas DoubleCheckCanvas;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checks for pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (Time.timeScale == 1)
            {
                PauseGame();
            }
            //resumes
            else
            {
                ResumeGame();
            }
        }
    }

    //Restores time scale
    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseCanvas.enabled = false;
        ControlsCanvas.enabled = false;
        InventoryCanvas.enabled = false;
        DoubleCheckCanvas.enabled = false;
    }

    //Stops Time scale
    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseCanvas.enabled = true;
    }

    public void InventoryButtonPressed()
    {
        PauseCanvas.enabled = false;
        InventoryCanvas.enabled = true;
    }

    public void ControlsButtonPressed()
    {
        PauseCanvas.enabled = false;
        ControlsCanvas.enabled = true;
    }

    public void BackButtonPressed()
    {
        PauseCanvas.enabled = true;
        ControlsCanvas.enabled = false;
        InventoryCanvas.enabled = false;
        DoubleCheckCanvas.enabled = false;
    }

    public void ExitCheck()
    {
        PauseCanvas.enabled = false;
        DoubleCheckCanvas.enabled = true;
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
