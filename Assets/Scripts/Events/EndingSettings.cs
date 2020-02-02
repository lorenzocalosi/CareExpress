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
	public float fadeTime;
	public float imageTime;
	public AnimationCurve fadeCurve;
	public void PlayEndingMusic()
	{
		SoundManager.Instance.StopAllSounds();
		SoundManager.Instance.PlaySoudtrack("Ending");
	}

	
	public IEnumerator ShowImages(List<Actor> keys,Image imageToChange, GameObject finalSprite)
	{
		if (endingSprites != null) {
			yield return CameraHandler.Instance.StartCoroutine(CameraHandler.Instance.Fade(true, fadeTime));
		
			imageToChange.gameObject.SetActive(true);
		
			for (int i = 0; i < keys.Count; i++)
			{
				Sprite endingSprite = endingSprites[keys[i]];
				imageToChange.sprite = endingSprite;
				yield return CameraHandler.Instance.StartCoroutine(CameraHandler.Instance.Fade(false, fadeTime));
				yield return new WaitForSeconds(imageTime);
				yield return CameraHandler.Instance.StartCoroutine(CameraHandler.Instance.Fade(true, fadeTime));
				
			}
			imageToChange.gameObject.SetActive(false);
		}
		
		finalSprite.SetActive(true);
		finalSprite.GetComponent<EndingScreen>().ShowActorImages(keys);
		yield return CameraHandler.Instance.StartCoroutine(CameraHandler.Instance.Fade(false, fadeTime));
	}
}