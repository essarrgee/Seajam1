using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
	protected Dictionary<string, float> dialogueTimingMap;
	protected Queue<string> dialogueQueue;
	
    protected TextMeshProUGUI dialogueBox;
	
	protected float textCooldown;
	
	protected virtual void Awake()
	{
		dialogueBox = GetComponent<TextMeshProUGUI>();
		
		dialogueTimingMap = new Dictionary<string, float>();
		dialogueQueue = new Queue<string>();
		textCooldown = 0f;
	}
	
	protected virtual void Update()
	{
		if (dialogueBox != null) {
			if (textCooldown <= 0) {
				dialogueBox.text = "";
			}
			if (dialogueQueue.Count > 0 && textCooldown <= 0) {
				string nextLine = dialogueQueue.Dequeue();
				textCooldown = dialogueTimingMap[nextLine];
				dialogueBox.text = nextLine;
				// dialogueBox.color = dialogueColorMap[nextLine];
				dialogueTimingMap.Remove(nextLine);
				// dialogueColorMap.Remove(nextLine);
			}
		}
			
		textCooldown = (textCooldown > 0) ? textCooldown - Time.deltaTime : 0;
	}
	
	public virtual void AddDialogue(string text, float timing, bool overrideAllText)
	{
		if (overrideAllText) {
			dialogueQueue.Clear();
		}
		dialogueTimingMap.Add(text, timing);
		// dialogueColorMap.Add(text, color);
		dialogueQueue.Enqueue(text);
	}
}
