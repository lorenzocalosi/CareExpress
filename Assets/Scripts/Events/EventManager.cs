using System.Collections.Generic;
using System.Linq;

namespace Event {
	public class EventManager : Singleton<EventManager> {
		public enum ActorState {
			Pending,
			Failure,
			Success
		}

		public int triesLimit;

		public struct ActorStateTries {
			public ActorState actorState;
			public int tries;

			public ActorStateTries(ActorState actorState, int tries) {
				this.actorState = actorState;
				this.tries = tries;
			}
		}

		public Dictionary<Actor, ActorStateTries> progress;

		private void Start() {
			foreach (var key in progress.Keys.ToList()) {
				progress[key] = new ActorStateTries(ActorState.Pending, 0);
			}
		}
	}
}