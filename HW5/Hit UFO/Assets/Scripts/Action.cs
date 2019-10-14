using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Disk
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

    public class IActionManager : SSActionManager, ISSActionCallBack
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

        public virtual void playDisk(DiskData disk)
        {

        }
    }

    public class CCActionManager : IActionManager
    {

        public override void playDisk(DiskData disk)
        {
            CCMove action = ScriptableObject.CreateInstance<CCMove>();
            action.disk = disk;
            action.speed = disk.ruler.speed;
            base.RunAction(disk.obj, action, (this));
        }
    }

    public class CCMove : SSAction
    {
        public float speed; //移动速度；
        public Vector3 dst;
        public DiskData disk;
 
        public override void Start()
        {
            int side = Random.Range(-1, 2);
            Vector3 dst1 = new Vector3(20, Random.Range(-1f, 3f), Random.Range(-2f, 0f));
            if (side < 1)
            {
                dst1.x = -dst1.x;
            }
            disk.obj.transform.position = new Vector3(-dst1.x, Random.Range(-1f, 3f), Random.Range(-2f, 0f));
            dst = dst1;
        }

        public override void Update()
        {
            if (gameObject.transform.position == dst)
            {
                this.destory = true;
                this.callback.SSActionEvent(this);
                DiskFactory.getInstance().freeDisk(disk);
            }
            else
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, dst, speed * Time.deltaTime);
            }
        }
    }

    public class PhysisActionManager : IActionManager
    {
        
        public override void playDisk(DiskData disk)
        {
            PhysisActionMove action = ScriptableObject.CreateInstance<PhysisActionMove>();
            action.disk = disk;
            base.RunAction(disk.obj, action, (this));
        }
    }

    public class PhysisActionMove : SSAction
    {
        public DiskData disk;

        public override void Start()
        {
            int side = Random.Range(-1, 2);
            Vector3 dst1 = new Vector3(19, Random.Range(5f, 7f), Random.Range(-3f, 0f));
            if (side < 1)
            {
                dst1.x = -dst1.x;
            }
            disk.obj.transform.position = dst1;
            if (disk.obj.GetComponent<Rigidbody>() == null)
                disk.obj.AddComponent<Rigidbody>();
            disk.obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (side < 1)
            {
                side = 1;
            }
            else
            {
                side = -1;
            }
            disk.obj.GetComponent<Rigidbody>().useGravity = false;

            disk.obj.GetComponent<Rigidbody>().AddForce(new Vector3(disk.ruler.speed*70*side, 0, 0), ForceMode.Force);
        }

        public override void Update()
        {
            if (disk.obj.transform.position.y < -20)
            {
                Destroy(disk.obj.GetComponent<Rigidbody>());
                this.destory = true;
                this.callback.SSActionEvent(this);
                DiskFactory.getInstance().freeDisk(disk);
            }
        }
    }
}
