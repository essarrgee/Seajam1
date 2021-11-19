using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
	public List<AudioSource> audioSourceList;
	
	protected Dictionary<string, AudioSource> audioMap;
	protected Dictionary<AudioSource, float> audioInitialVolumeMap;
	
	protected Transform audioContainerObject;
	
    protected virtual void Awake()
	{
		audioMap = new Dictionary<string, AudioSource>();
		audioInitialVolumeMap = new Dictionary<AudioSource, float>();
		
		audioContainerObject = transform.Find("Audio");
		if (audioContainerObject != null) {
			for (int i=0; i < audioContainerObject.childCount; i++) {
				Transform audioObject = audioContainerObject.GetChild(i);
				AudioSource audioSource = 
					audioObject.GetComponent<AudioSource>();
				if (audioSource != null) {
					audioMap.Add(audioObject.name, audioSource);
					audioInitialVolumeMap.Add(audioSource, audioSource.volume);
				}
			}
		}
	}
	
	public virtual void Play(string audioName)
	{
		if (audioMap.ContainsKey(audioName)) {
			audioMap[audioName].Play(0);
		}
	}
	
	public virtual void FadeAudio(float time, int increment)
	{
		Dictionary<string, AudioSource>.ValueCollection audioValues = 
			audioMap.Values;
		float timeIncrement = time/(float)increment;
		
		for (int i=0; i<increment; i++) {
			StartCoroutine(
				SetVolume(timeIncrement*i, 1-((float)(i+1)/(float)increment), audioValues)
			);
		}
		
	}
	
	protected virtual IEnumerator SetVolume(float delay, float volumeScale,
	Dictionary<string, AudioSource>.ValueCollection audioValues)
	{
		yield return new WaitForSeconds(delay);
		
		foreach (AudioSource audio in audioValues) {
			if (audioInitialVolumeMap.ContainsKey(audio)) {
				audio.volume = audioInitialVolumeMap[audio]*volumeScale;
			}
		}
	}
}
