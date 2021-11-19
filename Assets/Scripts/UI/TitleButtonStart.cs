using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleButtonStart : TitleButton
{
	
	public override void Confirm()
	{
		if (!confirmed) {
			confirmed = true;
			if (cameraAnimator != null) {
				cameraAnimator.SetTrigger("ZoomIn");
			}
			if (fadeAnimator != null) {
				fadeAnimator.SetTrigger("FadeIn");
			}
			if (audioManager != null) {
				audioManager.Play("MenuConfirm");
			}
			if (audioMaster != null) {
				audioMaster.FadeAllAudio(2.5f, 10);
			}
			StartCoroutine(ConfirmLate());
		}
	}
	
	protected override IEnumerator ConfirmLate()
	{
		yield return new WaitForSeconds(3f);
		
		SceneManager.LoadScene("Main");
	}
}
