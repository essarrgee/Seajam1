using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandlerMaster : MonoBehaviour
{
    public List<AudioHandler> audioHandlerList;
	
	protected virtual void Awake()
	{
		
	}
	
	public virtual void FadeAllAudio(float time, int increment)
	{
		for (int i=0; i<audioHandlerList.Count; i++) {
			audioHandlerList[i].FadeAudio(time, increment);
		}
	}
}
