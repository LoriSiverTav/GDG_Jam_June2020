using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorComp : MonoBehaviour
{
    public int sceneIdxToGo = 1;
    public int levelDataIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            LevelManager.instance.lvlIndex = levelDataIndex;
            SceneManager.LoadScene(sceneIdxToGo);
        }
    }
}
