using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleButtonEnd : TitleButton
{
	
	public override void Confirm()
	{
		if (!confirmed) {
			confirmed = true;
			if (title != null) {
				title.lockInput = true;
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
		yield return new WaitForSeconds(0.75f);
		
		Application.Quit();
	}
}
