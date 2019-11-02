using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
	public class EventManager : MonoBehaviour
	{
		private static EventManager _instance;
		public delegate void ScoreEvent();
		public static event ScoreEvent ScoreChange;
		public delegate void GameoverEvent();
		public static event GameoverEvent GameoverChange;
		public static void Escape()
		{
			if (ScoreChange != null)
			{
				ScoreChange();
			}
		}
		public static void Gameover()
		{
			if (GameoverChange != null)
			{
				GameoverChange();
			}
		}
	}
}