using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    //主角的Model类
    public class Role
    {
        public GameObject obj;
		public int area;
        public Role()
        {
			area = 2;
            obj = Object.Instantiate(Resources.Load("Role", typeof(GameObject)), new Vector3(0f, 0f, 45f), Quaternion.identity) as GameObject;
        }
        public void reset()
        {
            obj.transform.position = new Vector3(0f, 0f, 45f);
        }
    }
    //巡逻兵的Model类
    public class Patrolman
    {
        public GameObject obj;
        public int area;
        public int state;
        public Patrolman(int area)
        {
            obj = Object.Instantiate(Resources.Load("Patrolman", typeof(GameObject)), new Vector3(100f, 100f, 100f), Quaternion.identity) as GameObject;
            setArea(area);
		}
        public void setArea(int area)
        {
            state = 0;
            this.area = area;
            Vector3 new_position = Vector3.zero;
            switch (area)
            {
                case 1:
                    new_position = new Vector3(31f, 0, 22f);
                    break;
                case 2:
                    new_position = new Vector3(-3f, 0, 22f);
                    break;
                case 3:
                    new_position = new Vector3(-36f, 0, 22f);
                    break;
                case 4:
                    new_position = new Vector3(31f, 0, 75f);
                    break;
                case 5:
                    new_position = new Vector3(-3f, 0, 75f);
                    break;
                case 6:
                    new_position = new Vector3(-36f, 0, 75f);
                    break;
            }
            obj.transform.position = new_position;
        }
        public void reset()
        {
            setArea(this.area);
        }
        public void patrol()
        {
            CCMove ccMove;
            ccMove = CCMove.GetSSAction(this);
            var actionManager = ((FirstController)SSDirector.GetInstance().CurrentSceneController).actionManager;
            actionManager.RunAction(this.obj, ccMove, (CCActionManager)actionManager);
        }
    }
}
