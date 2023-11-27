using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public GameObject goal;
   
    public GameObject leftWall;
 
    public GameObject rightWall;

    public GameObject frontWall;

    public GameObject backWall;

    public GameObject unvisitedBlock;

    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
        unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall()
    {
        leftWall.SetActive(false);
    }

    public void ClearRightWall()
    {
        rightWall.SetActive(false);
    }

    public void ClearFrontWall()
    {
        frontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        backWall.SetActive(false);
    }

      public void createGoal(){
        Vector3 goalPosition = transform.position - new Vector3(0f, 4.5f, 0f);
        Instantiate(goal, goalPosition, Quaternion.identity);
    }
}