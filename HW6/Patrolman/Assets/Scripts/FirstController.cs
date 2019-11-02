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
using Patrol;
public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    public UserGUI usergui;
    public CCActionManager actionManager;
    public PatrolmanFactory patrolmanFactory;
    public Role role;//主角
    public Patrolman[] patrolmans;
    float Rolespeed;
    // the first scripts
    void Awake()
    {
        SSDirector director = SSDirector.GetInstance();
        patrolmanFactory = PatrolmanFactory.getInstance();
        director.CurrentSceneController = this;
        director.CurrentSceneController.LoadResources();
        usergui = gameObject.AddComponent<UserGUI>() as UserGUI;
        actionManager = gameObject.AddComponent<CCActionManager>() as CCActionManager;
        actionManager.Start();
        Rolespeed = 50;
    }

    // loading resources for the first scence
    public void LoadResources()
    {
        role = new Role();
		patrolmans = new Patrolman[6];
    }

    public void moveRole(DIRECTION direction)
    {
        //上下左右移动
        RaycastHit hit;
        if (direction == DIRECTION.UP)
        {
            if (Physics.Raycast(role.obj.transform.position, transform.TransformDirection(Vector3.forward), out hit, 2*Time.deltaTime * Rolespeed))
                return;
            Vector3 e_rot = transform.eulerAngles;
            e_rot.x = 0;
            e_rot.y = 180;
            e_rot.z = 0;
            role.obj.transform.eulerAngles = e_rot;
            role.obj.transform.Translate(Vector3.forward * Time.deltaTime * Rolespeed, Space.Self);
        }
        else if (direction == DIRECTION.DOWN)
        {
            if (Physics.Raycast(role.obj.transform.position, transform.TransformDirection(Vector3.back), out hit, 2*Time.deltaTime * Rolespeed))
                return;
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.x = 0;
            eulerAngles.y = 0;
            eulerAngles.z = 0;
            role.obj.transform.eulerAngles = eulerAngles;
            role.obj.transform.Translate(Vector3.forward * Time.deltaTime * Rolespeed, Space.Self);
        }
        else if (direction == DIRECTION.LEFT)
        {
            if (Physics.Raycast(role.obj.transform.position, transform.TransformDirection(new Vector3(-1,0,0)), out hit, 2 * Time.deltaTime * Rolespeed))
                return;
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.x = 0;
            eulerAngles.y = 90;
            eulerAngles.z = 0;
            role.obj.transform.eulerAngles = eulerAngles;
            role.obj.transform.Translate(Vector3.forward * Time.deltaTime * Rolespeed, Space.Self);
        }
        else if (direction == DIRECTION.RIGHT)
        {
            if (Physics.Raycast(role.obj.transform.position, transform.TransformDirection(new Vector3(1, 0, 0)), out hit, 2 * Time.deltaTime * Rolespeed))
                return;
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.x = 0;
            eulerAngles.y = -90;
            eulerAngles.z = 0;
            role.obj.transform.eulerAngles = eulerAngles;
            role.obj.transform.Translate(Vector3.forward * Time.deltaTime * Rolespeed, Space.Self);
        }
    }

    public void GameStart()
    {
		role.reset();
		for (int i = 0; i < 6; ++i)
        {
            patrolmans[i] = patrolmanFactory.getPatrolman(i + 1);
			patrolmans[i].patrol();
        }
    }
   

    public void Reset()
    {
        patrolmanFactory.reset();
    }

    // Use this for initialization
    void Start()
    {
		EventManager.GameoverChange += GameOver;
	}
	void GameOver()
	{
		actionManager.DestroyAll();
	}
    // Update is called once per frame
    void Update()
    {
		updateArea();
    }
	void updateArea() {
		if (usergui.state == STATE.STOP) return;
		float x = role.obj.transform.position.x;
		float z = role.obj.transform.position.z;
		int newArea;
		if (z < 57)
		{
			if (x > 16.7)
			{
				newArea = 1;
			}
			else if (x > -18)
			{
				newArea = 2;
			}
			else
			{
				newArea = 3;
			}
		}
		else
		{
			if (x > 16.7)
			{
				newArea = 4;
			}
			else if (x > -18)
			{
				newArea = 5;
			}
			else
			{
				newArea = 6;
			}
		}
		if (role.area != newArea)
		{
			role.area = newArea;
		}
	}
}
