using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSoundEvent : MonoBehaviour, ISelectHandler, IPointerDownHandler, IPointerEnterHandler
{
	// controller movement
	public void OnSelect(BaseEventData eventData)
	{
		SoundManager.Instance.PlayAudioClipOneShot("ButtonHighlight");
	}

	// mouse movement
	public void OnPointerEnter(PointerEventData eventData)
	{
		SoundManager.Instance.PlayAudioClipOneShot("ButtonHighlight");
	}

	// mouseclick
	public void OnPointerDown(PointerEventData eventData)
	{
		SoundManager.Instance.PlayAudioClipOneShot("ButtonSelection");
	}
}