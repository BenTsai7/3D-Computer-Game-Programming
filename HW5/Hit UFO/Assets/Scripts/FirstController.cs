using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Scene Controller
/// Usage: host on a gameobject in the scene   
/// responsiablities:
///   acted as a scene manager for scheduling actors.log something ...
///   interact with the director and players
/// </summary>
using Disk;
public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    public int state;
    public UserGUI usergui;
    public IActionManager actionManager;
    public ScoreController scoreCtrl;
    public DiskFactory diskFactory;
    public int round;
    public Ruler ruler;
    public bool RoundFinished = true;
    public float time;
    public float interval;

    // the first scripts
    void Awake()
    {
        state = 1;
        interval = 7.0f;
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        director.CurrentSceneController.LoadResources();
        usergui = gameObject.AddComponent<UserGUI>() as UserGUI;
        actionManager = gameObject.AddComponent<PhysisActionManager>() as IActionManager;
        diskFactory = DiskFactory.getInstance();
        scoreCtrl = new ScoreController();
        actionManager.Start();
    }

    public int getState()
    {
        return state;
    }

    // loading resources for the first scence
    public void LoadResources()
    {

    }
    public void GameStart()
    {
        round = 1;
        state = 0;
        time = 10;
    }
    public void hitDisk(GameObject disk)
    {
        DiskData temp = diskFactory.getHitDisk(disk);
        if (temp != null)
        {
            GameObject obj = Object.Instantiate(Resources.Load("ParticleSystem", typeof(GameObject)),disk.transform.position, Quaternion.identity) as GameObject;
            scoreCtrl.addScore(temp.ruler.score);
            diskFactory.freeDisk(temp);
            Destroy(obj, 0.5f);
        }
    }
    public void FlySomeDisk(int round)
    {
        int num = (int) Random.Range(5f, 10f);
        ruler = new Ruler(round);
        for(int i = 0; i < num; ++i)
        {   
            FlyOneDisk();
        }
    }

    public void FlyOneDisk()
    {
        DiskData disk = diskFactory.getDisk(ruler);
        disk.fly();
    }

    public void Reset()
    {
        diskFactory.reset();
        scoreCtrl.score = 0;
    }
   
    void checkIfNeedFly()
    {
        if (time >= interval)
        {
            if (round <= 5)
            {
                FlySomeDisk(round);
            }
            ++round;
            time = 0;
        }
    }

    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (state != 0) return;
        if (round >5)
        {
            if (time > 5.0f)
            {
                state = 1;
                return;
            }
        }
        time += Time.deltaTime;
        checkIfNeedFly();
    }
}
