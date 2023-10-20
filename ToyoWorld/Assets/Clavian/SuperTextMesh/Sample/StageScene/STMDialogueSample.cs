using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class STMDialogueSample : MonoBehaviour {
	public SuperTextMesh textMesh;
	public KeyCode advanceKey = KeyCode.Return;
	public Image advanceKeySprite;
	private Vector3 advanceKeyStartScale = Vector3.one;
	public Vector3 advanceKeyScale = Vector3.one;
	public float advanceKeyTime = 1f;
	[SerializeField] GameObject chatPanel;

	[TextArea]
	public string[] lines;
	public int currentLine = 0;

	void Start () {
		advanceKeyStartScale = advanceKeySprite.transform.localScale;
		Apply();
	}
	public void CompletedDrawing(){
		Debug.Log("I completed reading! Done!");
	}
	public void CompletedUnreading(){
		Debug.Log("I completed unreading!! Bye!");
		Apply(); //go to next line
	}
	public void Apply () {
		
		//isDoneFading = false;
		if((currentLine == lines.Length))
		{
			chatPanel.gameObject.SetActive(false);
		}
		try
		{
			textMesh.Text = lines[currentLine]; //invoke accessor so rebuild() is called
		} catch(Exception e)
		{
			Debug.Log(e);
		}
		currentLine++; //move to next line of dialogue...
	}
	void Update () {
		if(Input.GetKey(advanceKey)){
			advanceKeySprite.transform.localScale = advanceKeyScale; //scale key if held
		}else{
			advanceKeySprite.transform.localScale = Vector3.Lerp(advanceKeySprite.transform.localScale, advanceKeyStartScale, Time.deltaTime * advanceKeyTime);
		}
		if(Input.GetKeyDown(advanceKey)){
			if(textMesh.reading){ //is text being read out, and player has lifted up the key in this block of text?
				textMesh.SpeedRead(); //show all text, or speed up
			}
			if(!textMesh.reading && !textMesh.unreading){ //call Continue(), if no need to continue, advance to next box. only when button is pressed, too
				if(!textMesh.Continue()){
					textMesh.UnRead();
				}else{
					Debug.Log("CONTINUING NOW");
				}
			}
		}
		if(Input.GetKeyUp(advanceKey)){
			textMesh.RegularRead(); //return to normal reading speed, if possible.
		}
	}
}