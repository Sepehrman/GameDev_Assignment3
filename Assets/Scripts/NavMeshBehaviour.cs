using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshBehaviour : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    public void BuildNavMesh()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
    }
}
