using System.Collections.Generic;
using System;

namespace Event {
	[Serializable]
	public class Inventory {
		public List<Actor> actors;
		public List<Solution> items;

		public event Action<Actor> ActorRemoved;
		public event Action<Solution> ItemRemoved;

		public event Action<Actor> ActorAdded;
		public event Action<Solution> ItemAdded;

		public void AddActor(Actor actor) {
			actors.Add(actor);
			ActorAdded?.Invoke(actor);
		}

		public void AddItem(Solution solution) {
			items.Add(solution);
			ItemAdded?.Invoke(solution);
		}

		public void RemoveActor(Actor actor) {
			actors.Remove(actor);
			ActorRemoved?.Invoke(actor);
		}

		public void RemoveItem(Solution solution) {
			items.Remove(solution);
			ItemRemoved?.Invoke(solution);
		}
	}
}