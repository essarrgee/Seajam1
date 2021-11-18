using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
	
	protected Vector2 inputDirection;
	protected bool inputConfirm;
	
	protected float moveCooldown;
	protected float currentMoveCooldown;
	
	public TitleButton currentSelection;
	
	public bool lockInput = false;
	protected float currentInputCooldown;
	
    protected virtual void Awake()
	{
		inputDirection = new Vector2(0,0);
		inputConfirm = false;
		moveCooldown = 0.4f;
		currentMoveCooldown = moveCooldown;
		currentInputCooldown = 2f;
	}
	
	protected virtual void Start()
	{
		if (currentSelection != null) {
			currentSelection.Select(true);
		}
	}
	
	protected virtual void Update()
	{
		if (!lockInput && currentInputCooldown <= 0) {
			inputDirection.x = Input.GetAxisRaw("Horizontal");
			inputDirection.y = Input.GetAxisRaw("Vertical");
			inputConfirm = Input.GetKey(KeyCode.Space);
		}
		else {
			inputDirection = new Vector2(0,0);
			inputConfirm = false;
		}
		
		if (inputDirection.y <= 0.2f && inputDirection.y >= -0.2f) {
			currentMoveCooldown = 0;
		}
		
		if (currentSelection != null && currentMoveCooldown <= 0
		&& currentInputCooldown <= 0) {
			TitleButton previousSelection = null;
			if (inputDirection.y > 0.2f) {
				previousSelection = currentSelection;
				currentSelection = currentSelection.upSelection;
			}
			else if (inputDirection.y < -0.2f) {
				previousSelection = currentSelection;
				currentSelection = currentSelection.downSelection;
			}
			if (previousSelection != null && currentSelection != null) {
				currentMoveCooldown = moveCooldown;
				previousSelection.Select(false);
				currentSelection.Select(true);
			}
		}
		
		if (inputConfirm && currentSelection && !lockInput && currentInputCooldown <= 0) {
			lockInput = true;
			currentSelection.Confirm();
		}
		
		currentMoveCooldown = 
			(currentMoveCooldown > 0) ? currentMoveCooldown - Time.deltaTime : 0;
		currentInputCooldown = 
			(currentInputCooldown > 0) ? currentInputCooldown - Time.deltaTime : 0;
	}
}
