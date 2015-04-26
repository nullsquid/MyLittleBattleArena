using UnityEngine;
using System.Collections;

public class Inputmanager : MonoBehaviour {


	float P1_Horizontal, P1_Vertical, P2_Horizontal, P2_Vertical, P3_Horizontal, P3_Vertical, P4_Horizontal, P4_Vertical;
	bool P1_Fire, P2_Fire, P3_Fire, P4_Fire;

	bool keyboardInput = true;
	bool joystickInput = false;


	void Update(){

		if(keyboardInput){

			P1_Horizontal = Input.GetAxis ("Horizontal");
			P1_Vertical = Input.GetAxis("Vertical");
		
		//P1_Horizontal = (0-Input.GetKeyDown(KeyCode.A)+Input.GetKeyDown(KeyCode.D));
		//P1_Vertical = (0-Input.GetKeyDown(KeyCode.A)+Input.GetKeyDown(KeyCode.D));
		//P1_Horizontal Input.GetKeyDown(KeyCode.A) 
		
		P1_Fire = Input.GetKeyDown(KeyCode.Space);

		}


		if(joystickInput){
			P1_Horizontal = Input.GetAxis("Joy1 Axis 6");
			P1_Vertical = Input.GetAxis("Joy1 Axis 7");


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
