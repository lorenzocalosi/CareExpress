using System.Collections;
using System.Collections.Generic;
using Event;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EndingSettings", menuName = "GGJ20/EndingSettings")]
public class EndingSettings : SerializedScriptableObject
{
	public Dictionary<Actor, Sprite> endingSprites;
	public Image image;
	public Image blackImage;
	public float fadeTime;
	public float imageTime;

	public AnimationCurve fadeCurve;
	public void PlayEndingMusic()
	{
		SoundManager.Instance.StopAllSounds();
		SoundManager.Instance.PlaySoudtrack("Ending");
	}

	
	public IEnumerator ShowImages(List<Actor> keys)
	{
		if (endingSprites != null) {
			yield return CameraHandler.Instance.StartCoroutine(CameraHandler.Instance.Fade(true, fadeTime));
		
			image.gameObject.SetActive(true);
		
			for (int i = 0; i < keys.Count; i++)
			{
				Sprite endingSprite = endingSprites[keys[i]];
				image.sprite = endingSprite;
				yield return CameraHandler.Instance.StartCoroutine(CameraHandler.Instance.Fade(false, fadeTime));
				yield return new WaitForSeconds(imageTime);
				yield return CameraHandler.Instance.StartCoroutine(CameraHandler.Instance.Fade(true, fadeTime));
			}
		}
		
		Application.Quit();
	}
}