using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    public GameObject target;
    public float targetDistance = 10;
    public float lookSpeed = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Input.GetAxis("Mouse X") * Time.deltaTime * lookSpeed);
        transform.Translate(-1*Vector3.up * Input.GetAxis("Mouse Y") * Time.deltaTime * lookSpeed);
        transform.LookAt(target.transform);
        
        float distance = Vector3.Distance(target.transform.position, transform.position);
        transform.Translate(Vector3.forward * (distance - targetDistance));
        
        if(Input.GetKey("p")){
        	if(Cursor.lockState == CursorLockMode.Locked){
        		Cursor.lockState = CursorLockMode.None;
        		Cursor.visible = true;
        	}
        	else{
        		Cursor.lockState = CursorLockMode.Locked;
        		Cursor.visible = false;
		}
        }
    }
}
