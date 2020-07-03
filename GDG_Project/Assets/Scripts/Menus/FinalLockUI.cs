using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum Direction { up, down, left, right };

public class FinalLockUI : MonoBehaviour
{
    public List<int> playerInput;
    public GameObject[] slots;
    public int targetDial = 0;

    public Color highlightColor;
    

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            playerInput.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerMovement.isPuzzling)
        {
            return;
        }

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].GetComponent<Image>().color = targetDial == i ? highlightColor : Color.white;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveDialVertically(Direction.up);
        }
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDialVertically(Direction.down);
        }
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveDialHorizontally(Direction.left);
        }
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveDialHorizontally(Direction.right);
        }

        if((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && IsFinalSolutionCorrect())
        {
            SceneManager.LoadScene(3);
        }
    }

    void MoveDialVertically(Direction direction)
    {
        if(direction == Direction.up)
        {
            int nextNum = playerInput[targetDial] + 1 < 4 ? playerInput[targetDial] + 1 : 0;
            playerInput[targetDial] = nextNum;
            UpdateSlots();
        }
        else
        {
            int nextNum = playerInput[targetDial] - 1 < 0 ? 3 : playerInput[targetDial] - 1;
            playerInput[targetDial] = nextNum;
            UpdateSlots();
        }
    }

    void MoveDialHorizontally(Direction direction)
    {
        if (direction == Direction.left)
        {
            targetDial = targetDial - 1 < 0 ? 3 : targetDial - 1;
        }
        else
        {
            targetDial = targetDial + 1 < 4 ? targetDial + 1 : 0;
        }
    }

    bool IsFinalSolutionCorrect()
    {
        int[] solution = LevelManager.instance.finalSolution.finalSolutionCombo;

        for(int i = 0; i < 4; i++)
        {
            if(playerInput[i] != solution[i])
            {
                return false;
            }
        }

        return true;
    }

    void UpdateSlots()
    {
        for(int i = 0; i < playerInput.Count; i++)
        {
            slots[i].GetComponent<Image>().sprite = LevelManager.instance.finalSolution.allShapes[playerInput[i]].itemShape.itemSprite;
        }
    }
}
