using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public enum STATE { RUNNING, STOP };
    public enum DIRECTION {UP,DOWN,LEFT,RIGHT };
    public class UserGUI : MonoBehaviour
    {
        private IUserAction action;
        private ScoreController scoreCtrl;
        public STATE state;
        private GUIStyle style1, style2;
        void Start()
        {
            action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
            style1 = new GUIStyle() { fontSize = 30 };
            style1.normal.textColor = Color.black;
            style2 = new GUIStyle("button") { fontSize = 15 };
            state = STATE.STOP;
            scoreCtrl = new ScoreController();
            scoreCtrl.reset();
			EventManager.ScoreChange += addScore;
			EventManager.GameoverChange += GameOver;
		}

        void Update()
        {
            if (state == STATE.RUNNING)
            {
                checkRoleMove();
            }
        }

        void checkRoleMove()
        {
            if (Input.GetKey(KeyCode.W))
            {
                action.moveRole(DIRECTION.UP);
            }
            else if(Input.GetKey(KeyCode.S))
            {
                action.moveRole(DIRECTION.DOWN);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                action.moveRole(DIRECTION.LEFT);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                action.moveRole(DIRECTION.RIGHT);
            }
        }

        void OnGUI()
        {
             GUI.Label(new Rect(20, 20, 120, 25), "score: " + scoreCtrl.getScore());
            if (state == STATE.STOP)
            {
                if (GUI.Button(new Rect(Screen.width / 2 -50 , Screen.height/2 -25, 100, 50), "Start", style2))
                {
                    action.Reset();
                    scoreCtrl.reset();
                    action.GameStart();
                    state = STATE.RUNNING;
                }
            }
        }

        public void GameOver()
        {
            state = STATE.STOP;
        }

        public void addScore()
        {
            scoreCtrl.addScore(1);
        }
    }
}
