using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public Canvas DeathCanvas;
    public float timerReset = 2f;
    public float deathScreenTimer;
    private bool timerCheck;
    public static int userTries;
    // Start is called before the first frame update
    void Start()
    {
        deathScreenTimer = timerReset;
        timerCheck = false;
        userTries = 3;
    }

    // Update is called once per frame
    void Update()
    {
        deathScreenTimer -= Time.deltaTime;

        if (userTries <= 0)
        {
            if (!timerCheck)
            {
                deathScreenTimer = timerReset;
                timerCheck = true;
            }

            DeathCanvas.enabled = true;

            if (deathScreenTimer <= 0)
            {
                DeathSequence();
            }
        }
    }

    //pulls up a a fail screen for a few seconds 
    public void DeathSequence()
    {
        deathScreenTimer = timerReset;
        userTries = 3;
        timerCheck = false;
        DeathCanvas.enabled = false;
        PlayerMovement.isPuzzling = false;
        SceneManager.LoadScene(2);
    }
}
