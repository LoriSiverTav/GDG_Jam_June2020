using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLvlGen : MonoBehaviour
{
    public Transform[] rowSpawners0;
    public Transform[] rowSpawners1;
    public Transform[] rowSpawners2;
    public Transform[] rowSpawners3;
    public GameObject[] roomTypes;
    public int lvlIndex;
    public GameObject chest;

    // Start is called before the first frame update
    void Start()
    {
        LevelSol levelData = LevelManager.instance.GetLevelData();
        DrawRooms(levelData.solutionPath);

        // Place the player on the starting point
        GameObject player = GameObject.Find("tempPlayer");
        player.transform.position = rowSpawners0[(int)levelData.startPoint.y].position;

        Transform[] row = null;

        switch(levelData.endPoint.x)
        {
            case 0:
                row = rowSpawners0;
                break;
            case 1:
                row = rowSpawners1;
                break;
            case 2:
                row = rowSpawners2;
                break;
            case 3:
                row = rowSpawners3;
                break;
            default:
                break;
        }

        var newChest = Instantiate(chest, row[(int)levelData.endPoint.y].position, Quaternion.Euler(0,0,0));

        // Draw filler rooms
        // Add chest and doors at the start and end points
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddRoom(int row, int col, int room)
    {
        Transform[] targetRow = null;

        switch (row)
        {
            case 0:
                targetRow = rowSpawners0;
                break;
            case 1:
                targetRow = rowSpawners1;
                break;
            case 2:
                targetRow = rowSpawners2;
                break;
            case 3:
                targetRow = rowSpawners3;
                break;
            default:
                break;
        }

        GameObject newRoom = Instantiate(roomTypes[room], targetRow[col].position, transform.rotation);
        targetRow[col] = newRoom.transform;
    }

    private void DrawRooms(int[,] solutionPath)
    {
        for(int i = 0; i < LevelManager.instance.gridSize; i++)
        {
            for(int j = 0; j < LevelManager.instance.gridSize; j++)
            {
                if(solutionPath[i,j] != -1)
                    AddRoom(i, j, solutionPath[i, j]);
            }
        }
    }
}
