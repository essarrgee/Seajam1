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
	
	public bool lockInput = false;
	
	protected Transform cameraObject;
	protected Animator cameraAnimator;
	protected Transform model;
	
	protected GameObject gameManagerObject;
	protected GameManager gameManager;
	
	public Animator damageVignetteAnimator;
	
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
		
		stunTime = 0f;
	}
	
	protected virtual void Update()
	{
		if (!lockInput) {
			inputDirection.x = Input.GetAxisRaw("Horizontal");
			inputDirection.y = Input.GetAxisRaw("Vertical");
			inputInventory = Input.GetKey(KeyCode.Space);
		}
		else {
			inputDirection = new Vector2(0,0);
			inputInventory = false;
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
		
	}
	
	protected virtual void FixedUpdate()
	{
		if (inputInventory && gameManager != null) {
			gameManager.ShowTrash();
		}
		
		moveDirection = new Vector3(inputDirection.x,0,inputDirection.y);
		
		Vector3 newSpeed = moveDirection.normalized;
		Vector3 totalSpeed = 
			new Vector3(newSpeed.x, 0, newSpeed.z)*walkSpeed*
				(stunTime <= 0.2f ? 1 : 0);
		
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
		}
	}
}
