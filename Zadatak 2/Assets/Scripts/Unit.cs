using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Rigidbody2D myRB;
    public Transform target;

    public float speed;
    Vector3[] path;
    int targetIndex;
    public float waypointDistance;
    Vector3 currentWaypoint;
    float DistanceFromWaypoint;

    void Start()
    {
        PathRequestManager.RequestPath(transform.position,target.position,OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful){
            path = newPath;
            currentWaypoint = path[0];
        }
    }

   

    void FixedUpdate()
    {
        if(path != null)
        {
          
            Vector2 dir = (currentWaypoint - transform.position).normalized;
            dir *= speed * Time.deltaTime;
            myRB.MovePosition((Vector2)transform.position + dir);

            DistanceFromWaypoint = Vector2.Distance((Vector2)currentWaypoint,transform.position);

            if(DistanceFromWaypoint <= waypointDistance)
            {   
                targetIndex++;
            }

            if(targetIndex >= path.Length)
            {
                path = null;
                targetIndex = 0;
            }else
            {
            currentWaypoint = path[targetIndex];
            }
        }
    }
    

    
}
