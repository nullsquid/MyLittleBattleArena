#region Namespaces

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#endregion

/***********************************************************************
 *  cInput 2.4.5 by Ward Dewaele & Deozaan
 *  This script is NOT free, unlike Custom Inputmanager 1.x.
 *  Therefore the use of this script is strictly personal and 
 *  may not be spread without permission of me (Ward Dewaele).
 *  
 *  Any technical or license questions can be mailed
 *  to ward.dewaele@pandora.be, but read the 
 *  included help documents first please.
 ***********************************************************************/

public class cInput : MonoBehaviour {

	#region cInput vars

	public static GUISkin cSkin;

	public static float gravity = 3;
	public static float sensitivity = 3;
	public static float deadzone = 0.001f;
	public static bool scanning { get { return _scanning; } } // this is read-only
	public static int length {
		get {
			_CreatecObject(); // if cInput doesn't exist, create it
			return _inputLength + 1;
		}
	} // this is read-only
	public static bool allowDuplicates {
		get {
			_CreatecObject(); // if cInput doesn't exist, create it
			return _allowDuplicates;
		}
		set {
			_allowDuplicates = value;
			PlayerPrefs.SetString("_dubl", value.ToString());
			_exAllowDuplicates = value.ToString();
		}
	}

	// Private variables
	private static bool _allowDuplicates = false;
	private static string[,] _defaultStrings = new string[99, 3];
	private static string[] _inputName = new string[99]; // name of the input action (e.g., "Jump")
	private static KeyCode[] _inputPrimary = new KeyCode[99]; // primary input assigned to action (e.g., "Space")
	private static KeyCode[] _inputSecondary = new KeyCode[99]; // secondary input assigned to action
	private static string[] _axisName = new string[99];
	private static string[] _axisPrimary = new string[99];
	private static string[] _axisSecondary = new string[99];
	private static float[] _individualAxisSens = new float[99]; // individual axis sensitivity settings
	private static bool[] _invertAxis = new bool[99];
	private static int[,] _makeAxis = new int[99, 3];
	private static int _inputLength = -1;
	private static int _axisLength = -1;
	private static List<KeyCode> _forbiddenKeys = new List<KeyCode>();
	private static bool[] _virtAxis = new bool[99];

	private static bool[] _getKeyArray = new bool[99]; // values stored for GetKey function
	private static bool[] _getKeyDownArray = new bool[99]; // values stored for GetKeyDown
	private static bool[] _getKeyUpArray = new bool[99]; // values stored for GetKeyUp
	private static bool[] _axisTriggerArray = new bool[99]; // values that help to check if an axis is up or down
	private static float[] _getAxisArray = new float[99];
	private static float[] _getAxis = new float[99];
	private static float[] _getAxisArrayRaw = new float[99];
	private static float[] _getAxisRaw = new float[99];

	// which types of inputs to allow when assigning inputs to actions
	private static bool _allowMouseAxis = false;
	private static bool _allowMouseButtons = true;
	private static bool _allowJoystickButtons = true;
	private static bool _allowJoystickAxis = true;
	private static bool _allowKeyboard = true;

	private static int _numGamepads = 5; // number of gamepads supported by built-in Input Manager settings

	private static bool _showMenu;
	private Vector2 _scrollPosition;
	// these strings are set by ShowMenu() to customize the look of cInput's menu
	private static string _menuHeaderString = "label";
	private static string _menuActionsString = "box";
	private static string _menuInputsString = "box";
	private static string _menuButtonsString = "button";

	private static bool _scanning; // are we scanning inputs to make a new assignment?
	private static int _cScanIndex; // the index of the array for inputs
	private static int _cScanInput; // which input (primary or secondary)
	private static bool _cObjectOn; // whether cInput is initialized or not
	private static bool _cKeysLoaded;

	// External saving related variables
	private static string _exAllowDuplicates;
	private static string _exAxis;
	private static string _exAxisInverted;
	private static string _exDefaults;
	private static string _exInputs;
	private static string _exCalibrations;
	private static bool _externalSaving = false;

	Rect windowRect;
	float mouseTimer = 0.5f;
	bool showPopUp;
	private static Dictionary<string, KeyCode> _string2Key = new Dictionary<string, KeyCode>();

	private static int[] _axisType = new int[10 * _numGamepads];
	// Note: this wastes one slot
	private static string[,] _joyStrings = new string[_numGamepads, 11];
	private static string[,] _joyStringsPos = new string[_numGamepads, 11];
	private static string[,] _joyStringsNeg = new string[_numGamepads, 11];

	#endregion

	#region Awake/Start functions

	void Awake() {
		DontDestroyOnLoad(this); // Keep this thing from getting destroyed if we change levels.
	}

	void Start() {
		_CreateDictionary();
		if (_externalSaving) {
			_LoadExternalInputs();
			//Debug.Log("Loaded inputs from external source.");
		} else {
			_LoadInputs();
			//Debug.Log("Loaded inputs from PlayerPrefs.");
		}
	}

	#endregion

	public static void Init() {
		_CreatecObject(); // if cInput doesn't exist, create it 
	}

	private static void _CreateDictionary() {
		if (_string2Key.Count == 0) { // don't create the dictionary more than once
			for (int i = (int)KeyCode.None; i < (int)KeyCode.Joystick4Button19 + 1; i++) {
				KeyCode key = (KeyCode)i;
				_string2Key.Add(key.ToString(), key);
			}

			// Create joystrings dictionaries
			for (int i = 1; i < _numGamepads; i++) {
				for (int j = 1; j <= 10; j++) {
					_joyStrings[i, j] = "Joy" + i + " Axis " + j;
					_joyStringsPos[i, j] = "Joy" + i + " Axis " + j + "+";
					_joyStringsNeg[i, j] = "Joy" + i + " Axis " + j + "-";
				}
			}
		}
	}

	public static void ForbidKey(KeyCode key) {
		_CreateDictionary();
		if (!_forbiddenKeys.Contains(key)) {
			_forbiddenKeys.Add(key);
		}
	}

	public static void ForbidKey(string keyString) {
		_CreateDictionary();
		KeyCode key = ConvertString2Key(keyString);
		ForbidKey(key);
	}

	public static KeyCode ConvertString2Key(string _str) {
		if (_string2Key.Count == 0) { return KeyCode.None; }

		if (_string2Key.ContainsKey(_str)) {
			KeyCode _key = _string2Key[_str];
			return _key;
		} else {
			if (!_isAxisValid(_str)) {
				Debug.Log("cInput error: " + _str + " is not a valid input.");
			}

			return KeyCode.None;
		}

	}

	#region SetKey functions

	// this is for compatibility with UnityScript which doesn't accept default parameters
	public static void SetKey(string action, string input1) {
		SetKey(action, input1, "");
	}

	// This automatically sets the input number for people who don't care what the number is
	public static void SetKey(string action, string primary, string secondary) {
		// make sure this key hasn't already been set
		if (findKeyByDescription(action) == -1) {
			int _num = _inputLength + 1;
			_SetDefaultKey(_num, action, primary, secondary);
		} else {
			// skip this warning if we loaded from an external source or we already created the cInput object
			if (_externalSaving == false || GameObject.Find("cInput").GetComponent<cInput>() == null) {
				// Whoops! Key with this name already exists!
				Debug.LogWarning("A key with the name of " + action + " already exists. You should use ChangeKey() if you want to change an existing key!");
			}
		}
	}

	private static void _SetDefaultKey(int _num, string _name, string _input1, string _input2 = "") {
		_defaultStrings[_num, 0] = _name;
		_defaultStrings[_num, 1] = _input1;

		if (string.IsNullOrEmpty(_input2)) {
			_defaultStrings[_num, 2] = KeyCode.None.ToString();
		} else {
			_defaultStrings[_num, 2] = _input2;
		}

		if (_num > _inputLength) {
			_inputLength = _num;
		}

		_SetKey(_num, _name, _input1, _input2);
		_SaveDefaults();

	}

	private static void _SetKey(int _num, string _name, string _input1, string _input2 = "") {
		// input description 
		_inputName[_num] = _name;
		_axisPrimary[_num] = "";

		if (_string2Key.Count == 0) { return; }

		if (!string.IsNullOrEmpty(_input1)) {

			// enter keyboard input in the input  array
			KeyCode _keyCode1 = ConvertString2Key(_input1);
			_inputPrimary[_num] = _keyCode1;

			// enter mouse and gamepad axis inputs in the inputstring array
			string axisName = _ChangeStringToAxisName(_input1);
			if (_input1 != axisName) {
				_axisPrimary[_num] = _input1;
			}

		}

		_axisSecondary[_num] = "";

		if (!string.IsNullOrEmpty(_input2)) {

			// enter input in the alt input  array
			KeyCode _keyCode2 = ConvertString2Key(_input2);
			_inputSecondary[_num] = _keyCode2;

			// enter mouse and gamepad axis inputs in the inputstring array
			string axisName = _ChangeStringToAxisName(_input2);
			if (_input2 != axisName) {
				_axisSecondary[_num] = _input2;
			}
		}
	}

	#endregion

	#region SetAxis and SetAxisSensitivity & related functions

	// overloaded SetAxis function
	public static void SetAxis(string description, string negativeInput, string positiveInput) {
		SetAxis(description, negativeInput, positiveInput, sensitivity);
	}

	// This is the function that all other SetAxis overload methods call to actually set the axis
	public static void SetAxis(string description, string negativeInput, string positiveInput, float axisSensitivity) {
		if (IsKeyDefined(negativeInput)) {
			int _num = _axisLength + 1;
			int posInput = -1; // -1 by default, which means no input.
			if (IsKeyDefined(positiveInput)) {
				posInput = findKeyByDescription(positiveInput);
			} else if (positiveInput != "-1") {
				// the key isn't defined and we're not passing in -1 as a value, so there's a problem
				Debug.LogError("Can't define Axis named: " + description + ". Please define '" + positiveInput + "' with SetKey() first.");
				return; // break out of this function without trying to assign the axis
			}

			_SetAxis(_num, description, findKeyByDescription(negativeInput), posInput);
			_individualAxisSens[_num] = axisSensitivity;
		} else {
			Debug.LogError("Can't define Axis named: " + description + ". Please define '" + negativeInput + "' with SetKey() first.");
		}
	}

