using System.Collections.Generic;
using UnityEngine;

namespace Event {
	public class EventScreen : MonoBehaviour {
		public List<UnityEngine.UI.Image> imageList;

		public void BuildScreen(List<Actor> actors) {
			for (int i = 0; i < imageList.Count; i++) {
				imageList[i].sprite = actors[i].sceneSprite[$"{gameObject.name}_{i+1}"];
			}
		}
	}
}