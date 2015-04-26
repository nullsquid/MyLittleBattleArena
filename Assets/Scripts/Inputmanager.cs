using UnityEngine;
using System.Collections;




public class Inputmanager : MonoBehaviour {


	static public float P1_Horizontal, P1_Vertical, P2_Horizontal, P2_Vertical, P3_Horizontal, P3_Vertical, P4_Horizontal, P4_Vertical;
	static public bool P1_Fire, P2_Fire, P3_Fire, P4_Fire;

	public bool keyboardInput = true;
	public bool joystickInput = false;



	void Start(){



	}

	void FixedUpdate(){

		if(keyboardInput){

			P1_Horizontal = Input.GetAxis ("Horizontal1");
			P1_Vertical = Input.GetAxis("Vertical1");
			P1_Fire = Input.GetKeyDown(KeyCode.Space);

			P2_Horizontal = Input.GetAxis ("Horizontal2");
			P2_Vertical = Input.GetAxis("Vertical2");
			P2_Fire = Input.GetKeyDown(KeyCode.B);

			
			P3_Horizontal = Input.GetAxis ("Horizontal3");
			P3_Vertical = Input.GetAxis("Vertical3");
			P3_Fire = Input.GetKeyDown(KeyCode.M);

			
			P4_Horizontal = Input.GetAxis ("Horizontal4");
			P4_Vertical = Input.GetAxis("Vertical4");
			P4_Fire = Input.GetKeyDown(KeyCode.RightShift);

		}


		if(joystickInput){  //For Xbox

			P1_Horizontal = Input.GetAxis("DPad_XAxis_1");
			P1_Vertical = Input.GetAxis("DPad_YAxis_1");
			P1_Fire = Input.GetButton("A_1");

			P2_Horizontal = Input.GetAxis("DPad_XAxis_2");
			P2_Vertical = Input.GetAxis("DPad_YAxis_2");
			P2_Fire = Input.GetButton("A_2");

			P3_Horizontal = Input.GetAxis("DPad_XAxis_3");
			P3_Vertical = Input.GetAxis("DPad_YAxis_3");
			P3_Fire = Input.GetButton("A_3");
		
			P4_Horizontal = Input.GetAxis("DPad_XAxis_4");
			P4_Vertical = Input.GetAxis("DPad_YAxis_4");
			P4_Fire = Input.GetButton("A_4");

//			print (Input.GetAxis("DPad_XAxis_1") + " " + Input.GetAxis("DPad_XAxis_2") + " " +P3_Horizontal + " " +P4_Horizontal + " " );

//			P2_Horizontal = Input.GetAxis("Joy2 Axis 6");
//			P2_Vertical = Input.GetAxis("Joy2 Axis 7");
//
//			P3_Horizontal = Input.GetAxis("Joy3 Axis 6");
//			P3_Vertical = Input.GetAxis("Joy3 Axis 7");
//
//			P4_Horizontal = Input.GetAxis("Joy4 Axis 6");
//			P4_Vertical = Input.GetAxis("Joy4 Axis 7");

//			 if(Input.GetButton("A_1") !=0){
//				P1_Fire = true;
//			}
//			else {
//				P1_Fire = false;
//			}

//
//			if(Input.GetAxis("Joy2 Axis 3") !=0){
//				P2_Fire = true;
//			}
//			else {
//				P2_Fire = false;
//			} 
//
//			if(Input.GetAxis("Joy3 Axis 3") !=0){
//				P3_Fire = true;
//			}
//			else {
//				P3_Fire = false;
//			} 
//
//			if(Input.GetAxis("Joy4 Axis 3") !=0){
//				P4_Fire = true;
//			}
//			else {
//				P4_Fire = false;
//			}

			//P2_Fire = Input.GetAxis("Joy2 Axis 3");
			//P3_Fire = Input.GetAxis("Joy3 Axis 3");
			//P4_Fire = Input.GetAxis("Joy4 Axis 3");
			

			/*
			P1_Horizontal = (0-Input.GetKeyDown(KeyCode.Joystick1Button7)+Input.GetKeyDown(KeyCode.Joystick1Button8));
			P1_Vertical = (0-Input.GetKeyDown(KeyCode.Joystick1Button5)+Input.GetKeyDown(KeyCode.Joystick1Button6));

			P2_Horizontal = (0-Input.GetKeyDown(KeyCode.Joystick2Button7)+Input.GetKeyDown(KeyCode.Joystick2Button8));
			P2_Vertical = (0-Input.GetKeyDown(KeyCode.Joystick2Button5)+Input.GetKeyDown(KeyCode.Joystick2Button6));
		
			P2_Horizontal = (0-Input.GetKeyDown(KeyCode.Joystick3Button7)+Input.GetKeyDown(KeyCode.Joystick3Button8));
			P2_Vertical = (0-Input.GetKeyDown(KeyCode.Joystick3Button5)+Input.GetKeyDown(KeyCode.Joystick3Button6));

			P2_Horizontal = (0-Input.GetKeyDown(KeyCode.Joystick4Button7)+Input.GetKeyDown(KeyCode.Joystick4Button8));
			P2_Vertical = (0-Input.GetKeyDown(KeyCode.Joystick4Button5)+Input.GetKeyDown(KeyCode.Joystick4Button6));
*/
		}



	}
}
