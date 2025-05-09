using NodeCanvas.BehaviourTrees;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class CatController : MonoBehaviour
{
    NavMeshAgent myNavMeshAgent;
    public int posZ;
    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        myNavMeshAgent.updateRotation = false;
        if (Input.GetMouseButtonDown(0))
        {
            SetDestinationToMousePosition();
        }
    }

    void SetDestinationToMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            myNavMeshAgent.SetDestination(new Vector3(hit.point.x, hit.point.y, posZ));
        }
    }
}
