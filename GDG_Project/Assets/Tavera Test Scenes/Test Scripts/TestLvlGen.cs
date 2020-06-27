using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLvlGen : MonoBehaviour
{
    private enum roomDirection { start, sides, downOnce, nonPath};

    public Transform[] rowSpawners0;
    public Transform[] rowSpawners1;
    public Transform[] rowSpawners2;
    public Transform[] rowSpawners3;

    public GameObject[] roomTypes;

    private int[,] solutionPath = new int[4,4];
    int lastUsedRow;
    int lastUsedCol;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                solutionPath[i, j]= -1;
                Debug.Log("Row: " + i + " Col: " + j);
            }


        int randomStartCol = Random.Range(0, 4);
        GenerateRoom(0, randomStartCol, roomDirection.start);
        lastUsedRow = 0;
        lastUsedCol = randomStartCol;

        GenerateSolutionPath();
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    private void ReplaceRoom(int row, int col, int newRoom)
    {
        Transform[] targetRow = null;

        switch(row)
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

        if(targetRow[col].gameObject.tag != "EmptyRoom")
        {
            Destroy(targetRow[col].gameObject);
            AddRoom(row, col, newRoom);
        }
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

    private void GenerateRoom(int row, int col, roomDirection direction)
    {
        /*
         * 0 = Not part of solution path - no entrances/exits
         * 1 = Has exits to the right and left
         * 2 = Has exits to the right, left, and downwards
         * 3 = Has exits to the right, left, and upwards 
         * 4 = All 4 sides are open
        */

        int room = 1;

        switch(direction)
        {
            case roomDirection.start:
                room = Random.Range(1, 3);
                break;
            case roomDirection.sides:
                room = 1;
                break;
            case roomDirection.downOnce:
                room = 3;
                break;
        }

        AddRoom(row, col, room);
    }

    private bool IsSpotOk(int row, int col)
    {
        if(row < 0 || row >= 4) { return false; }
        if(col < 0 || col >= 4) { return false; }

        Transform[] targetRow = null;

        switch(row)
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

        return (targetRow[col].gameObject.tag == "EmptyRoom");
    }
    
    public void GenerateSolutionPath()
    {
        int downCounter = 0;

        for(; ; )
        {
            int randomDir = Random.Range(1, 6);

            if(randomDir == 1 || randomDir == 2)
            {
                // Try to go Left
                if(IsSpotOk(lastUsedRow, lastUsedCol-1))
                {
                    GenerateRoom(lastUsedRow, lastUsedCol - 1, roomDirection.sides);
                    lastUsedCol--;
                    downCounter = 0;
                }
                else
                {
                    randomDir = 5;
                }
            }
            else if(randomDir == 3 || randomDir == 4)
            {
                // Try to go Right
                if(IsSpotOk(lastUsedRow, lastUsedCol+1))
                {
                    GenerateRoom(lastUsedRow, lastUsedCol + 1, roomDirection.sides);
                    lastUsedCol++;
                    downCounter = 0;
                }
                else
                {
                    randomDir = 5;
                }
            }
            else if(randomDir == 5)
            {
                // Go down
                downCounter++;

                // Update the previous room to have an opening going downwards
                int roomToSpawn = downCounter >= 2 ? 4 : 2;
                ReplaceRoom(lastUsedRow, lastUsedCol, roomToSpawn);
                
                // Create the new room
                if(IsSpotOk(lastUsedRow + 1, lastUsedCol))
                {
                    GenerateRoom(lastUsedRow + 1, lastUsedCol, roomDirection.downOnce);
                    lastUsedRow++;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
