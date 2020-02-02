using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraBehavior : MonoBehaviour {

	private Camera camera;
	private Vector3 startPosition;
	//Move Settings
	public float moveTime;
	public AnimationCurve moveCurve;
	//Zoom Settings
	public float zoomTime;
	public AnimationCurve zoomCurve;
	public float zoomAmount;
	//Fade Settings
	public Image fadeImage;
	public AnimationCurve fadeCurve;
	public float fadeTime;
	public bool isAnimating;

	private void Start() {
		camera = GetComponent<Camera>();
	}

	public void ZoomTowards(Vector3 zoomTarget, Action onFadeFinish) {
		if (!isAnimating) {
			isAnimating = true;		
			StartCoroutine(Fade(true, fadeTime, () => { onFadeFinish?.Invoke(); StartCoroutine(Fade(false, fadeTime / 2, () => { isAnimating = false; })); }));
		}
	}

	public void FadeOut(Action onFadeFinish) {
		StartCoroutine(Fade(true, fadeTime / 2, () => { onFadeFinish?.Invoke(); StartCoroutine(Fade(false, fadeTime / 2)); }));
	}

	private IEnumerator Move(Vector3 moveTarget, Action endAction = null) {
		float count = 0;
		startPosition = transform.position;
		while (count <= moveTime) {
			count += Time.deltaTime;
			transform.position = new Vector3(
				Mathf.Lerp(startPosition.x, moveTarget.x, moveCurve.Evaluate(count / moveTime)),
				Mathf.Lerp(startPosition.y, moveTarget.y, moveCurve.Evaluate(count / moveTime)),
				startPosition.z
			);
			yield return null;
		}
		transform.position = startPosition;
		endAction?.Invoke();
	}

	private IEnumerator Zoom(Action endAction = null) {
		float count = 0;
		float startSize = camera.orthographicSize;
		while (count <= zoomTime) {
			count += Time.deltaTime;
			camera.orthographicSize = Mathf.Lerp(startSize, zoomAmount, moveCurve.Evaluate(count / zoomTime));
			yield return null;
		}
		camera.orthographicSize = startSize;
		endAction?.Invoke();
	}

	private IEnumerator Fade(bool fadeIn, float time, Action endAction = null) {
		float count = 0;
		float startAlpha = fadeIn ? 0 : 1;
		float finishAlpha = fadeIn ? 1 : 0;
		while (count <= time) {
			count += Time.deltaTime;
			fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.Lerp(startAlpha, finishAlpha, fadeCurve.Evaluate(count / time)));
			yield return null;
		}
		endAction?.Invoke();
	}
}