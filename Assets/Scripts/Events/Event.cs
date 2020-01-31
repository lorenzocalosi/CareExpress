using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Event {

	[CreateAssetMenu(fileName = "Event", menuName = "GGJ20/Event")]
	public class Event : ScriptableObject {

		[System.Serializable]
		public struct ActorGroup {
			public List<Actor> possibleActors;
		}

		public List<ActorGroup> actors;
		[AssetsOnly, LabelText("Event Prefab")]
		public GameObject basic;
		[AssetsOnly, LabelText("Success Prefab")]
		public GameObject success;
		[AssetsOnly, LabelText("Failure Prefab")]
		public GameObject failure;
		public Solution solution;
	}
}