using System.Collections.Generic;
using UnityEngine;

namespace Event {
	public class EventScreen : MonoBehaviour {
		public List<Animator> animatorList;

		public void BuildScreen(List<Actor> actors) {
			for (int i = 0; i < animatorList.Count; i++) {
				animatorList[i].runtimeAnimatorController = actors[i].animator;
			}
		}
	}
}