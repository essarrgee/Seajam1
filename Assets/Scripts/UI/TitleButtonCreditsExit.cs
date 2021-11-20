using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonCreditsExit : TitleButton
{
	public CreditsDisplayManager credits;
	
	public override void Confirm()
	{
		if (audioManager != null) {
			audioManager.Play("MenuConfirm");
		}
		if (title != null) {
			title.lockInput = false;
		}
		if (credits != null) {
			credits.Show(false);
		}
	}
	
	protected override IEnumerator ConfirmLate()
	{
		yield return new WaitForSeconds(1);
	}
}