using System.Collections.Generic;
using UnityEngine;

namespace Event {
	[CreateAssetMenu(fileName = "SolutionCollection", menuName = "GGJ20/SolutionCollection")]
	public class SolutionCollection : SingletonScriptableObject<SolutionCollection> {
		public List<Solution> allSolutions;

		public List<Solution> GetRandomSolutions(Solution avoid) {
			List<Solution> list = new List<Solution>();
			List<Solution> filter = allSolutions.FindAll(x => x != avoid);
			//for (int i = 0; i < EventHandler.Instance.buttons.Count-1; i++) {
			//	list.Add(filter[Random.Range(0, filter.Count)]);
			//	filter = allSolutions.FindAll(x => x != avoid && !list.Contains(x));
			//}
			return list;
		}
	}
}