using UnityEngine;

namespace Event {

	[CreateAssetMenu(fileName = "Event", menuName = "GGJ20/Event")]
	public class Event : ScriptableObject {
		public Actor actor;
		public Sprite background;
	}
}