	// overloard method to allow you to set an axis with only one input
	public static void SetAxis(string description, string input) {
		SetAxis(description, input, "-1", sensitivity);
	}

	// overload method to allow you to set an axis with only one input, and set sensitivity
	public static void SetAxis(string description, string input, float axisSensitivity) {
		SetAxis(description, input, "-1", axisSensitivity);
	}

	private static void _SetAxis(int _num, string _description, int _negative, int _positive) {
		if (_num > _axisLength) {
			_axisLength = _num;
		}
		_invertAxis[_num] = false;
		_axisName[_num] = _description;
		_makeAxis[_num, 0] = _negative;
		_makeAxis[_num, 1] = _positive;
		_SaveAxis();
		_SaveAxInverted();
	}

	// this allows you to set the axis sensitivity directly (after it the axis has been defined)
	public static void SetAxisSensitivity(string axisName, float sensitivity) {
		int axis = _FindAxisByDescription(axisName);
		if (axis == -1) {
			// axis not defined!
			Debug.LogError("Cannot set sensitivity of " + axisName + ". Have you defined this axis with SetAxis() yet?");
		} else {
			// axis has been defined
			_individualAxisSens[axis] = sensitivity;
		}
	}

	#endregion

	#region Calibration functions

	public static void Calibrate() {
		string _saveCals = "";
		for (int i = 1; i < _numGamepads; i++) {
			for (int n = 1; n < 11; n++) {
				int index = 10 * (i - 1) + (n - 1);
				string _joystring = _joyStrings[i, n];
				float axis = Input.GetAxisRaw(_joystring);
				_axisType[index] = axis < -deadzone ? 1 :
					axis > deadzone ? -1 :
					0;
				_saveCals += _axisType[index] + "*";
				PlayerPrefs.SetString("_saveCals", _saveCals);
				_exCalibrations = _saveCals;
			}
		}
	}

	private static float _GetCalibratedAxisInput(string description) {
		float rawValue = Input.GetAxis(_ChangeStringToAxisName(description));

		for (int i = 1; i < _numGamepads; i++) {
			for (int j = 1; j < 10; j++) {
				string joyPos = _joyStringsPos[i, j];
				string joyNeg = _joyStringsNeg[i, j];
				if (description == joyPos || description == joyNeg) {
					int index = 10 * (i - 1) + (j - 1);
					switch (_axisType[index]) {
						case 0: {
								return rawValue;
							}
						case 1: {
								return (rawValue + 1) / 2;
							}
						case -1: {
								return (rawValue - 1) / 2;
							}
					}
				}
			}
		}

		return rawValue;
	}

	#endregion

	#region ChangeKey functions

	public static void ChangeKey(string action, int input, bool mouseAx, bool mouseBut, bool joyAx, bool joyBut, bool keyb) {
		_CreatecObject(); // if cInput doesn't exist, create it
		int _num = findKeyByDescription(action);
		_ScanForNewKey(_num, input, mouseAx, mouseBut, joyAx, joyBut, keyb);
	}

	#region overloaded ChangeKey(string) functions for UnityScript compatibility

	public static void ChangeKey(string action) {
		ChangeKey(action, 1, _allowMouseAxis, _allowMouseButtons, _allowJoystickAxis, _allowJoystickButtons, _allowKeyboard);
	}

	public static void ChangeKey(string action, int input) {
		ChangeKey(action, input, _allowMouseAxis, _allowMouseButtons, _allowJoystickAxis, _allowJoystickButtons, _allowKeyboard);
	}

	public static void ChangeKey(string action, int input, bool mouseAx) {
		ChangeKey(action, input, mouseAx, _allowMouseButtons, _allowJoystickAxis, _allowJoystickButtons, _allowKeyboard);
	}

	public static void ChangeKey(string action, int input, bool mouseAx, bool mouseBut) {
		ChangeKey(action, input, mouseAx, mouseBut, _allowJoystickAxis, _allowJoystickButtons, _allowKeyboard);
	}

	public static void ChangeKey(string action, int input, bool mouseAx, bool mouseBut, bool joyAx) {
		ChangeKey(action, input, mouseAx, mouseBut, joyAx, _allowJoystickButtons, _allowKeyboard);
	}

	public static void ChangeKey(string action, int input, bool mouseAx, bool mouseBut, bool joyAx, bool joyBut) {
		ChangeKey(action, input, mouseAx, mouseBut, joyAx, joyBut, _allowKeyboard);
	}

	#endregion

	// use an int with ChangeKey, useful in for loops for GUI
	public static void ChangeKey(int index, int input, bool mouseAx, bool mouseBut, bool joyAx, bool joyBut, bool keyb) {
		_CreatecObject(); // if cInput doesn't exist, create it
		_ScanForNewKey(index, input, mouseAx, mouseBut, joyAx, joyBut, keyb);
	}

	#region overloaded ChangeKey(int) functions for UnityScript compatibility

	public static void ChangeKey(int index) {
		ChangeKey(index, 1, false, true, true, true, true);
	}

	public static void ChangeKey(int index, int input) {
		ChangeKey(index, input, false, true, true, true, true);
	}

	public static void ChangeKey(int index, int input, bool mouseAx) {
		ChangeKey(index, input, mouseAx, true, true, true, true);
	}

	public static void ChangeKey(int index, int input, bool mouseAx, bool mouseBut) {
		ChangeKey(index, input, mouseAx, mouseBut, true, true, true);
	}

	public static void ChangeKey(int index, int input, bool mouseAx, bool mouseBut, bool joyAx) {
		ChangeKey(index, input, mouseAx, mouseBut, joyAx, true, true);
	}

	public static void ChangeKey(int index, int input, bool mouseAx, bool mouseBut, bool joyAx, bool joyBut) {
		ChangeKey(index, input, mouseAx, mouseBut, joyAx, joyBut, true);
	}

	#endregion

	// this lets the dev directly change the key without waiting for the player to push buttons.
	public static void ChangeKey(string action, string primary, string secondary) {
		_CreatecObject(); // if cInput doesn't exist, create it
		int _num = findKeyByDescription(action);

		_ChangeKey(_num, action, primary, secondary);
	}

	#region overloaded ChangeKey(string, primary, secondary) function for UnityScript compatibility)

	public static void ChangeKey(string action, string primary) {
		ChangeKey(action, primary, "");
	}

	#endregion

	private static void _ScanForNewKey(int num, int input = 1, bool mouseAx = false, bool mouseBut = true,
								bool joyAx = true, bool joyBut = true, bool keyb = true) {
		_allowMouseAxis = mouseAx;
		_allowMouseButtons = mouseBut;
		_allowJoystickButtons = joyBut;
		_allowJoystickAxis = joyAx;
		_allowKeyboard = keyb;

		_cScanInput = input;
		_cScanIndex = num;
		_scanning = true;
	}

	private static void _ChangeKey(int num, string action, string primary, string secondary = "") {
		_SetKey(num, action, primary, secondary);
		_SaveInputs();
	}

	#endregion

	#region IsKeyDefined and IsAxisDefined functions

	public static bool IsKeyDefined(string keyName) {
		if (findKeyByDescription(keyName) >= 0) {
			return true;
		}

		// if we got here then no key or axis was found
		return false;
	}

	public static bool IsAxisDefined(string axisName) {
		if (_FindAxisByDescription(axisName) >= 0) {
			return true;
		}

		// if we got here then no key or axis was found
		return false;
	}

	#endregion

	#region CheckInputs function

	private void CheckInputs() {

		bool input1 = false;
		bool input2 = false;
		bool axis1 = false;
		bool axis2 = false;
		float axFloat1 = 0f;
		float axFloat2 = 0f;

		for (int n = 0; n < _inputLength + 1; n++) {
			// handle inputs
			input1 = Input.GetKey(_inputPrimary[n]);
			input2 = Input.GetKey(_inputSecondary[n]);

			if (!string.IsNullOrEmpty(_axisPrimary[n])) {
				axis1 = true;
				axFloat1 = _GetCalibratedAxisInput(_axisPrimary[n]) * _PosOrNeg(_axisPrimary[n]);
			} else {
				axis1 = false;
				axFloat1 = 0f;
			}

			if (!string.IsNullOrEmpty(_axisSecondary[n])) {
				axis2 = true;
				axFloat2 = _GetCalibratedAxisInput(_axisSecondary[n]) * _PosOrNeg(_axisSecondary[n]);
			} else {
				axis2 = false;
				axFloat2 = 0f;
			}

			if (input1 || input2 || (axis1 && axFloat1 > 0.1f) || (axis2 && axFloat2 > 0.1f)) {
				_getKeyArray[n] = true;
			} else {
				_getKeyArray[n] = false;
			}

			if (Input.GetKeyDown(_inputPrimary[n]) || Input.GetKeyDown(_inputSecondary[n])) {
				_getKeyDownArray[n] = true;
			} else if ((axis1 && axFloat1 > deadzone && !_axisTriggerArray[n]) ||
						(axis2 && axFloat2 > deadzone && !_axisTriggerArray[n])) {
				_axisTriggerArray[n] = true;
				_getKeyDownArray[n] = true;
			} else {
				_getKeyDownArray[n] = false;
			}

			if (Input.GetKeyUp(_inputPrimary[n]) || Input.GetKeyUp(_inputSecondary[n])) {
				_getKeyUpArray[n] = true;
			} else if ((axis1 && axFloat1 <= deadzone && _axisTriggerArray[n]) ||
						(axis2 && axFloat2 <= deadzone && _axisTriggerArray[n])) {
				_axisTriggerArray[n] = false;
				_getKeyUpArray[n] = true;
			} else {
				_getKeyUpArray[n] = false;
			}

			// handle axis
			if (axis1 && !_virtAxis[n]) {
				_getAxis[n] = axFloat1;
				_getAxisRaw[n] = axFloat1;
			}

			if (axis2 && !_virtAxis[n]) {
				_getAxis[n] = axFloat2;
				_getAxisRaw[n] = axFloat2;
			}

			if (axis1 && axis2 && !_virtAxis[n]) {
				if (axFloat1 > 0) {
					_getAxis[n] = axFloat1;
					_getAxisRaw[n] = axFloat1;
				}

				if (axFloat2 > 0) {
					_getAxis[n] = axFloat2;
					_getAxisRaw[n] = axFloat2;
				}

				if (axFloat1 > 0 && axFloat2 > 0) {
					_getAxis[n] = (axFloat1 + axFloat2) / 2;
					_getAxisRaw[n] = (axFloat1 + axFloat2) / 2;
				}
			}

			// sensitivity
			if (input1 || input2) {
				_virtAxis[n] = true;
				_getAxis[n] += (sensitivity * Time.deltaTime);
				_getAxisRaw[n] = 1;
				if (_getAxis[n] > 1) {
					_getAxis[n] = 1;
				}
				if ((axis1 && axFloat1 < deadzone) || (axis2 && axFloat2 < deadzone)) {
					_getAxisRaw[n] = 1;
				}
			}

			// gravity (not for axis)
			if (!input1 && !input2) {
				if ((axis1 && axFloat1 < deadzone) || (axis2 && axFloat2 < deadzone) || (!axis1 && !axis2)) {
					_getAxis[n] -= gravity * Time.deltaTime;
					_getAxisRaw[n] = 0;
					if (_getAxis[n] < 0) {
						_getAxis[n] = 0;
					}
				}

			}

			if (_getAxis[n] == 0) {
				_virtAxis[n] = false;
			}
		}

		// compile the axis (negative and positive)
		for (int n = 0; n < _axisLength + 1; n++) {
			int neg = (int)_makeAxis[n, 0];
			int pos = (int)_makeAxis[n, 1];
			if (_makeAxis[n, 1] == -1) {
				_getAxisArray[n] = _getAxis[neg];
				_getAxisArrayRaw[n] = _getAxisRaw[neg];
			} else {
				_getAxisArray[n] = _getAxis[pos] - _getAxis[neg];
				_getAxisArrayRaw[n] = _getAxisRaw[pos] - _getAxisRaw[neg];
			}
		}
	}

