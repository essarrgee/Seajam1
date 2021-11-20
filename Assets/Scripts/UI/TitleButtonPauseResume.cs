using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonPauseResume : TitleButton
{	
	public TitleManager pauseScreen;

    public override void Confirm()
	{
		if (title != null) {
			title.lockInput = true;
		}
		if (pauseScreen != null) {
			pauseScreen.Pause(false);
		}
	}
	
	protected override IEnumerator ConfirmLate()
	{
		yield return new WaitForSeconds(1);
	}

}
