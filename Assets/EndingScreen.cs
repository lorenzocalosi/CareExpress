using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Event;
using Sirenix.OdinInspector;

public class EndingScreen : SerializedMonoBehaviour
{
	public Dictionary<Actor, List<Image>> images;

    public void ShowActorImages(List<Actor> actors) {
		for (int i = 0; i < actors.Count; i++) {
			if (images.ContainsKey(actors[i])) {
				foreach (Image image in images[actors[i]]) {
					image.color = Color.white;
				}
			}
			else
			{
				foreach (Image image in images[actors[i]]) {
					image.color = Color.black;
				}
			}
		}
	}
}