	#endregion

	#region GetKey, GetAxis, GetText, and related functions

	#region GetKey functions

	// returns -1 only if there was an error
	private static int findKeyByDescription(string description) {
		for (int i = 0; i < _inputName.Length; i++) {
			if (_inputName[i] == description) {
				return i;
			}
		}

		// uh oh, the string didn't match!
		return -1;
	}

	// Returns true if the key is currently being pressed (continual)
	public static bool GetKey(string description) {
		if (!PlayerPrefs.HasKey("_count")) {
			Debug.LogWarning("No default inputs found. Please setup your default inputs with SetKey first.");
			return false;
		}

		_CreatecObject(); // if cInput doesn't exist, create it
		if (!_cKeysLoaded) { return false; } // make sure we've saved/loaded keys before trying to access them.
		int _index = findKeyByDescription(description);

		if (_index > -1) {
			return _getKeyArray[_index];
		} else {
			// if we got this far then the string didn't match and there's a problem
			Debug.LogWarning("Couldn't find a key match for " + description + ". Is it possible you typed it wrong or forgot to setup your defaults after making changes?");
			return false;
		}
	}

	// Returns true just once when the key is first pressed down
	public static bool GetKeyDown(string description) {
		if (!PlayerPrefs.HasKey("_count")) {
			Debug.LogWarning("No default inputs found. Please setup your default inputs with SetKey first.");
			return false;
		}

		_CreatecObject(); // if cInput doesn't exist, create it
		if (!_cKeysLoaded) { return false; } // make sure we've saved/loaded keys before trying to access them.
		int _index = findKeyByDescription(description);

		if (_index > -1) {
			return _getKeyDownArray[findKeyByDescription(description)];
		} else {
			// if we got this far then the string didn't match and there's a problem
			Debug.LogWarning("Couldn't find a key match for " + description + ". Is it possible you typed it wrong or forgot to setup your defaults after making changes?");
			return false;
		}
	}

	// Returns true just once when the key is released
	public static bool GetKeyUp(string description) {
		if (!PlayerPrefs.HasKey("_count")) {
			Debug.LogWarning("No default inputs found. Please setup your default inputs with SetKey first.");
			return false;
		}

		_CreatecObject(); // if cInput doesn't exist, create it
		if (!_cKeysLoaded) { return false; } // make sure we've saved/loaded keys before trying to access them.
		int _index = findKeyByDescription(description);

		if (_index > -1) {
			return _getKeyUpArray[findKeyByDescription(description)];
		} else {
			// if we got this far then the string didn't match and there's a problem
			Debug.LogWarning("Couldn't find a key match for " + description + ". Is it possible you typed it wrong or forgot to setup your defaults after making changes?");
			return false;
		}
	}

	#region GetButton functions -- they just call GetKey functions

	public static bool GetButton(string description) {
		return GetKey(description);
	}

	public static bool GetButtonDown(string description) {
		return GetKeyDown(description);
	}

	public static bool GetButtonUp(string description) {
		return GetKeyUp(description);
	}

	#endregion

	#endregion

	#region GetAxis and related functions

	private static int _FindAxisByDescription(string axisName) {
		for (int i = 0; i < _axisName.Length; i++) {
			if (_axisName[i] == axisName) {
				return i;
			}
		}

		return -1; // uh oh, the string didn't match!
	}

	public static float GetAxis(string axisName) {
		_CreatecObject(); // if cInput doesn't exist, create it
		if (!PlayerPrefs.HasKey("_count")) {
			Debug.LogWarning("No default inputs found. Please setup your default inputs with SetKey first.");
			return 0;
		}

		int index = _FindAxisByDescription(axisName);
		if (index > -1) {
			// an axis is getting callibrated so we dont return any input
			if (_invertAxis[index]) {
				// this axis should be inverted, so invert the value!
				sensitivity = _individualAxisSens[index];
				return _getAxisArray[index] * -1;
			} else {
				// this axis is normal, return the normal value
				sensitivity = _individualAxisSens[index];
				return _getAxisArray[index];
			}
		}

		// if we got this far then the string didn't match and there's a problem
		Debug.LogWarning("Couldn't find an axis match for " + axisName + ". Is it possible you typed it wrong?");
		return 0;
	}

	public static float GetAxisRaw(string axisName) {
		_CreatecObject(); // if cInput doesn't exist, create it
		if (!PlayerPrefs.HasKey("_count")) {
			Debug.LogWarning("No default inputs found. Please setup your default inputs with SetKey first.");
			return 0;
		}

		int index = _FindAxisByDescription(axisName);
		if (index > -1) {
			// an axis is getting callibrated so we dont return any input
			if (_invertAxis[index]) {
				// this axis should be inverted, so invert the value!
				return _getAxisArrayRaw[index] * -1;
			} else {
				// this axis is normal, return the normal value
				return _getAxisArrayRaw[index];
			}
		}

		// if we got this far then the string didn't match and there's a problem
		Debug.LogWarning("Couldn't find an axis match for " + axisName + ". Is it possible you typed it wrong?");
		return 0;
	}

	#endregion

	#region GetText, _ChangeStringToAxisName, _PosOrNeg functions

	public static string GetText(string action, int input) {
		_CreatecObject(); // if cInput doesn't exist, create it
		// make sure a valid value is passed in
		if (input < 0 && input > 2) {
			input = 1;
			Debug.LogWarning("Can't look for text #" + input + " for " + action + " input. Only 0, 1, or 2 is acceptable. Defaulting to " + input + ".");
		}

		int _index = findKeyByDescription(action);
		return GetText(_index, input);
	}

	#region overloaded GetText(string) and GetText(int) functions for UnityScript compatibility

	public static string GetText(string action) {
		return GetText(action, 1);
	}

	public static string GetText(int index) {
		return GetText(index, 0);
	}

	#endregion

	// use an int to getText of input assignments. Useful in for loops for GUIs.
	public static string GetText(int index, int input) {
		_CreatecObject(); // if cInput doesn't exist, create it
		// make sure a valid value is passed in
		if (input < 0 && input > 2) {
			input = 0;
			Debug.LogWarning("Can't look for text #" + input + " for " + _inputName[index] + " input. Only 0, 1, or 2 is acceptable. Defaulting to " + input + ".");
		}

		string name;

		if (input == 1) {
			if (!string.IsNullOrEmpty(_axisPrimary[index])) {
				name = _axisPrimary[index];
			} else {
				name = _inputPrimary[index].ToString();
			}
		} else if (input == 2) {
			if (!string.IsNullOrEmpty(_axisSecondary[index])) {
				name = _axisSecondary[index];
			} else {
				name = _inputSecondary[index].ToString();
			}
		} else {
			name = _inputName[index];
			return name;
		}

		// check to see if this key is currently waiting to be reassigned
		if (_scanning && (index == _cScanIndex) && (input == _cScanInput)) {
			name = ". . .";
		}

		return name;
	}

	private static string _ChangeStringToAxisName(string description) {
		// First we need to change the name of some of these things. . .
		switch (description) {
			case "Mouse Left":
				return "Mouse Horizontal";

			case "Mouse Right":
				return "Mouse Horizontal";

			case "Mouse Up":
				return "Mouse Vertical";

			case "Mouse Down":
				return "Mouse Vertical";

			case "Mouse Wheel Up":
				return "Mouse Wheel";

			case "Mouse Wheel Down":
				return "Mouse Wheel";
		}

		string joystring = _FindJoystringByDescription(description);
		if (joystring != null) {
			return joystring;
		}

		return description;
	}


	private static string _FindJoystringByDescription(string desc) {
		for (int i = 1; i < _numGamepads; i++) {
			for (int j = 1; j <= 10; j++) {
				string joyPos = _joyStringsPos[i, j];
				string joyNeg = _joyStringsNeg[i, j];
				if (desc == joyPos || desc == joyNeg) {
					return _joyStrings[i, j];
				}
			}
		}
		return null;
	}

	private static bool _isAxisValid(string _ax) {
		switch (_ax) {
			case "Mouse Left":
				return true;

			case "Mouse Right":
				return true;

			case "Mouse Up":
				return true;

			case "Mouse Down":
				return true;

			case "Mouse Wheel Up":
				return true;

			case "Mouse Wheel Down":
				return true;
		}
		bool _state = false;
		for (int i = 1; i < _numGamepads; i++) {
			for (int j = 1; j <= 10; j++) {
				string joyPos = _joyStringsPos[i, j];
				string joyNeg = _joyStringsNeg[i, j];
				if (_ax == joyPos || _ax == joyNeg) {
					_state = true;
				}
			}
		}
		return _state;
	}

