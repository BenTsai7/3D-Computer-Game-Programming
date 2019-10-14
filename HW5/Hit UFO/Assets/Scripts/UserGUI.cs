using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Disk
{
    public class UserGUI : MonoBehaviour
    {
        private IUserAction action;
        private GUIStyle style1, style2;
        void Start()
        {
            action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
            style1 = new GUIStyle() { fontSize = 30 };
            style1.normal.textColor = Color.black;
            style2 = new GUIStyle("button") { fontSize = 15 };
        }

        void Update()
        {
            if (action.getState() == 0)
            {
                checkHit();
            }
        }

        void checkHit()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 mp = Input.mousePosition;
                Camera ca = Camera.main;
                Ray ray = ca.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    ((FirstController)SSDirector.GetInstance().CurrentSceneController).hitDisk(hit.transform.gameObject);
                }
            }
        }

        void OnGUI()
        {
            if (action.getState() == 0)
            {
                GUI.Label(new Rect(20, 20, 120, 25), "score: " + ((FirstController)SSDirector.GetInstance().CurrentSceneController).scoreCtrl.score,style1);
                return;
            }
            if (action.getState() == 1)
            {
                if (GUI.Button(new Rect(Screen.width / 2 -50 , Screen.height/2 -25, 100, 50), "Start", style2))
                {
                    action.Reset();
                    action.GameStart();
                }
            }
        }
    }
}
