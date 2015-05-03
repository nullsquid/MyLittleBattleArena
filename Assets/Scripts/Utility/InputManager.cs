using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InputManager : MonoBehaviour {
	private List <CharacterBase> activeCharacters = new List<CharacterBase>();
	public static InputManager instance;
	public InputScheme[] inputSchemes = new InputScheme[0];
	private int schemeToAssign = 0;
	private void Awake(){
		instance = this;
	}
	public void AssignPlayerInputs(CharacterBase playerReference){
		if (!activeCharacters.Contains(playerReference) && schemeToAssign < inputSchemes.Length){
			playerReference.inputScheme = inputSchemes[schemeToAssign];
			activeCharacters.Add(playerReference);
			schemeToAssign++;
		}
	}
}
[System.Serializable]
public class InputScheme {
	public KeyCode positiveHorizontalButton;
	public KeyCode negativeHorizontalButton;
	public KeyCode positiveVerticalButton;
	public KeyCode negativeVerticalButton;
	public KeyCode abilityButton1;
	public KeyCode abilityButton2;
}
