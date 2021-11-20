using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewManager : MonoBehaviour
{
    public Animator fadeAnimator;
	public Animator continueDisplayAnimator;
	
	protected bool inputConfirm;
	
	protected float currentInputCooldown;
	protected bool canInput;
	protected bool confirmed;
	
	
	protected virtual void Awake()
	{
		currentInputCooldown = 3f;
		canInput = false;
		confirmed = false;
		
		inputConfirm = false;
	}
	
	protected virtual void Update()
	{
		if (currentInputCooldown <= 0) {
			inputConfirm = Input.GetButtonDown("Fire1");
				// Input.GetKeyDown(KeyCode.Space);
			
			if (inputConfirm) {
				Confirm();
			}
		}
		
		currentInputCooldown = (currentInputCooldown > 0) ? 
			currentInputCooldown - Time.deltaTime : 0;
			
		if (currentInputCooldown <= 0 && !canInput && continueDisplayAnimator != null) {
			canInput = true;
			continueDisplayAnimator.SetTrigger("Activate");
		}
	}
	
	protected virtual void Confirm()
	{
		if (!confirmed && currentInputCooldown <= 0) {
			confirmed = true;
			if (fadeAnimator != null) {
				fadeAnimator.SetTrigger("FadeIn");
			}
			StartCoroutine(ConfirmLate());
		}
	}
	
	protected virtual IEnumerator ConfirmLate()
	{
		yield return new WaitForSeconds(2f);
		
		SceneManager.LoadScene("Title");
	}
}
