using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Movement : MonoBehaviour
{
    public float speed = 0.5f;
    public float rotationSpeed = 5;
    
    public Rigidbody rb;
    HingeJoint leftJoint, rightJoint; 
    JointMotor leftMotor, rightMotor;
    
    //Marks the location of the wheels
    public GameObject leftWheel;
    public GameObject rightWheel;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        leftJoint = leftWheel.GetComponent<HingeJoint>();
        rightJoint = rightWheel.GetComponent<HingeJoint>();
        
        leftMotor = leftJoint.motor;
        rightMotor = rightJoint.motor;
    }

    // Update is called once per frame
    void Update()
    {
    	//Modified forward vector to keep 2D
    	Vector3 forward = transform.forward;
    	forward.y = 0;
    
    	//Horizontal Movement
       if (Input.GetKey("w")){
	    transform.position += transform.forward * speed;
	} 
	
	if (Input.GetKey("s")){
	    transform.position -= transform.forward * speed;
	} 
	
	if (Input.GetKey("a")){
	    transform.position -= transform.right * speed;
	} 
	
	if (Input.GetKey("d")){
	    transform.position += transform.right * speed;
	} 
	
	
	/*
	//Vertical
	if(Input.GetKey("left")){
		//transform.eulerAngles += new Vector3(0,0,rotationSpeed);
		rb.AddForceAtPosition(forward*50,leftWheel.transform.position);
		Debug.DrawRay(leftWheel.transform.position, forward*50,Color.blue);
		
	}
	if(Input.GetKey("right")){
		//transform.eulerAngles -= new Vector3(0,0,rotationSpeed);
		rb.AddForceAtPosition(forward*50,rightWheel.transform.position);
		Debug.DrawRay(rightWheel.transform.position, forward*50,Color.red);
	}
	*/
	
	//Vertical
	if(Input.GetKey("left")){
		leftMotor.targetVelocity = 90;
		
	}
	else{
		leftMotor.targetVelocity = 0;
	}
	if(Input.GetKey("right")){
		rightMotor.targetVelocity = 90;
	}
	else{
		rightMotor.targetVelocity = 0;
	}
	
	leftJoint.motor = leftMotor;
	rightJoint.motor = rightMotor;
    }
}
