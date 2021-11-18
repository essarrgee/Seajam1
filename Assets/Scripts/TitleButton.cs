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
	
	protected TextMeshProUGUI textDisplay;
	protected string initialText;
	
	protected bool confirmed;
	
	protected virtual void Awake()
	{
		textDisplay = GetComponent<TextMeshProUGUI>();
		if (textDisplay != null) {
			initialText = textDisplay.text;
		}
		
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
