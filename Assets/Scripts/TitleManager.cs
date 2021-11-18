using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
	
	protected Vector2 inputDirection;
	protected bool inputConfirm;
	protected bool inputPause;
	
	protected float moveCooldown;
	protected float currentMoveCooldown;
	
	public TitleButton currentSelection;
	
	protected GameObject playerObject;
	protected Player player;
	
	public bool lockInput = false;
	protected float currentInputCooldown;
	
    protected virtual void Awake()
	{
		inputDirection = new Vector2(0,0);
		inputConfirm = false;
		moveCooldown = 0.4f;
		currentMoveCooldown = moveCooldown;
		currentInputCooldown = 0.1f;
		
		playerObject = GameObject.FindWithTag("Player");
		if (playerObject != null) {
			player = playerObject.GetComponent<Player>();
		}
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
			inputConfirm = Input.GetKey(KeyCode.Space);
			inputPause = Input.GetKeyDown(KeyCode.Escape);
		}
		else {
			inputDirection = new Vector2(0,0);
			inputConfirm = false;
			inputPause = false;
		}
		
		if (inputDirection.y <= 0.2f && inputDirection.y >= -0.2f) {
			currentMoveCooldown = 0;
		}
		
		if (currentSelection != null && currentMoveCooldown <= 0) {
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
		
		if (inputPause && !lockInput) {
			Pause(false);
		}
		
		currentMoveCooldown = 
			(currentMoveCooldown > 0) ? currentMoveCooldown - Time.unscaledDeltaTime : 0;
		currentInputCooldown = 
			(currentInputCooldown > 0) ? currentInputCooldown - Time.unscaledDeltaTime : 0;
	}
	
	public virtual void Pause(bool state)
	{
		lockInput = !state;
		Time.timeScale = state ? 0 : 1;
		if (!state) {
			gameObject.SetActive(false);
		}
		if (player != null) {
			player.lockInput = state;
		}
	}
	
	public virtual void Pause(bool state, bool exiting)
	{
		lockInput = !state;
		Time.timeScale = state ? 0 : 1;
	}
}
