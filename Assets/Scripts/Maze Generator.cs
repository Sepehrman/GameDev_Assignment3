using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public MazeCell mazeCellPrefab;

    public int mazeWidth;

    public int mazeDepth;

    public GameObject navMeshSurfaceObject;

    private NavMeshBehaviour navMeshBehaviour;

    private MazeCell[,] mazeGrid;

    private float sizeOfCell = 5f;

    private bool goalCreated = false;

    private int count = 0;

    private int highestCount = 0;

    private MazeCell highestCell;

    void Start()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                mazeGrid[x, z] = Instantiate(mazeCellPrefab, new Vector3(x*sizeOfCell, 0, z*sizeOfCell), Quaternion.identity);
            }
        }

        GenerateMaze(null, mazeGrid[0, 0]);

        if (highestCell != null){
            highestCell.createGoal();
        }

        // Build NavMesh after maze is generated
        navMeshBehaviour = navMeshSurfaceObject.GetComponent<NavMeshBehaviour>();
        navMeshBehaviour.BuildNavMesh();
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;
        
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                count++;
                GenerateMaze(currentCell, nextCell);
                count--;
            }
        } while (nextCell != null);
        getDeepestCell(currentCell, count);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();

    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
{
    int x = (int)(currentCell.transform.position.x / sizeOfCell); 
    int z = (int)(currentCell.transform.position.z / sizeOfCell); 

    if (x + 1 < mazeWidth)
    {
        var cellToRight = mazeGrid[x + 1, z];

        if (cellToRight.IsVisited == false)
        {
            yield return cellToRight;
        }
    }

    if (x - 1 >= 0)
    {
        var cellToLeft = mazeGrid[x - 1, z];

        if (cellToLeft.IsVisited == false)
        {
            yield return cellToLeft;
        }
    }

    if (z + 1 < mazeDepth)
    {
        var cellToFront = mazeGrid[x, z + 1];

        if (cellToFront.IsVisited == false)
        {
            yield return cellToFront;
        }
    }

    if (z - 1 >= 0)
    {
        var cellToBack = mazeGrid[x, z - 1];

        if (cellToBack.IsVisited == false)
        {
            yield return cellToBack;
        }
    }
}


    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    void getDeepestCell(MazeCell currentCell, int count){
        if (count > highestCount && !HasUnvisitedNeighbors(currentCell))
            {
                highestCount = count;
                highestCell = currentCell;
            }
    }

    private bool HasUnvisitedNeighbors(MazeCell currentCell)
    {
        return GetUnvisitedCells(currentCell).Any();
    }


}
