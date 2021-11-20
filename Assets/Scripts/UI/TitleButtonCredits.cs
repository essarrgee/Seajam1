using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonCredits : TitleButton
{
	public CreditsDisplayManager credits;
	
	public override void Confirm()
	{
		if (audioManager != null) {
			audioManager.Play("MenuConfirm");
		}
		if (title != null) {
			title.lockInput = true;
		}
		if (credits != null) {
			credits.Show(true);
		}
	}
	
	protected override IEnumerator ConfirmLate()
	{
		yield return new WaitForSeconds(1);
	}
}
