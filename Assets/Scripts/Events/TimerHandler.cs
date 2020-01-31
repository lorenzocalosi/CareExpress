using System.Collections;
using UnityEngine;

namespace Event {
	public class TimerHandler : Singleton<TimerHandler> {
		public float time;
		public Animator timer;
		private float count;
		private Coroutine coroutine;

		public void Wait(System.Action onFinish) {
			coroutine = StartCoroutine(WaitTime(onFinish));
		}

		public void Stop() {
			StopCoroutine(coroutine);
		}

		private IEnumerator WaitTime(System.Action onFinish) {
			count = 0;
			while (count <= time) {
				timer.SetFloat("timer", count);
				count += Time.deltaTime;
				yield return null;
			}
			onFinish.Invoke();
		}
	}
}