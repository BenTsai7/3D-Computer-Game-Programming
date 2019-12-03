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
using PAD;
public class FirstController : MonoBehaviour, ISceneController, IUserAction
{

    public GameObject scene;
    public RiverSide leftside;
    public RiverSide rightside;
    public Boat boat;
    public Character[] characters;
    public UserGUI usergui;
	public Node STATE;
	public Graph GRAPH;
    // the first scripts
    void Awake()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        director.CurrentSceneController.LoadResources();
        usergui = gameObject.AddComponent<UserGUI>() as UserGUI;
		STATE = new Node(3, 3, true);
		GRAPH = new Graph(3, 3);
	}

    // loading resources for the first scence
    public void LoadResources()
    {
        scene = Object.Instantiate(Resources.Load("River", typeof(GameObject)), new Vector3(-2.503f, -8.267982f, 2.844666f), Quaternion.identity) as GameObject;
        characters = new Character[6];
        leftside = new RiverSide(true);
        rightside = new RiverSide(false);
        Vector3 dst;
        for (int i = 0; i < 3; i++)
        {
            Character character = new Character(true, i);
            characters[i] = character;
            dst = leftside.JoinLand(character);
            character.obj.transform.position = dst;
        }
        for (int i = 3; i < 6; i++)
        {
            Character character = new Character(false, i);
            characters[i] = character;
            dst = leftside.JoinLand(character);
            character.obj.transform.position = dst;
        }
        boat = new Boat();
    }
    public void Restart()
    { 
		Vector3 dst;
        leftside.Reset();
        rightside.Reset();
        for (int i = 0; i < 6; i++)
        {
            characters[i].Reset();
            dst = leftside.JoinLand(characters[i]);
            characters[i].obj.transform.position = dst;
        }
        boat.Reset();
		STATE.P = 3;
		STATE.D = 3;
		STATE.B = true;
    }
    public void MoveBoat()
    {
        if (usergui.state != 0) return;
        if (boat.count != 0)
        {
			usergui.tips = false;
			int P = boat.getPriestCount();
			int D = boat.getDevilCount();
			bool B = boat.left;
			boat.MoveBoat();
			if (B)
			{
				STATE.P -= P;
				STATE.D -= D;
				STATE.B = !B;
			}
			else {
				STATE.P += P;
				STATE.D += D;
				STATE.B = !B;
			}
			usergui.state = CheckGameOver();
        }
    }
    public int CheckGameOver()
    {
        if (rightside.DevilCount + rightside.PriestCount == 6) return 1;
        if (boat.left)
        {
            if (leftside.DevilCount + boat.getDevilCount() > leftside.PriestCount + boat.getPriestCount() && leftside.PriestCount + boat.getPriestCount() != 0) return 2;
            if (rightside.DevilCount > rightside.PriestCount && rightside.PriestCount != 0) return 2;
        }
        else
        {
            if (rightside.DevilCount + boat.getDevilCount() > rightside.PriestCount + boat.getPriestCount() && rightside.PriestCount + boat.getPriestCount() != 0) return 2;
            if (leftside.DevilCount > leftside.PriestCount && leftside.PriestCount != 0) return 2;
        }
        return 0;
    }
    public void MoveModel(Character Model)
    {
        if (usergui.state != 0) return;
        if (boat.left != Model.left) return;
        Vector3 dst;
        if (Model.onGround)
        {
            dst = boat.JoinBoat(Model);
			if (dst != Vector3.zero)  //不一定船上有空位
            {
                if (Model.left) leftside.LeaveLand(Model.id);
                else rightside.LeaveLand(Model.id);
                Model.LeaveLand();
                Model.JoinBoat(boat);
                Model.move.MoveToPosition(dst);
			}
        }
        else
        {
            Model.LeaveBoat();
            boat.leaveBoat(Model.id);
            if (Model.left)
            {
                Model.JoinLand(leftside);
                dst = leftside.JoinLand(Model);
            }
            else
            {
                Model.JoinLand(rightside);
                dst = rightside.JoinLand(Model);
            }
            Model.move.MoveToPosition(dst);
            usergui.state = CheckGameOver();
        }
    }
    // Use this for initialization
    void Start()
    {

	}
    // Update is called once per frame
    void Update()
    {

    }

	public string tips() {
		StateMove statemove = GRAPH.getNextMove(STATE);
		string tipstring;
		string number = "";
		int moveP = statemove.P;
		int moveD = statemove.D;
		if (moveP != 0) {
			number += moveP.ToString();
			if (moveP > 1)
			{
				number += " Priests";
			}
			else {
				number += " Priest";
			}
		}
		if (moveD != 0) {
			if (moveP != 0) number += " And ";
			number += moveD.ToString();
			if (moveD > 1)
			{
				number += " Devils";
			}
			else
			{
				number += " Devil";
			}
		}

		if (boat.left)
		{
			tipstring = "Move " + number + " To Right";
		}
		else {
			tipstring = "Move " + number + " To Left";
		}
		return tipstring;
	}

}
