using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsDisplayManager : MonoBehaviour
{
    protected Vector2 inputDirection;
	protected bool inputConfirm;
	
	public Transform displayContents;
	
	public TitleButton currentSelection;
	
	public bool lockInput = false;
	
	protected virtual void Awake()
	{
		inputDirection = new Vector2(0,0);
		inputConfirm = false;
	}
	
	protected virtual void Start()
	{
		if (currentSelection != null) {
			currentSelection.Select(true);
		}
	}
	
	protected virtual void Update()
	{
		if (!lockInput) {
			inputDirection.x = Input.GetAxisRaw("Horizontal");
			inputDirection.y = Input.GetAxisRaw("Vertical");
			inputConfirm = Input.GetButtonDown("Fire1");
				// Input.GetKeyDown(KeyCode.Space);
		}
		else {
			inputDirection = new Vector2(0,0);
			inputConfirm = false;
		}
		
		if (displayContents != null) {
			if (inputDirection.y >= 0.2f) {
				displayContents.localPosition = 
					(displayContents.localPosition.y > 0) ?
					displayContents.localPosition - new Vector3(0,0.2f,0) :
					new Vector3(0,0,0);
			}
			else if (inputDirection.y <= -0.2f) {
				displayContents.localPosition = 
					(displayContents.localPosition.y < 37) ?
					displayContents.localPosition + new Vector3(0,0.2f,0) :
					new Vector3(0,37,0);
			}
		}
		
		if (inputConfirm && currentSelection && !lockInput) {
			currentSelection.Confirm();
		}
		
	}
	
	public virtual void Show(bool state)
	{
		lockInput = !state;
		gameObject.SetActive(state);
	}
}
