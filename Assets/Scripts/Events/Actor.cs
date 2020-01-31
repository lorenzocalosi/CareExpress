using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Event {
	[CreateAssetMenu(fileName = "Actor", menuName = "GGJ20/Actor")]
	public class Actor : SerializedScriptableObject {
		[DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, KeyLabel = "EventName_Index", ValueLabel = "EventSprite")]
		public Dictionary<string, Sprite> sceneSprite;
	}
}