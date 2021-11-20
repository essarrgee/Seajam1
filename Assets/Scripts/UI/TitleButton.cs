using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public abstract class TitleButton : MonoBehaviour
{
    public TitleButton upSelection;
	public TitleButton downSelection;
	
	public Animator cameraAnimator;
	public Animator fadeAnimator;
	
	public TitleManager title;
	
	public AudioHandlerMaster audioMaster;
	protected AudioHandler audioManager;
	
	protected GameObject playerObject;
	protected Player player;
	
	protected TextMeshProUGUI textDisplay;
	protected string initialText;
	
	protected bool confirmed;
	
	
	protected virtual void Awake()
	{
		playerObject = GameObject.FindWithTag("Player");
		if (playerObject != null) {
			player = playerObject.GetComponent<Player>();
		}
		
		textDisplay = GetComponent<TextMeshProUGUI>();
		if (textDisplay != null) {
			initialText = textDisplay.text;
		}
		
		audioManager = GetComponent<AudioHandler>();
		
		confirmed = false;
	}
	
	public virtual void Select(bool state)
	{
		if (textDisplay != null) {
			if (state) {
				textDisplay.text = '>' + initialText + '<';
			}
			else {
				textDisplay.text = initialText;
			}
		}
	}
	
	public abstract void Confirm();
	
	protected abstract IEnumerator ConfirmLate();
}
