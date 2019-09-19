using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAD
{
    public class UserGUI : MonoBehaviour
    {
        public int state;
        private IUserAction action;
        private GUIStyle style1, style2;
        void Start()
        {
            action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
            style1 = new GUIStyle() { fontSize = 30 };
            style1.normal.textColor = Color.white;
            style2 = new GUIStyle("button") { fontSize = 15 };
        }

        void OnGUI()
        {

            if (state == 0) return;

            if (state == 2)
            {
                GUI.Label(new Rect(Screen.width / 2 - 90, Screen.height / 2 - 70, 100, 50), "Game Over", style1);
                if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 100, 50), "Restart", style2))
                {
                    action.Restart();
                    state = 0;
                }
            }
            else if (state == 1)
            {
                GUI.Label(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 120, 100, 50), "You Win", style1);
                if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 100, 50), "Restart", style2))
                {
                    action.Restart();
                    state = 0;
                }
            }
        }
    }
}
