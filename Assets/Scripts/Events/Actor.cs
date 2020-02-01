using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Event {
	[CreateAssetMenu(fileName = "Actor", menuName = "GGJ20/Actor")]
	public class Actor : SerializedScriptableObject {
		public RuntimeAnimatorController animator;
		public Actor relatedActor;
		public Solution relatedSolution;
		public Sprite icon;
	}
}