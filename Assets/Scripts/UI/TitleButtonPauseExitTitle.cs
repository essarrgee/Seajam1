using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleButtonPauseExitTitle : TitleButton
{
	public TitleManager pauseScreen;
	
	public override void Confirm()
	{
		if (!confirmed) {
			confirmed = true;
			if (pauseScreen != null) {
				pauseScreen.Pause(false, true);
			}
			if (fadeAnimator != null) {
				fadeAnimator.SetTrigger("FadeIn");
			}
			StartCoroutine(ConfirmLate());
		}
	}
	
	protected override IEnumerator ConfirmLate()
	{
		yield return new WaitForSeconds(1.7f);
		
		SceneManager.LoadScene("Title");
	}
}
