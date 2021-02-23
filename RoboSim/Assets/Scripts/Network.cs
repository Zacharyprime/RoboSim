using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Network : MonoBehaviour {  	

	public Rigidbody rb;
	
	public float leftForce = 0;
    	public float rightForce = 0;
	
	//Marks the location of the wheels
	public GameObject leftWheel;
	public GameObject rightWheel;

	private Vector3 currentVelocity = Vector3.zero;
	//Handles TCP commands
	/*
	Command List:
	vel x y z
	*/
	string currentMessage = "none";
	bool readMessage = false;
	private void processMessage(string message){
		string[] words = message.Split(' ');
		
		//COMMAND DEFINITIONS
			if(words[0] == "vel"){
				currentVelocity = new Vector3(float.Parse(words[1]),float.Parse(words[2]),float.Parse(words[3]));
			}
			if(words[0] == "vel+"){
				currentVelocity += new Vector3(float.Parse(words[1]),float.Parse(words[2]),float.Parse(words[3]));
			}
			if(words[0] == "motor"){
				if(words[1] == "left" || words[1] == "0"){
					leftForce = float.Parse(words[2]);
					
				}
				if(words[1] == "right" || words[1] == "1"){
					rightForce = float.Parse(words[2]);
				}
			}
			if(words[0]=="forward"){
				leftForce = float.Parse(words[1]);
				rightForce = leftForce;
			}
		
		readMessage = false;
	}




	//Networking stuff
	//Minor edit of https://gist.github.com/danielbierwirth/0636650b005834204cb19ef5ae6ccedb
	//All credit to original creator. ^
	#region private members 	
	private TcpClient socketConnection; 	
	private Thread clientReceiveThread; 	
	#endregion  	
	// Use this for initialization 	
	void Start () {
		ConnectToTcpServer();   
		rb = GetComponent<Rigidbody>();  
	}  	
	// Update is called once per frame
	void Update () {         
		if (Input.GetKey("i")) {             
			SendMessage();         
		}     
		if(readMessage){
			processMessage(currentMessage);
		}
		
		
		//Command Handling
		rb.velocity = transform.TransformDirection(currentVelocity);
		
		rb.AddForceAtPosition(transform.forward*leftForce,leftWheel.transform.position);
		Debug.DrawRay(leftWheel.transform.position, transform.forward*leftForce,Color.blue);
		
		rb.AddForceAtPosition(transform.forward*rightForce,rightWheel.transform.position);
		Debug.DrawRay(rightWheel.transform.position, transform.forward*rightForce,Color.red);	
	}  	
	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	private void ConnectToTcpServer () { 		
		try {  			
			clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
			clientReceiveThread.IsBackground = true; 			
			clientReceiveThread.Start(); 
			Debug.Log("Connected to Server"); 		
		} 		
		catch (Exception e) { 			
			Debug.Log("On client connect exception " + e); 		
		} 	
	}  
		
	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData() { 		
		try { 			
			socketConnection = new TcpClient("192.168.125.1", 5005);  			
			Byte[] bytes = new Byte[1024];             
			while (true) { 				
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream()) { 					
					int length; 					
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 						
						var incommingData = new byte[length]; 						
						Array.Copy(bytes, 0, incommingData, 0, length); 						
						// Convert byte array to string message. 						
						string serverMessage = Encoding.ASCII.GetString(incommingData); 						
						Debug.Log("server message received as: " + serverMessage); 
						currentMessage = serverMessage;
						readMessage = true;					
					} 				
				} 			
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}  	
	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	private void SendMessage() {         
		if (socketConnection == null) {             
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream(); 			
			if (stream.CanWrite) {                 
				string clientMessage = "This is a message from one of your clients."; 				
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage); 				
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);                 
				Debug.Log("Client sent his message - should be received by server");             
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	} 
}
