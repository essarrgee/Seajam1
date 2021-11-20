using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float walkSpeed = 0.2f;
	
	protected Rigidbody rb;
	
	protected Vector2 inputDirection;
	protected Vector3 moveDirection;
	protected bool inputInventory;
	protected bool inputPause;
	
	public bool lockInput = false;
	
	protected Transform cameraObject;
	protected Animator cameraAnimator;
	protected Transform model;
	
	protected GameObject gameManagerObject;
	protected GameManager gameManager;
	
	public Animator damageVignetteAnimator;
	
	public TitleManager pauseScreen;
	
	protected AudioHandler audioManager;
	
	protected float stunTime;
	
	
    protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody>();
		
		inputDirection = new Vector2(0,0);
		moveDirection = new Vector2(0,0);
		inputInventory = false;
		
		model = transform.Find("Model");
		cameraObject = transform.Find("CameraHandler");
		if (cameraObject != null) {
			cameraAnimator = cameraObject.GetComponent<Animator>();
		}
		
		gameManagerObject = GameObject.FindWithTag("GameController");
		if (gameManagerObject != null) {
			gameManager = gameManagerObject.GetComponent<GameManager>();
		}
		
		audioManager = GetComponent<AudioHandler>();
		
		stunTime = 0f;
	}
	
	protected virtual void Update()
	{
		if (!lockInput) {
			inputDirection.x = Input.GetAxisRaw("Horizontal");
			inputDirection.y = Input.GetAxisRaw("Vertical");
			inputInventory = Input.GetButton("Fire2");
				// Input.GetKey(KeyCode.LeftShift);
			inputPause = Input.GetButtonDown("Submit");
				// Input.GetKeyDown(KeyCode.Escape);
		}
		else {
			inputDirection = new Vector2(0,0);
			inputInventory = false;
			inputPause = false;
		}
		
		stunTime = (stunTime > 0) ? stunTime - Time.deltaTime : 0;

		
		if (model != null) {
			if (stunTime > 0) {
				model.gameObject.SetActive(
					(model.gameObject.activeSelf) ? false : true);
			}
			else {
				model.gameObject.SetActive(true);
			}
		}
		
		if (inputPause && pauseScreen != null && !lockInput) {
			lockInput = true;
			pauseScreen.gameObject.SetActive(true);
			pauseScreen.Pause(true);
		}
		
		if (gameManager != null && inputInventory) {
			gameManager.ShowTrash();
		}
		
	}
	
	protected virtual void FixedUpdate()
	{
		moveDirection = new Vector3(inputDirection.x,0,inputDirection.y);
		
		Vector3 newSpeed = moveDirection.normalized;
		Vector3 totalSpeed = 
			new Vector3(newSpeed.x, 0, newSpeed.z)*walkSpeed*
				(stunTime <= 0.1f ? 1 : 0);
		
		if (rb != null) {
			rb.AddForce(totalSpeed, ForceMode.Impulse);
		}
	}
	
	public virtual void Damage()
	{
		if (stunTime <= 0) {
			stunTime = 1.1f;
			if (cameraAnimator != null) {
				cameraAnimator.SetTrigger("Damage");
			}
			if (damageVignetteAnimator != null) {
				damageVignetteAnimator.SetTrigger("Damage");
			}
			if (audioManager != null) {
				audioManager.Play("Impact");
			}
		}
	}
}
