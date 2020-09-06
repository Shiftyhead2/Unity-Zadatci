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
    [Header("Ground check")]
    public Transform FloorCheck;
    public float GroundCheckRadious;

    private bool hasBox = false;

    [Header("Distance to pickup/drop variable")]
    public float DistanceToPickUpOrDrop;
    [Header("Speed variable")]
    public float Speed;
    private float actualSpeed;

    [Header("Jump variables")]
    public float JumpRadius;

    private float DistanceFromTarget;

    private GameObject[] containers;
    private GameObject[] objects;

    public float JumpForce;


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
            checkIfInRadius();
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

        //Flip the AI
        if(Movement.x >= 0.01f){
            transform.localScale = new Vector3(1,2,1);
            
        }else if(Movement.x <= 0.01f)
        {
            transform.localScale = new Vector3(-1,2,1);
        }
    }

    //Function that finds the target for the AI to go to.
    private void findTarget()
    {
        objects = new GameObject[0];
        if(!hasBox)
        {
            objects = GameObject.FindGameObjectsWithTag("Box");
            if(objects.Length > 0){
                int which = Random.Range(0,objects.Length);
                target = objects[which].transform;
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

    //Function that checks if there is a box in the way. If there is the AI jumps if it can.
    private void checkIfInRadius()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,JumpRadius);
        if(colliders.Length > 0)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].CompareTag("Box"))
                {
                    if(colliders[i].transform != target || colliders[i].transform != pickedUpObject){
                        if(IsGrounded())
                        {
                            //Debug.Log("Jumping");
                            Vector2 force = Vector2.up * JumpForce;
                            myRB.AddForce(force);
                        }
                    }
                }
            }
        }
    }

    //Boolean that checks if the AI is touching the ground
    private bool IsGrounded()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(FloorCheck.position,GroundCheckRadious,9);
        if(collider2Ds.Length > 0)
        {
            return true;
        }
        return false;
    }
}
