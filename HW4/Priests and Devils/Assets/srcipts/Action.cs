using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAD
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
    }
    public class CCActionManager : SSActionManager, ISSActionCallBack
    {
        public FirstController sceneController;

        public void MoveObject(GameObject obj,Vector3 dst)
        {
            CCMove MoveAction = CCMove.GetSSAction(dst);
            this.RunAction(obj, MoveAction, this);
        }
        /*public void moveBoat(GameObject boat, int to_or_from)
        {
            if (to_or_from == -1)
            {
                CCBoatMove boatMoveToRight = CCBoatMove.GetSSAction(new Vector3(5, 1, 0));
                this.RunAction(boat, boatMoveToRight, this);
            }
            else
            {
                CCBoatMove boatMoveToLeft = CCBoatMove.GetSSAction(new Vector3(-5, 1, 0));
                this.RunAction(boat, boatMoveToLeft, this);
            }
        }

        public void moveCharacter(GameObject obj, Vector3 dest)
        {
            CCCharacterMove characterMove = CCCharacterMove.GetSSAction(dest);
            this.RunAction(obj, characterMove, this);
        }*/

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
        public Vector3 dst;
        public float speed = 15; //移动速度；

        public static CCMove GetSSAction(Vector3 dst)
        {
            CCMove action = ScriptableObject.CreateInstance<CCMove>();
            action.dst = dst;
            return action;
        }

        public override void Start()
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, dst, speed * Time.deltaTime);
        }

        public override void Update()
        {
            if (gameObject.transform.position == dst)
            {
                this.destory = true;
                this.callback.SSActionEvent(this);
            }
            else
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, dst, speed * Time.deltaTime);
            }
        }
    }

}
