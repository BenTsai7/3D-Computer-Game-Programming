using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public class SSAction : ScriptableObject
    {
        public bool destory = false;
        public bool enabled = true;
        public GameObject gameObject { get; set; }
        public ISSActionCallBack callback { get; set; }
        protected SSAction() { }
        public virtual void Start() { throw new System.NotImplementedException(); }
        public virtual void Update() { throw new System.NotImplementedException(); }
    }

    public enum SSActionEventType : int { Started, Completed };
    public interface ISSActionCallBack
    {
        void SSActionEvent(SSAction sourse, SSActionEventType events = SSActionEventType.Completed);
    }

    public class SSActionManager : MonoBehaviour
    {
        private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
        private List<SSAction> AddList = new List<SSAction>();
        private List<int> DeleteList = new List<int>();

        protected void Update()
        {
            foreach (SSAction ac in AddList) actions[ac.GetInstanceID()] = ac;
            AddList.Clear();
            foreach (KeyValuePair<int, SSAction> kv in actions)
            {
                SSAction ac = kv.Value;
                if (ac.destory)
                {
                    DeleteList.Add(ac.GetInstanceID());
                }
                else if (ac.enabled)
                {
                    ac.Update();
                }
            }

            foreach (int key in DeleteList)
            {
                SSAction ac = actions[key];
                actions.Remove(key);
                Object.Destroy(ac);
            }
            DeleteList.Clear();
        }


		public void Start() { }

        public void RunAction(GameObject gameObject, SSAction action, ISSActionCallBack manager)
        {
            action.gameObject = gameObject;
            action.callback = manager;
            AddList.Add(action);
            action.Start();
        }

		public void DestroyAll()
		{
			foreach (KeyValuePair<int, SSAction> kv in actions)
			{
				SSAction ac = kv.Value;
				ac.destory = true;
			}
		}
	}
    public class CCActionManager : SSActionManager, ISSActionCallBack
    {
        public FirstController sceneController;

        public new void Start()
        {
            sceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
            sceneController.actionManager = this;
        }

        protected new void Update()
        {
            base.Update();
        }

        public void SSActionEvent(SSAction sourse, SSActionEventType events = SSActionEventType.Completed)
        {
            
        }

    }

    public class CCMove : SSAction
    {
        public float speed; //移动速度；
        public Vector3 initialpos;
		public int state; //0表示巡逻，1表示追逐玩家，2表示返回巡逻初始点
		public enum Direction { EAST, NORTH, WEST, SOUTH };
		public Direction direction;//巡逻方向'
		public float movelen = 20f;
		public Vector3 dst;
		public Patrolman p;
		

		public static CCMove GetSSAction(Patrolman p)
        {
            CCMove action = ScriptableObject.CreateInstance<CCMove>();
            action.p = p;
			action.state = 2;
			action.direction = Direction.EAST;
			action.speed = 10;
			return action;
        }

        public override void Start()
        {
			switch (p.area)
			{
				case 1:
					initialpos = new Vector3(42f, 0f, 17f);
					break;
				case 2:
					initialpos = new Vector3(13f, 0f, 17f);
					break;
				case 3:
					initialpos = new Vector3(-25f, 0f, 17f);
					break;
				case 4:
					initialpos = new Vector3(42f, 0f, 67f);
					break;
				case 5:
					initialpos = new Vector3(13f, 0f, 67f);
					break;
				case 6:
					initialpos = new Vector3(-25f, 0f, 67f);
					break;

			}
			dst = p.obj.transform.position + new Vector3(-movelen, 0f, 0f);

		}

        public override void Update()
        {
			p.obj.GetComponent<Animator>().SetBool("Walk Forward", true);
			if (((FirstController)SSDirector.GetInstance().CurrentSceneController).role.area == p.area) {
				state = 1;
			}
			if (state == 0)
			{
				if (p.obj.transform.position != dst)
				{
					p.obj.transform.position = Vector3.MoveTowards(p.obj.transform.position, dst, speed * Time.deltaTime);
				}
				else
				{
					switch (direction)
					{
						case Direction.EAST:
							direction = Direction.SOUTH;
							dst = p.obj.transform.position + new Vector3(0f, 0f, movelen);
							break;
						case Direction.SOUTH:
							direction = Direction.WEST;
							dst = p.obj.transform.position + new Vector3(movelen,0f,0f);
							break;
							
						case Direction.WEST:
							direction = Direction.NORTH;
							dst = p.obj.transform.position + new Vector3(0f,0f, -movelen);
							break;
						case Direction.NORTH:
							direction = Direction.EAST;
							dst = p.obj.transform.position + new Vector3(-movelen,0f,0f);
							break;
					}
				}
			}
			else if (state == 1)
			{
				if (((FirstController)SSDirector.GetInstance().CurrentSceneController).role.area != p.area) {
					EventManager.Escape();
					state = 2;
				}
				else {	
					Vector3 pos = ((FirstController)SSDirector.GetInstance().CurrentSceneController).role.obj.transform.position;
					p.obj.transform.position = Vector3.MoveTowards(p.obj.transform.position, pos, speed * Time.deltaTime);
					if ((p.obj.transform.position - pos).magnitude < 5)
					{
						EventManager.Gameover();
					}
				}

			}
			else if (state == 2)
			{
				p.obj.transform.position = Vector3.MoveTowards(p.obj.transform.position, initialpos, speed * Time.deltaTime);
				if (p.obj.transform.position == initialpos)
				{
					state = 0;
					direction = Direction.EAST;
					dst = p.obj.transform.position + new Vector3(-movelen, 0f, 0f);
				}
			}
 
        }
    }

}
