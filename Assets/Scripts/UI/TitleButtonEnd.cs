using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleButtonEnd : TitleButton
{
	public float quitDelay = 0.75f;
	public TitleManager pauseScreen;
	
	public override void Confirm()
	{
		if (!confirmed) {
			confirmed = true;
			if (title != null) {
				title.lockInput = true;
			}
			if (pauseScreen != null) {
				pauseScreen.Pause(false, true);
			}
			if (cameraAnimator != null) {
				cameraAnimator.SetTrigger("ZoomOut");
			}
			if (fadeAnimator != null) {
				fadeAnimator.SetTrigger("FadeIn");
			}
			if (audioManager != null) {
				audioManager.Play("MenuConfirm");
			}
			StartCoroutine(ConfirmLate());
		}
	}
	
	protected override IEnumerator ConfirmLate()
	{
		yield return new WaitForSeconds(quitDelay);
		
		Application.Quit();
	}
}