	// This function returns -1 for negative axes
	private static int _PosOrNeg(string description) {
		int posneg = 1;

		switch (description) {
			case "Mouse Left":
				return -1;

			case "Mouse Right":
				return 1;

			case "Mouse Up":
				return 1;

			case "Mouse Down":
				return -1;

			case "Mouse Wheel Up":
				return 1;

			case "Mouse Wheel Down":
				return -1;
		}

		for (int i = 1; i < _numGamepads; i++) {
			for (int j = 1; j < 10; j++) {
				string joyPos = _joyStringsPos[i, j];
				string joyNeg = _joyStringsNeg[i, j];
				if (description == joyPos) {
					return 1;
				} else if (description == joyNeg) {
					return -1;
				}
			}
		}

		return posneg;
	}

	#endregion

	#endregion

	#region Save, Load & Reset functions

	private static void _SaveAxis() {
		int _num = _axisLength + 1;
		string _axName = "";
		string _axNeg = "";
		string _axPos = "";
		string _indAxSens = "";
		for (int n = 0; n < _num; n++) {
			_axName += _axisName[n] + "*";
			_axNeg += _makeAxis[n, 0] + "*";
			_axPos += _makeAxis[n, 1] + "*";
			_indAxSens += _individualAxisSens[n] + "*";
		}

		string _axis = _axName + "#" + _axNeg + "#" + _axPos + "#" + _num;
		PlayerPrefs.SetString("_axis", _axis);
		PlayerPrefs.SetString("_indAxSens", _indAxSens);
		_exAxis = _axis + "¿" + _indAxSens;
	}

	private static void _SaveAxInverted() {
		int _num = _axisLength + 1;
		string _axInv = "";

		for (int n = 0; n < _num; n++) {
			_axInv += _invertAxis[n] + "*";
		}

		PlayerPrefs.SetString("_axInv", _axInv);
		_exAxisInverted = _axInv;
	}

	private static void _SaveDefaults() {
		// saving default inputs
		int _num = _inputLength + 1;
		string _defName = "";
		string _def1 = "";
		string _def2 = "";
		for (int n = 0; n < _num; n++) {

			_defName += _defaultStrings[n, 0] + "*";
			_def1 += _defaultStrings[n, 1] + "*";
			_def2 += _defaultStrings[n, 2] + "*";
		}

		string _Default = _defName + "#" + _def1 + "#" + _def2;
		PlayerPrefs.SetInt("_count", _num);
		PlayerPrefs.SetString("_defaults", _Default);
		_exDefaults = _num + "¿" + _Default;
	}

	private static void _SaveInputs() {
		int _num = _inputLength + 1;
		// *** save input configuration ***
		string _descr = "";
		string _inp = "";
		string _alt_inp = "";
		string _inpStr = "";
		string _alt_inpStr = "";

		for (int n = 0; n < _num; n++) {
			// make the strings
			_descr += _inputName[n] + "*";
			_inp += _inputPrimary[n] + "*";
			_alt_inp += _inputSecondary[n] + "*";
			_inpStr += _axisPrimary[n] + "*";
			_alt_inpStr += _axisSecondary[n] + "*";
		}

		// save the strings to the PlayerPrefs
		PlayerPrefs.SetString("_descr", _descr);
		PlayerPrefs.SetString("_inp", _inp);
		PlayerPrefs.SetString("_alt_inp", _alt_inp);
		PlayerPrefs.SetString("_inpStr", _inpStr);
		PlayerPrefs.SetString("_alt_inpStr", _alt_inpStr);
		_exInputs = _descr + "¿" + _inp + "¿" + _alt_inp + "¿" + _inpStr + "¿" + _alt_inpStr;
	}

	public static string externalInputs {
		get {
			return _exAllowDuplicates + "æ" + _exAxis + "æ" + _exAxisInverted + "æ" + _exDefaults + "æ" + _exInputs + "æ" + _exCalibrations;
			//string tmpExternalString = _exAllowDuplicates + "æ" + _exAxis + "æ" + _exAxisInverted + "æ" + _exDefaults + "æ" + _exInputs + "æ" + _exCalibrations;
			//return tmpExternalString;
		}
	}

	public static void LoadExternal(string externString) {
		string[] tmpExternalStrings = externString.Split('æ');
		_exAllowDuplicates = tmpExternalStrings[0];
		_exAxis = tmpExternalStrings[1];
		_exAxisInverted = tmpExternalStrings[2];
		_exDefaults = tmpExternalStrings[3];
		_exInputs = tmpExternalStrings[4];
		_exCalibrations = tmpExternalStrings[5];
		_LoadExternalInputs();
	}

	private static void _LoadInputs() {

		if (PlayerPrefs.HasKey("_dubl")) {
			if (PlayerPrefs.GetString("_dubl") == "True") {
				allowDuplicates = true;
			} else {
				allowDuplicates = false;
			}
		}

		int _count = PlayerPrefs.GetInt("_count");
		_inputLength = _count - 1;

		string _defaults = PlayerPrefs.GetString("_defaults");
		string[] ar_defs = _defaults.Split('#');
		string[] ar_defName = ar_defs[0].Split('*');
		string[] ar_defPrime = ar_defs[1].Split('*');
		string[] ar_defSec = ar_defs[2].Split('*');

		for (int n = 0; n < ar_defName.Length - 1; n++) {
			_SetDefaultKey(n, ar_defName[n], ar_defPrime[n], ar_defSec[n]);
		}

		if (PlayerPrefs.HasKey("_inp")) {
			string _descr = PlayerPrefs.GetString("_descr");
			string _inp = PlayerPrefs.GetString("_inp");
			string _alt_inp = PlayerPrefs.GetString("_alt_inp");
			string _inpStr = PlayerPrefs.GetString("_inpStr");
			string _alt_inpStr = PlayerPrefs.GetString("_alt_inpStr");

			string[] ar_descr = _descr.Split('*');
			string[] ar_inp = _inp.Split('*');
			string[] ar_alt_inp = _alt_inp.Split('*');
			string[] ar_inpStr = _inpStr.Split('*');
			string[] ar_alt_inpStr = _alt_inpStr.Split('*');

			for (int n = 0; n < ar_descr.Length - 1; n++) {
				if (ar_descr[n] == _defaultStrings[n, 0]) {
					_inputName[n] = ar_descr[n];
					_inputPrimary[n] = ConvertString2Key(ar_inp[n]);
					_inputSecondary[n] = ConvertString2Key(ar_alt_inp[n]);
					_axisPrimary[n] = ar_inpStr[n];
					_axisSecondary[n] = ar_alt_inpStr[n];
				}
			}

			// fixes inputs when defaults are beeing changed
			for (int m = 0; m < ar_defName.Length - 1; m++) {
				for (int n = 0; n < ar_descr.Length - 1; n++) {
					if (ar_descr[n] == _defaultStrings[m, 0]) {
						_inputName[m] = ar_descr[n];
						_inputPrimary[m] = ConvertString2Key(ar_inp[n]);
						_inputSecondary[m] = ConvertString2Key(ar_alt_inp[n]);
						_axisPrimary[m] = ar_inpStr[n];
						_axisSecondary[m] = ar_alt_inpStr[n];
					}
				}
			}
		}

		if (PlayerPrefs.HasKey("_axis")) {

			string _invAx = PlayerPrefs.GetString("_axInv");
			string[] _axInv = _invAx.Split('*');
			string _ax = PlayerPrefs.GetString("_axis");

			string[] _axis = _ax.Split('#');
			string[] _axName = _axis[0].Split('*');
			string[] _axNeg = _axis[1].Split('*');
			string[] _axPos = _axis[2].Split('*');

			int _axCount = int.Parse(_axis[3]);
			for (int n = 0; n < _axCount; n++) {
				int _neg = int.Parse(_axNeg[n]);
				int _pos = int.Parse(_axPos[n]);
				_SetAxis(n, _axName[n], _neg, _pos);
				if (_axInv[n] == "True") {
					_invertAxis[n] = true;
				} else {
					_invertAxis[n] = false;
				}
			}
		}

		if (PlayerPrefs.HasKey("_indAxSens")) {
			string _tmpAxisSens = PlayerPrefs.GetString("_indAxSens");
			string[] _arrAxisSens = _tmpAxisSens.Split('*');
			for (int n = 0; n < _arrAxisSens.Length - 1; n++) {
				_individualAxisSens[n] = float.Parse(_arrAxisSens[n]);
			}
		}

		// calibration loading
		if (PlayerPrefs.HasKey("_saveCals")) {
			string _saveCals = PlayerPrefs.GetString("_saveCals");
			string[] _saveCalsArr = _saveCals.Split('*');
			for (int n = 1; n <= _saveCalsArr.Length - 2; n++) {
				_axisType[n] = int.Parse(_saveCalsArr[n]);
			}
		}

		_cKeysLoaded = true;
	}

