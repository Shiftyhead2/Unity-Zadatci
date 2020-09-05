using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Behaviour : MonoBehaviour
{

    private Transform target;
    private Rigidbody2D myRB;
    private Vector2 Movement;

    private Transform pickedUpObject;

    public Transform PickUpOrigin;

    private bool hasBox = false;

    public float DistanceToPickUpOrDrop;
    public float Speed;
    private float actualSpeed;

    private float DistanceFromTarget;

    private GameObject[] containers;
    private GameObject[] objects;


    // Start is called before the first frame update
    void Start()
    {
        myRB = this.GetComponent<Rigidbody2D>();
        containers = GameObject.FindGameObjectsWithTag("Container");
        actualSpeed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            //Find target
            findTarget();
        }else
        {
            //Move towards the target
            Vector3 direction = target.position - transform.position;
            Movement = direction.normalized;
            moveCharacter(Movement);
            DistanceFromTarget = (int)Vector2.Distance(target.position,transform.position);
            if(DistanceFromTarget > DistanceToPickUpOrDrop){
                actualSpeed = Speed;
            }
            else if(DistanceFromTarget <= DistanceToPickUpOrDrop)
            {
                actualSpeed = 0f;
                if(target.CompareTag("Box"))
                {
                    pickUpBox();
                }else if(target.CompareTag("Container"))
                {
                    dropBox();
                }
            }

        }

        if(Movement.x >= 0.01f){
            transform.localScale = new Vector3(1,2,1);
            
        }else if(Movement.x <= 0.01f)
        {
            transform.localScale = new Vector3(-1,2,1);
        }
    }


    private void findTarget()
    {
        objects = new GameObject[0];
        if(!hasBox)
        {
            objects = GameObject.FindGameObjectsWithTag("Box");
            if(objects.Length > 0){
                target = objects[0].transform;
            }
            
        }else
        {
            BoxManager box = pickedUpObject.GetComponent<BoxManager>();
            if(box.currentColor == Color.red){
                //Debug.Log(containers[0].name + " has the same color as the " + pickedUpObject.name);
                target = containers[0].transform;
            }else if(box.currentColor == Color.blue){
                //Debug.Log(containers[1].name + " has the same color as the " + pickedUpObject.name);
                target = containers[1].transform;
            }
            
            
        }

        if(target != null)
        {
        Debug.Log("Target:" + target.name);
        }
    }

    private void moveCharacter(Vector2 direction)
    {
        myRB.MovePosition((Vector2)transform.position + (direction * actualSpeed * Time.fixedDeltaTime));
    }

    private void pickUpBox()
    {
        target.GetComponent<BoxManager>().PickedUp(PickUpOrigin);
        pickedUpObject = target;
        target = null;
        hasBox = true;
    }

    private void dropBox()
    {
        target.GetComponent<Container>().IncreaseAmount();
        pickedUpObject.GetComponent<BoxManager>().Drop();
        pickedUpObject = null;
        target = null;
        hasBox = false;
    }
}
