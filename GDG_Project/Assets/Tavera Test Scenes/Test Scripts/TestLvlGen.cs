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

    // Start is called before the first frame update
    void Start()
    {
        int[,] solutionPath = LevelManager.instance.GetSolutionPath();
        DrawRooms(solutionPath);
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
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                if(solutionPath[i,j] != -1)
                    AddRoom(i, j, solutionPath[i, j]);
            }
        }
    }

    static public int[,] GenerateLvlSolution()
    {
        int[,] solutionPath = new int[4, 4];

        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                solutionPath[i, j] = -1;

        int downCounter = 0;

        // Generate a room for the starting room
        int initialRndCol = Random.Range(0, 4);
        solutionPath[0, initialRndCol] = Random.Range(1, 3);
        
        int myLastRow = 0;
        int myLastCol = initialRndCol;

        for(; ; )
        {
            int rdmDirection = Random.Range(1, 6);

            if(rdmDirection == 1 || rdmDirection == 2)
            {
                // Try to go left
                if(IsPathOk(solutionPath, myLastRow, myLastCol - 1))
                {
                    solutionPath[myLastRow, myLastCol - 1] = 1;
                    myLastCol--;
                    downCounter = 0;
                }
                else
                {
                    rdmDirection = 5;
                }
            }
            else if(rdmDirection == 3 || rdmDirection == 4)
            {
                // Try to go right
                if(IsPathOk(solutionPath, myLastRow, myLastCol + 1))
                {
                    solutionPath[myLastRow, myLastCol + 1] = 1;
                    myLastCol++;
                    downCounter = 0;
                }
                else
                {
                    rdmDirection = 5;
                }
            }
            else if(rdmDirection == 5)
            {
                downCounter++;

                // Update the previous room to have an opening going downwards
                int roomToSpawn = downCounter >= 2 ? 4 : 2;
                solutionPath[myLastRow, myLastCol] = roomToSpawn;

                // Generate a room with an upward opening
                if(IsPathOk(solutionPath, myLastRow + 1, myLastCol))
                {
                    solutionPath[myLastRow + 1, myLastCol] = 3;
                    myLastRow++;
                }
                else
                {
                    break;
                }
            }
        }
        
        return solutionPath;
    }

    static public bool IsPathOk(int[,] solutionPath, int row, int col)
    {
        if(row < 0 || row >= 4 || col < 0 || col >= 4) { return false; }

        return solutionPath[row, col] == -1;
    }
}