	private static void _LoadExternalInputs() {
		_externalSaving = true;
		// splitting the external strings
		string[] _es1 = _exAxis.Split('¿');
		string[] _es3 = _exDefaults.Split('¿');
		string[] _es4 = _exInputs.Split('¿');

		if (_exAllowDuplicates == "True") {
			allowDuplicates = true;
		} else {
			allowDuplicates = false;
		}
		int _count = int.Parse(_es3[0]);
		_inputLength = _count - 1;

		string _defaults = _es3[1];
		string[] ar_defs = _defaults.Split('#');
		string[] ar_defName = ar_defs[0].Split('*');
		string[] ar_defPrime = ar_defs[1].Split('*');
		string[] ar_defSec = ar_defs[2].Split('*');

		for (int n = 0; n < ar_defName.Length - 1; n++) {
			_SetDefaultKey(n, ar_defName[n], ar_defPrime[n], ar_defSec[n]);
		}

		if (!string.IsNullOrEmpty(_es4[0])) {
			string _descr = _es4[0];
			string _inp = _es4[1];
			string _alt_inp = _es4[2];
			string _inpStr = _es4[3];
			string _alt_inpStr = _es4[4];

			string[] ar_descr = _descr.Split('*');
			string[] ar_inp = _inp.Split('*');
			string[] ar_alt_inp = _alt_inp.Split('*');
			string[] ar_inpStr = _inpStr.Split('*');
			string[] ar_alt_inpStr = _alt_inpStr.Split('*');

			for (int n = 0; n < ar_descr.Length - 1; n++) {
				if (ar_descr[n] == _defaultStrings[n, 0]) {
					_inputName[n] = ar_descr[n];
					_inputPrimary[n] = ConvertString2Key(ar_inp[n]);
					_inputSecondary[n] = ConvertString2Key(ar_alt_inp[n]);
					_axisPrimary[n] = ar_inpStr[n];
					_axisSecondary[n] = ar_alt_inpStr[n];
				}
			}

			// fixes inputs when defaults are beeing changed
			for (int m = 0; m < ar_defName.Length - 1; m++) {
				for (int n = 0; n < ar_descr.Length - 1; n++) {
					if (ar_descr[n] == _defaultStrings[m, 0]) {
						_inputName[m] = ar_descr[n];
						_inputPrimary[m] = ConvertString2Key(ar_inp[n]);
						_inputSecondary[m] = ConvertString2Key(ar_alt_inp[n]);
						_axisPrimary[m] = ar_inpStr[n];
						_axisSecondary[m] = ar_alt_inpStr[n];
					}
				}
			}
		}



		if (!string.IsNullOrEmpty(_es1[0])) {

			string _invAx = _exAxisInverted;
			string[] _axInv = _invAx.Split('*');
			string _ax = _es1[0];

			string[] _axis = _ax.Split('#');
			string[] _axName = _axis[0].Split('*');
			string[] _axNeg = _axis[1].Split('*');
			string[] _axPos = _axis[2].Split('*');

			int _axCount = int.Parse(_axis[3]);
			for (int n = 0; n < _axCount; n++) {
				int _neg = int.Parse(_axNeg[n]);
				int _pos = int.Parse(_axPos[n]);
				_SetAxis(n, _axName[n], _neg, _pos);
				if (_axInv[n] == "true") {
					_invertAxis[n] = true;
				} else {
					_invertAxis[n] = false;
				}
			}
		}

		if (!string.IsNullOrEmpty(_es1[1])) {
			string _tmpAxisSens = _es1[1];
			string[] _arrAxisSens = _tmpAxisSens.Split('*');
			for (int n = 0; n < _arrAxisSens.Length - 1; n++) {
				_individualAxisSens[n] = float.Parse(_arrAxisSens[n]);
			}
		}

		// calibration loading
		if (!string.IsNullOrEmpty(_exCalibrations)) {
			string _saveCals = _exCalibrations;
			string[] _saveCalsArr = _saveCals.Split('*');
			for (int n = 1; n <= _saveCalsArr.Length - 2; n++) {
				_axisType[n] = int.Parse(_saveCalsArr[n]);
			}
		}
		_cKeysLoaded = true;
	}


	public static void ResetInputs() {
		_CreatecObject(); // if cInput doesn't exist, create it
		// reset inputs to default values
		for (int n = 0; n < _inputLength + 1; n++) {
			_SetKey(n, _defaultStrings[n, 0], _defaultStrings[n, 1], _defaultStrings[n, 2]);
		}

		for (int n = 0; n < _axisLength; n++) {
			_invertAxis[n] = false;
		}

		Clear();
		_SaveDefaults();
		_SaveInputs();
		_SaveAxInverted();
	}

	public static void Clear() {
		PlayerPrefs.DeleteKey("_axInv");
		PlayerPrefs.DeleteKey("_axis");
		PlayerPrefs.DeleteKey("_indAxSens");
		PlayerPrefs.DeleteKey("_count");
		PlayerPrefs.DeleteKey("_defaults");
		PlayerPrefs.DeleteKey("_descr");
		PlayerPrefs.DeleteKey("_inp");
		PlayerPrefs.DeleteKey("_alt_inp");
		PlayerPrefs.DeleteKey("_inpStr");
		PlayerPrefs.DeleteKey("_alt_inpStr");
		PlayerPrefs.DeleteKey("_dubl");
		PlayerPrefs.DeleteKey("_saveCals");
	}

	#endregion

	#region InvertAxis and IsAxisInverted functions

	// this sets the inversion of axisName to invertedStatus
	public static bool AxisInverted(string axisName, bool invertedStatus) {
		_CreatecObject(); // if cInput doesn't exist, create it
		int index = _FindAxisByDescription(axisName);
		if (index > -1) {
			_invertAxis[index] = invertedStatus;
			_SaveAxInverted();
			return invertedStatus;
		}

		// if we got this far then the string didn't match and there's a problem.
		Debug.LogWarning("Couldn't find an axis match for " + axisName + " while trying to set inversion status. Is it possible you typed it wrong?");
		return false;
	}

	// this just returns inversion status of axisName
	public static bool AxisInverted(string axisName) {
		_CreatecObject(); // if cInput doesn't exist, create it
		int index = _FindAxisByDescription(axisName);
		if (index > -1) {
			return _invertAxis[index];
		}

		// if we got this far then the string didn't match and there's a problem.
		Debug.LogWarning("Couldn't find an axis match for " + axisName + " while trying to get inversion status. Is it possible you typed it wrong?");
		return false;
	}

	#endregion

	#region OnGUI, Update, createcObject, ShowMenu, and related functions

	#region OnGUI
	void OnGUI() {
		if (_showMenu) {
			GUI.skin = cSkin;
			int _amnt = (Mathf.Clamp(_inputLength, 2, 10)) * 15;
			windowRect = new Rect(50, _amnt / 2, Screen.width - 100, Screen.height - _amnt);
			windowRect = GUILayout.Window(0, windowRect, MenuWindow, "");

			if (showPopUp) {
				GUI.Window(0, new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 150, 400, 300), popUp, "");
			}
		}


	}

	void popUp(int windowID) {
		GUILayout.Space(50);
		GUILayout.Box("Please leave all analog inputs in their neutral positions.");
		GUILayout.Space(30);

		if (GUILayout.Button("Click here when ready.", _menuButtonsString)) {
			Calibrate();
			showPopUp = false;
		}
	}
	void MenuWindow(int windowID) {
		GUILayout.BeginHorizontal("box");
		float _buttonWidth = (windowRect.width / 3) - 50;
		GUILayout.Label("Action", _menuHeaderString, GUILayout.Width(_buttonWidth + 8));
		GUILayout.Label("Primary", _menuHeaderString, GUILayout.Width(_buttonWidth + 8));
		GUILayout.Label("Secondary", _menuHeaderString, GUILayout.Width(_buttonWidth + 8));
		GUILayout.EndHorizontal();

		_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
		GUILayout.BeginHorizontal();

		GUILayout.BeginVertical();

		for (int n = 0; n < cInput.length; n++) {
			GUILayout.BeginHorizontal();
			GUILayout.Label(cInput.GetText(n, 0), _menuActionsString, GUILayout.Width(_buttonWidth));

			if (GUILayout.Button(cInput.GetText(n, 1), _menuInputsString, GUILayout.Width(_buttonWidth)) && Input.GetMouseButtonUp(0)) {
				if (Time.realtimeSinceStartup > mouseTimer) {
					cInput.ChangeKey(n, 1);
				}
			}

			if (GUILayout.Button(cInput.GetText(n, 2), _menuInputsString, GUILayout.Width(_buttonWidth)) && Input.GetMouseButtonUp(0)) {
				if (Time.realtimeSinceStartup > mouseTimer) {
					cInput.ChangeKey(n, 2);
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();

		GUILayout.Space(20);
		GUILayout.BeginHorizontal("textfield");

		if (GUILayout.Button("Reset to defaults", _menuButtonsString, GUILayout.Width(_buttonWidth + 10)) && Input.GetMouseButtonUp(0)) {
			cInput.ResetInputs();
		}

		if (GUILayout.Button("Calibrate joysticks", _menuButtonsString, GUILayout.Width(_buttonWidth + 10)) && Input.GetMouseButtonUp(0)) {
			showPopUp = true;
		}

		if (GUILayout.Button("Close", _menuButtonsString, GUILayout.Width(_buttonWidth + 10)) && Input.GetMouseButtonUp(0)) {
			_showMenu = false;
		}

		GUILayout.EndHorizontal();
	}

	#endregion

	void Update() {

		if (_scanning && _cScanInput == 0) {
			string _prim;
			string _sec;

			if (string.IsNullOrEmpty(_axisPrimary[_cScanIndex])) {
				_prim = _inputPrimary[_cScanIndex].ToString();
			} else {
				_prim = _axisPrimary[_cScanIndex];
			}

			if (string.IsNullOrEmpty(_axisSecondary[_cScanIndex])) {
				_sec = _inputSecondary[_cScanIndex].ToString();
			} else {
				_sec = _axisSecondary[_cScanIndex];
			}

			_ChangeKey(_cScanIndex, _inputName[_cScanIndex], _prim, _sec);

			_scanning = false;
		}

		if (!_scanning) {
			CheckInputs();
		}

		if (_cScanInput != 0) {
			_InputScans();
		}
	}

	#region ShowMenu functions

	public static bool ShowMenu() {
		_CreatecObject(); // if cInput doesn't exist, create it
		return _showMenu;
	}

	#region overloaded ShowMenu functions

	public static void ShowMenu(bool state) {
		ShowMenu(state, _menuHeaderString, _menuActionsString, _menuInputsString, _menuButtonsString);
	}

	public static void ShowMenu(bool state, string menuHeader) {
		ShowMenu(state, menuHeader, _menuActionsString, _menuInputsString, _menuButtonsString);
	}

	public static void ShowMenu(bool state, string menuHeader, string menuActions) {
		ShowMenu(state, menuHeader, menuActions, _menuInputsString, _menuButtonsString);
	}

	public static void ShowMenu(bool state, string menuHeader, string menuActions, string menuInputs) {
		ShowMenu(state, menuHeader, menuActions, menuInputs, _menuButtonsString);
	}

	#endregion overloaded showMenu functions

	public static void ShowMenu(bool state, string menuHeader, string menuActions, string menuInputs, string menuButtons) {
		_CreatecObject(); // if cInput doesn't exist, create it

		_menuHeaderString = menuHeader;
		_menuActionsString = menuActions;
		_menuInputsString = menuInputs;
		_menuButtonsString = menuButtons;

		_showMenu = state;
	}

	#endregion

	private static void _CreatecObject() {
		if (!_cObjectOn) {
			GameObject cInputObject;
			if (!GameObject.Find("cInput")) {
				cInputObject = new GameObject();
			} else {
				cInputObject = GameObject.Find("cInput");
			}

			cInputObject.name = "cInput";
			cInputObject.AddComponent<cInput>();
			_cObjectOn = true;
		}
	}

	private void _CheckingDuplicates(int _num, int _count) {
		if (allowDuplicates) {
			return;
		}

		for (int n = 0; n < length; n++) {
			if (_count == 1) {
				if (_num != n && _inputPrimary[_num] == _inputPrimary[n]) {
					_inputPrimary[n] = KeyCode.None;
				}

				if (_inputPrimary[_num] == _inputSecondary[n]) {
					_inputSecondary[n] = KeyCode.None;
				}
			}

			if (_count == 2) {
				if (_inputSecondary[_num] == _inputPrimary[n]) {
					_inputPrimary[n] = KeyCode.None;
				}

				if (_num != n && _inputSecondary[_num] == _inputSecondary[n]) {
					_inputSecondary[n] = KeyCode.None;
				}
			}
		}
	}

	private void _CheckingDuplicateStrings(int _num, int _count) {
		if (allowDuplicates) {
			return;
		}

		for (int n = 0; n < length; n++) {
			if (_count == 1) {
				if (_num != n && _axisPrimary[_num] == _axisPrimary[n]) {
					_axisPrimary[n] = "";
					_inputPrimary[n] = KeyCode.None;
				}

				if (_axisPrimary[_num] == _axisSecondary[n]) {
					_axisSecondary[n] = "";
					_inputSecondary[n] = KeyCode.None;
				}
			}

			if (_count == 2) {
				if (_axisSecondary[_num] == _axisPrimary[n]) {
					_axisPrimary[n] = "";
					_inputPrimary[n] = KeyCode.None;
				}

				if (_num != n && _axisSecondary[_num] == _axisSecondary[n]) {
					_axisSecondary[n] = "";
					_inputSecondary[n] = KeyCode.None;
				}
			}
		}
	}

	private void _InputScans() {
		if (Input.GetKey(KeyCode.Escape)) {
			if (_cScanInput == 1) {
				_inputPrimary[_cScanIndex] = KeyCode.None;
				_axisPrimary[_cScanIndex] = "";
				_cScanInput = 0;
			}

			if (_cScanInput == 2) {
				_inputSecondary[_cScanIndex] = KeyCode.None;
				_axisSecondary[_cScanIndex] = "";
				_cScanInput = 0;
			}
		}

		// keyboard + mouse + joystick button scanning
		if (_scanning && Input.anyKeyDown && !Input.GetKey(KeyCode.Escape)) {
			KeyCode _key = KeyCode.None;

			for (int i = (int)KeyCode.None; i < 450; i++) {
				KeyCode _ckey = (KeyCode)i;
				if (_ckey.ToString().StartsWith("Mouse")) {
					mouseTimer = Time.realtimeSinceStartup + 0.15f;
					if (!_allowMouseButtons) {
						continue;
					}
				} else if (_ckey.ToString().StartsWith("Joystick")) {
					if (!_allowJoystickButtons) {
						continue;
					}
				} else if (!_allowKeyboard) {
					continue;
				}

				if (Input.GetKeyDown(_ckey)) {
					_key = _ckey;
				}
			}

			if (_key != KeyCode.None) {
				bool _keyCleared = true;
				// check if the entered key is forbidden
				for (int b = 0; b < _forbiddenKeys.Count; b++) {
					if (_key == _forbiddenKeys[b]) {
						_keyCleared = false;
					}
				}

				if (_keyCleared) {
					if (_cScanInput == 1) {
						_inputPrimary[_cScanIndex] = _key;
						_axisPrimary[_cScanIndex] = "";
						_CheckingDuplicates(_cScanIndex, _cScanInput);
					}

					if (_cScanInput == 2) {
						_inputSecondary[_cScanIndex] = _key;
						_axisSecondary[_cScanIndex] = "";
						_CheckingDuplicates(_cScanIndex, _cScanInput);
					}
				}

				_cScanInput = 0;
			}
		}

		// mouse scroll wheel scanning (considered to be a mousebutton)
		if (_allowMouseButtons) {
			if (Input.GetAxis("Mouse Wheel") > 0 && !Input.GetKey(KeyCode.Escape)) {
				if (_cScanInput == 1) {
					_axisPrimary[_cScanIndex] = "Mouse Wheel Up";
				}

				if (_cScanInput == 2) {
					_axisSecondary[_cScanIndex] = "Mouse Wheel Up";
				}

				_CheckingDuplicateStrings(_cScanIndex, _cScanInput);
				_cScanInput = 0;
			} else if (Input.GetAxis("Mouse Wheel") < 0 && !Input.GetKey(KeyCode.Escape)) {
				if (_cScanInput == 1) {
					_axisPrimary[_cScanIndex] = "Mouse Wheel Down";
				}

				if (_cScanInput == 2) {
					_axisSecondary[_cScanIndex] = "Mouse Wheel Down";
				}

				_CheckingDuplicateStrings(_cScanIndex, _cScanInput);
				_cScanInput = 0;
			}
		}

		// mouse axis scanning
		if (_allowMouseAxis) {
			if (Input.GetAxis("Mouse Horizontal") < -deadzone && !Input.GetKey(KeyCode.Escape)) {
				if (_cScanInput == 1) {
					_axisPrimary[_cScanIndex] = "Mouse Left";
				}

				if (_cScanInput == 2) {
					_axisSecondary[_cScanIndex] = "Mouse Left";
				}

				_CheckingDuplicateStrings(_cScanIndex, _cScanInput);
				_cScanInput = 0;
			} else if (Input.GetAxis("Mouse Horizontal") > deadzone && !Input.GetKey(KeyCode.Escape)) {
				if (_cScanInput == 1) {
					_axisPrimary[_cScanIndex] = "Mouse Right";
				}

				if (_cScanInput == 2) {
					_axisSecondary[_cScanIndex] = "Mouse Right";
				}

				_CheckingDuplicateStrings(_cScanIndex, _cScanInput);
				_cScanInput = 0;
			}

			if (Input.GetAxis("Mouse Vertical") > deadzone && !Input.GetKey(KeyCode.Escape)) {
				if (_cScanInput == 1) {
					_axisPrimary[_cScanIndex] = "Mouse Up";
				}

				if (_cScanInput == 2) {
					_axisSecondary[_cScanIndex] = "Mouse Up";
				}

				_CheckingDuplicateStrings(_cScanIndex, _cScanInput);
				_cScanInput = 0;
			} else if (Input.GetAxis("Mouse Vertical") < -deadzone && !Input.GetKey(KeyCode.Escape)) {
				if (_cScanInput == 1) {
					_axisPrimary[_cScanIndex] = "Mouse Down";
				}

				if (_cScanInput == 2) {
					_axisSecondary[_cScanIndex] = "Mouse Down";
				}

				_CheckingDuplicateStrings(_cScanIndex, _cScanInput);
				_cScanInput = 0;
			}

		}

		// joystick axis scanning
		if (_allowJoystickAxis) {
			for (int i = 1; i < _numGamepads; i++) {
				for (int j = 1; j <= 10; j++) {
					string _joystring = _joyStrings[i, j];
					string _joystringPos = _joyStringsPos[i, j];
					string _joystringNeg = _joyStringsNeg[i, j];
					float axis = _GetCalibratedAxisInput(_joystring);
					if (_scanning && Mathf.Abs(axis) > deadzone && !Input.GetKey(KeyCode.Escape)) {
						if (_cScanInput == 1) {
							if (axis > deadzone) {
								_axisPrimary[_cScanIndex] = _joystringPos;
							} else if (axis < -deadzone) {
								_axisPrimary[_cScanIndex] = _joystringNeg;
							}

							_CheckingDuplicateStrings(_cScanIndex, _cScanInput);
							_cScanInput = 0;
							break;
						} else if (_cScanInput == 2) {
							if (axis > deadzone) {
								_axisSecondary[_cScanIndex] = _joystringPos;
							} else if (axis < -deadzone) {
								_axisSecondary[_cScanIndex] = _joystringNeg;
							}

							_CheckingDuplicateStrings(_cScanIndex, _cScanInput);
							_cScanInput = 0;
							break;
						}
					}
				}
			}
		}
	}

	#endregion

}

#region Keys: Easy access to KeyCode strings required by cInput

public class Keys : MonoBehaviour {

	#region keyboard input values
	public static string None = "None";
	public static string Alpha0 = "Alpha0";
	public static string Alpha1 = "Alpha1";
	public static string Alpha2 = "Alpha2";
	public static string Alpha3 = "Alpha3";
	public static string Alpha4 = "Alpha4";
	public static string Alpha5 = "Alpha5";
	public static string Alpha6 = "Alpha6";
	public static string Alpha7 = "Alpha7";
	public static string Alpha8 = "Alpha8";
	public static string Alpha9 = "Alpha9";
	public static string AltGr = "AltGr";
	public static string Ampersand = "Ampersand";
	public static string Apostrophe = "Quote"; // alternative for and equivalent to Quote
	public static string ArrowDown = "DownArrow"; // alternative for and equivalent to DownArrow
	public static string ArrowLeft = "LeftArrow"; // alternative for and equivalent to LeftArrow
	public static string ArrowRight = "RightArrow"; // alternative for and equivalent to RightArrow
	public static string ArrowUp = "UpArrow"; // alternative for and equivalent to UpArrow
	public static string Asterisk = "Asterisk";
	public static string AtSymbol = "At";
	public static string BackQuote = "BackQuote";
	public static string Backslash = "Backslash";
	public static string Backspace = "Backspace";
	public static string Break = "Break";
	public static string CapsLock = "CapsLock";
	public static string Caret = "Caret";
	public static string Clear = "Clear";
	public static string Colon = "Colon";
	public static string Comma = "Comma";
	public static string Delete = "Delete";
	public static string Dollar = "Dollar";
	public static string DoubleQuote = "DoubleQuote";
	public static string DownArrow = "DownArrow";
	public static string End = "End";
	public static string Enter = "Return"; // alternative for and equivalent to Return
	public static string EqualSign = "Equals";
	public static string Escape = "Escape";
	public static string Exclaim = "Exclaim";
	public static string ExclamationMark = "Exclaim"; // alternative for and equivalent to Exclaim
	public static string F1 = "F1";
	public static string F2 = "F2";
	public static string F3 = "F3";
	public static string F4 = "F4";
	public static string F5 = "F5";
	public static string F6 = "F6";
	public static string F7 = "F7";
	public static string F8 = "F8";
	public static string F9 = "F9";
	public static string F10 = "F10";
	public static string F11 = "F11";
	public static string F12 = "F12";
	public static string F13 = "F13";
	public static string F14 = "F14";
	public static string F15 = "F15";
	public static string ForwardSlash = "Slash"; // alternative for and equivalent to Slash
	public static string GreaterThan = "Greater";
	public static string Hash = "Hash";
	public static string Help = "Help";
	public static string Home = "Home";
	public static string Insert = "Insert";
	public static string Keypad0 = "Keypad0";
	public static string Keypad1 = "Keypad1";
	public static string Keypad2 = "Keypad2";
	public static string Keypad3 = "Keypad3";
	public static string Keypad4 = "Keypad4";
	public static string Keypad5 = "Keypad5";
	public static string Keypad6 = "Keypad6";
	public static string Keypad7 = "Keypad7";
	public static string Keypad8 = "Keypad8";
	public static string Keypad9 = "Keypad9";
	public static string KeypadAsterisk = "KeypadMultiply"; // alternative for and equivalent to KeypadMultiply
	public static string KeypadDivide = "KeypadDivide";
	public static string KeypadEnter = "KeypadEnter";
	public static string KeypadEquals = "KeypadEquals";
	public static string KeypadMinus = "KeypadMinus";
	public static string KeypadMultiply = "KeypadMultiply";
	public static string KeypadPeriod = "KeypadPeriod";
	public static string KeypadPlus = "KeypadPlus";
	public static string KeypadSlash = "KeypadDivide"; // alternative for and equivalent to KeypadDivide
	public static string LeftAlt = "LeftAlt";
	public static string LeftApple = "LeftApple";
	public static string LeftArrow = "LeftArrow";
	public static string LeftBracket = "LeftBracket";
	public static string LeftControl = "LeftControl";
	public static string LeftParen = "LeftParen";
	public static string LeftShift = "LeftShift";
	public static string LeftWindows = "LeftWindows";
	public static string LessThan = "Less";
	public static string Menu = "Menu";
	public static string Minus = "Minus";
	public static string NumberSign = "Hash"; // alternative for and equivalent to Hash
	public static string Numlock = "Numlock";
	public static string PageDown = "PageDown";
	public static string PageUp = "PageUp";
	public static string Pause = "Pause";
	public static string Period = "Period";
	public static string Plus = "Plus";
	public static string PoundSign = "Hash"; // alternative for and equivalent to Hash
	public static string Print = "Print";
	public static string QuestionMark = "Question";
	public static string Quote = "Quote";
	public static string Return = "Return";
	public static string RightAlt = "RightAlt";
	public static string RightApple = "RightApple";
	public static string RightArrow = "RightArrow";
	public static string RightBracket = "RightBracket";
	public static string RightControl = "RightControl";
	public static string RightParen = "RightParen";
	public static string RightShift = "RightShift";
	public static string RightWindows = "RightWindows";
	public static string ScrollLock = "ScrollLock";
	public static string Semicolon = "Semicolon";
	public static string Slash = "Slash";
	public static string Space = "Space";
	public static string SysReq = "SysReq";
	public static string Tab = "Tab";
	public static string Underscore = "Underscore";
	public static string UpArrow = "UpArrow";
	public static string A = "A";
	public static string B = "B";
	public static string C = "C";
	public static string D = "D";
	public static string E = "E";
	public static string F = "F";
	public static string G = "G";
	public static string H = "H";
	public static string I = "I";
	public static string J = "J";
	public static string K = "K";
	public static string L = "L";
	public static string M = "M";
	public static string N = "N";
	public static string O = "O";
	public static string P = "P";
	public static string Q = "Q";
	public static string R = "R";
	public static string S = "S";
	public static string T = "T";
	public static string U = "U";
	public static string V = "V";
	public static string W = "W";
	public static string X = "X";
	public static string Y = "Y";
	public static string Z = "Z";

	#endregion

	#region mouse input values

	public static string Mouse0 = "Mouse0";
	public static string Mouse1 = "Mouse1";
	public static string Mouse2 = "Mouse2";
	public static string Mouse3 = "Mouse3";
	public static string Mouse4 = "Mouse4";
	public static string Mouse5 = "Mouse5";
	public static string Mouse6 = "Mouse6";
	public static string MouseUp = "Mouse Up";
	public static string MouseDown = "Mouse Down";
	public static string MouseLeft = "Mouse Left";
	public static string MouseRight = "Mouse Right";
	public static string MouseWheelUp = "Mouse Wheel Up";
	public static string MouseWheelDown = "Mouse Wheel Down";

	#endregion

	#region gamepad values

	#region gamepad buttons

	public static string JoystickButton0 = "JoystickButton0";
	public static string JoystickButton1 = "JoystickButton1";
	public static string JoystickButton2 = "JoystickButton2";
	public static string JoystickButton3 = "JoystickButton3";
	public static string JoystickButton4 = "JoystickButton4";
	public static string JoystickButton5 = "JoystickButton5";
	public static string JoystickButton6 = "JoystickButton6";
	public static string JoystickButton7 = "JoystickButton7";
	public static string JoystickButton8 = "JoystickButton8";
	public static string JoystickButton9 = "JoystickButton9";
	public static string JoystickButton10 = "JoystickButton10";
	public static string JoystickButton11 = "JoystickButton11";
	public static string JoystickButton12 = "JoystickButton12";
	public static string JoystickButton13 = "JoystickButton13";
	public static string JoystickButton14 = "JoystickButton14";
	public static string JoystickButton15 = "JoystickButton15";
	public static string JoystickButton16 = "JoystickButton16";
	public static string JoystickButton17 = "JoystickButton17";
	public static string JoystickButton18 = "JoystickButton18";
	public static string JoystickButton19 = "JoystickButton19";
	public static string Joystick1Button0 = "Joystick1Button0";
	public static string Joystick1Button1 = "Joystick1Button1";
	public static string Joystick1Button2 = "Joystick1Button2";
	public static string Joystick1Button3 = "Joystick1Button3";
	public static string Joystick1Button4 = "Joystick1Button4";
	public static string Joystick1Button5 = "Joystick1Button5";
	public static string Joystick1Button6 = "Joystick1Button6";
	public static string Joystick1Button7 = "Joystick1Button7";
	public static string Joystick1Button8 = "Joystick1Button8";
	public static string Joystick1Button9 = "Joystick1Button9";
	public static string Joystick1Button10 = "Joystick1Button10";
	public static string Joystick1Button11 = "Joystick1Button11";
	public static string Joystick1Button12 = "Joystick1Button12";
	public static string Joystick1Button13 = "Joystick1Button13";
	public static string Joystick1Button14 = "Joystick1Button14";
	public static string Joystick1Button15 = "Joystick1Button15";
	public static string Joystick1Button16 = "Joystick1Button16";
	public static string Joystick1Button17 = "Joystick1Button17";
	public static string Joystick1Button18 = "Joystick1Button18";
	public static string Joystick1Button19 = "Joystick1Button19";
	public static string Joystick2Button0 = "Joystick2Button0";
	public static string Joystick2Button1 = "Joystick2Button1";
	public static string Joystick2Button2 = "Joystick2Button2";
	public static string Joystick2Button3 = "Joystick2Button3";
	public static string Joystick2Button4 = "Joystick2Button4";
	public static string Joystick2Button5 = "Joystick2Button5";
	public static string Joystick2Button6 = "Joystick2Button6";
	public static string Joystick2Button7 = "Joystick2Button7";
	public static string Joystick2Button8 = "Joystick2Button8";
	public static string Joystick2Button9 = "Joystick2Button9";
	public static string Joystick2Button10 = "Joystick2Button10";
	public static string Joystick2Button11 = "Joystick2Button11";
	public static string Joystick2Button12 = "Joystick2Button12";
	public static string Joystick2Button13 = "Joystick2Button13";
	public static string Joystick2Button14 = "Joystick2Button14";
	public static string Joystick2Button15 = "Joystick2Button15";
	public static string Joystick2Button16 = "Joystick2Button16";
	public static string Joystick2Button17 = "Joystick2Button17";
	public static string Joystick2Button18 = "Joystick2Button18";
	public static string Joystick2Button19 = "Joystick2Button19";
	public static string Joystick3Button0 = "Joystick3Button0";
	public static string Joystick3Button1 = "Joystick3Button1";
	public static string Joystick3Button2 = "Joystick3Button2";
	public static string Joystick3Button3 = "Joystick3Button3";
	public static string Joystick3Button4 = "Joystick3Button4";
	public static string Joystick3Button5 = "Joystick3Button5";
	public static string Joystick3Button6 = "Joystick3Button6";
	public static string Joystick3Button7 = "Joystick3Button7";
	public static string Joystick3Button8 = "Joystick3Button8";
	public static string Joystick3Button9 = "Joystick3Button9";
	public static string Joystick3Button10 = "Joystick3Button10";
	public static string Joystick3Button11 = "Joystick3Button11";
	public static string Joystick3Button12 = "Joystick3Button12";
	public static string Joystick3Button13 = "Joystick3Button13";
	public static string Joystick3Button14 = "Joystick3Button14";
	public static string Joystick3Button15 = "Joystick3Button15";
	public static string Joystick3Button16 = "Joystick3Button16";
	public static string Joystick3Button17 = "Joystick3Button17";
	public static string Joystick3Button18 = "Joystick3Button18";
	public static string Joystick3Button19 = "Joystick3Button19";
	public static string Joystick4Button0 = "Joystick4Button0";
	public static string Joystick4Button1 = "Joystick4Button1";
	public static string Joystick4Button2 = "Joystick4Button2";
	public static string Joystick4Button3 = "Joystick4Button3";
	public static string Joystick4Button4 = "Joystick4Button4";
	public static string Joystick4Button5 = "Joystick4Button5";
	public static string Joystick4Button6 = "Joystick4Button6";
	public static string Joystick4Button7 = "Joystick4Button7";
	public static string Joystick4Button8 = "Joystick4Button8";
	public static string Joystick4Button9 = "Joystick4Button9";
	public static string Joystick4Button10 = "Joystick4Button10";
	public static string Joystick4Button11 = "Joystick4Button11";
	public static string Joystick4Button12 = "Joystick4Button12";
	public static string Joystick4Button13 = "Joystick4Button13";
	public static string Joystick4Button14 = "Joystick4Button14";
	public static string Joystick4Button15 = "Joystick4Button15";
	public static string Joystick4Button16 = "Joystick4Button16";
	public static string Joystick4Button17 = "Joystick4Button17";
	public static string Joystick4Button18 = "Joystick4Button18";
	public static string Joystick4Button19 = "Joystick4Button19";
	public static string Joystick5Button0 = "Joystick5Button0";
	public static string Joystick5Button1 = "Joystick5Button1";
	public static string Joystick5Button2 = "Joystick5Button2";
	public static string Joystick5Button3 = "Joystick5Button3";
	public static string Joystick5Button4 = "Joystick5Button4";
	public static string Joystick5Button5 = "Joystick5Button5";
	public static string Joystick5Button6 = "Joystick5Button6";
	public static string Joystick5Button7 = "Joystick5Button7";
	public static string Joystick5Button8 = "Joystick5Button8";
	public static string Joystick5Button9 = "Joystick5Button9";
	public static string Joystick5Button10 = "Joystick5Button10";
	public static string Joystick5Button11 = "Joystick5Button11";
	public static string Joystick5Button12 = "Joystick5Button12";
	public static string Joystick5Button13 = "Joystick5Button13";
	public static string Joystick5Button14 = "Joystick5Button14";
	public static string Joystick5Button15 = "Joystick5Button15";
	public static string Joystick5Button16 = "Joystick5Button16";
	public static string Joystick5Button17 = "Joystick5Button17";
	public static string Joystick5Button18 = "Joystick5Button18";
	public static string Joystick5Button19 = "Joystick5Button19";

	#endregion

	#region gamepad axes

	public static string Joy1Axis1Negative = "Joy1 Axis 1-";
	public static string Joy1Axis1Positive = "Joy1 Axis 1+";
	public static string Joy1Axis2Negative = "Joy1 Axis 2-";
	public static string Joy1Axis2Positive = "Joy1 Axis 2+";
	public static string Joy1Axis3Negative = "Joy1 Axis 3-";
	public static string Joy1Axis3Positive = "Joy1 Axis 3+";
	public static string Joy1Axis4Negative = "Joy1 Axis 4-";
	public static string Joy1Axis4Positive = "Joy1 Axis 4+";
	public static string Joy1Axis5Negative = "Joy1 Axis 5-";
	public static string Joy1Axis5Positive = "Joy1 Axis 5+";
	public static string Joy1Axis6Negative = "Joy1 Axis 6-";
	public static string Joy1Axis6Positive = "Joy1 Axis 6+";
	public static string Joy1Axis7Negative = "Joy1 Axis 7-";
	public static string Joy1Axis7Positive = "Joy1 Axis 7+";
	public static string Joy1Axis8Negative = "Joy1 Axis 8-";
	public static string Joy1Axis8Positive = "Joy1 Axis 8+";
	public static string Joy1Axis9Negative = "Joy1 Axis 9-";
	public static string Joy1Axis9Positive = "Joy1 Axis 9+";
	public static string Joy1Axis10Negative = "Joy1 Axis 10-";
	public static string Joy1Axis10Positive = "Joy1 Axis 10+";
	public static string Joy2Axis1Negative = "Joy2 Axis 1-";
	public static string Joy2Axis1Positive = "Joy2 Axis 1+";
	public static string Joy2Axis2Negative = "Joy2 Axis 2-";
	public static string Joy2Axis2Positive = "Joy2 Axis 2+";
	public static string Joy2Axis3Negative = "Joy2 Axis 3-";
	public static string Joy2Axis3Positive = "Joy2 Axis 3+";
	public static string Joy2Axis4Negative = "Joy2 Axis 4-";
	public static string Joy2Axis4Positive = "Joy2 Axis 4+";
	public static string Joy2Axis5Negative = "Joy2 Axis 5-";
	public static string Joy2Axis5Positive = "Joy2 Axis 5+";
	public static string Joy2Axis6Negative = "Joy2 Axis 6-";
	public static string Joy2Axis6Positive = "Joy2 Axis 6+";
	public static string Joy2Axis7Negative = "Joy2 Axis 7-";
	public static string Joy2Axis7Positive = "Joy2 Axis 7+";
	public static string Joy2Axis8Negative = "Joy2 Axis 8-";
	public static string Joy2Axis8Positive = "Joy2 Axis 8+";
	public static string Joy2Axis9Negative = "Joy2 Axis 9-";
	public static string Joy2Axis9Positive = "Joy2 Axis 9+";
	public static string Joy2Axis10Negative = "Joy2 Axis 10-";
	public static string Joy2Axis10Positive = "Joy2 Axis 10+";
	public static string Joy3Axis1Negative = "Joy3 Axis 1-";
	public static string Joy3Axis1Positive = "Joy3 Axis 1+";
	public static string Joy3Axis2Negative = "Joy3 Axis 2-";
	public static string Joy3Axis2Positive = "Joy3 Axis 2+";
	public static string Joy3Axis3Negative = "Joy3 Axis 3-";
	public static string Joy3Axis3Positive = "Joy3 Axis 3+";
	public static string Joy3Axis4Negative = "Joy3 Axis 4-";
	public static string Joy3Axis4Positive = "Joy3 Axis 4+";
	public static string Joy3Axis5Negative = "Joy3 Axis 5-";
	public static string Joy3Axis5Positive = "Joy3 Axis 5+";
	public static string Joy3Axis6Negative = "Joy3 Axis 6-";
	public static string Joy3Axis6Positive = "Joy3 Axis 6+";
	public static string Joy3Axis7Negative = "Joy3 Axis 7-";
	public static string Joy3Axis7Positive = "Joy3 Axis 7+";
	public static string Joy3Axis8Negative = "Joy3 Axis 8-";
	public static string Joy3Axis8Positive = "Joy3 Axis 8+";
	public static string Joy3Axis9Negative = "Joy3 Axis 9-";
	public static string Joy3Axis9Positive = "Joy3 Axis 9+";
	public static string Joy3Axis10Negative = "Joy3 Axis 10-";
	public static string Joy3Axis10Positive = "Joy3 Axis 10+";
	public static string Joy4Axis1Negative = "Joy4 Axis 1-";
	public static string Joy4Axis1Positive = "Joy4 Axis 1+";
	public static string Joy4Axis2Negative = "Joy4 Axis 2-";
	public static string Joy4Axis2Positive = "Joy4 Axis 2+";
	public static string Joy4Axis3Negative = "Joy4 Axis 3-";
	public static string Joy4Axis3Positive = "Joy4 Axis 3+";
	public static string Joy4Axis4Negative = "Joy4 Axis 4-";
	public static string Joy4Axis4Positive = "Joy4 Axis 4+";
	public static string Joy4Axis5Negative = "Joy4 Axis 5-";
	public static string Joy4Axis5Positive = "Joy4 Axis 5+";
	public static string Joy4Axis6Negative = "Joy4 Axis 6-";
	public static string Joy4Axis6Positive = "Joy4 Axis 6+";
	public static string Joy4Axis7Negative = "Joy4 Axis 7-";
	public static string Joy4Axis7Positive = "Joy4 Axis 7+";
	public static string Joy4Axis8Negative = "Joy4 Axis 8-";
	public static string Joy4Axis8Positive = "Joy4 Axis 8+";
	public static string Joy4Axis9Negative = "Joy4 Axis 9-";
	public static string Joy4Axis9Positive = "Joy4 Axis 9+";
	public static string Joy4Axis10Negative = "Joy4 Axis 10-";
	public static string Joy4Axis10Positive = "Joy4 Axis 10+";
	public static string Joy5Axis1Negative = "Joy5 Axis 1-";
	public static string Joy5Axis1Positive = "Joy5 Axis 1+";
	public static string Joy5Axis2Negative = "Joy5 Axis 2-";
	public static string Joy5Axis2Positive = "Joy5 Axis 2+";
	public static string Joy5Axis3Negative = "Joy5 Axis 3-";
	public static string Joy5Axis3Positive = "Joy5 Axis 3+";
	public static string Joy5Axis4Negative = "Joy5 Axis 4-";
	public static string Joy5Axis4Positive = "Joy5 Axis 4+";
	public static string Joy5Axis5Negative = "Joy5 Axis 5-";
	public static string Joy5Axis5Positive = "Joy5 Axis 5+";
	public static string Joy5Axis6Negative = "Joy5 Axis 6-";
	public static string Joy5Axis6Positive = "Joy5 Axis 6+";
	public static string Joy5Axis7Negative = "Joy5 Axis 7-";
	public static string Joy5Axis7Positive = "Joy5 Axis 7+";
	public static string Joy5Axis8Negative = "Joy5 Axis 8-";
	public static string Joy5Axis8Positive = "Joy5 Axis 8+";
	public static string Joy5Axis9Negative = "Joy5 Axis 9-";
	public static string Joy5Axis9Positive = "Joy5 Axis 9+";
	public static string Joy5Axis10Negative = "Joy5 Axis 10-";
	public static string Joy5Axis10Positive = "Joy5 Axis 10+";

	#endregion

	#endregion

}

#endregion
