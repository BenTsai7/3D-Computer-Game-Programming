using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAD
{
    public class Character
    {
        public GameObject obj;
        public bool isPriest;
        public bool onGround;
        public bool left;
        public int id;
        public Click click;
        public Move move;
        public Character(bool isPriest, int c_id)
        {
            if (isPriest)
            {
                obj = Object.Instantiate(Resources.Load("Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                this.isPriest = true;
            }
            else
            {
                obj = Object.Instantiate(Resources.Load("Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                this.isPriest = false;
            }
            left = true;
            onGround = true;
            id = c_id;
            click = obj.AddComponent(typeof(Click)) as Click;
            click.SetCharacter(this);
            move = obj.AddComponent(typeof(Move)) as Move;
        }
        public void JoinLand(RiverSide r)
        {
            onGround = true;
            left = r.isLeft;
        }
        public void LeaveLand()
        {
            onGround = false;
        }
        public void JoinBoat(Boat b)
        {
            obj.transform.parent = b.obj.transform;
            onGround = false;
        }
        public void LeaveBoat()
        {
            obj.transform.parent = null;
            onGround = true;
        }
        public void ChangeSide()
        {
            left = !left;
        }
        public void Reset()
        {
            obj.transform.parent = null;
            onGround = true;
            left = true;
        }
    }

    public class RiverSide
    {
        public int PriestCount, DevilCount;
        public Vector3[] positions;
        public bool[] slots;
        public bool isLeft;
        Character[] characters;
        public RiverSide(bool isLeft)
        {
            //不需要额外空间以储存Gameobj，Gameobj已经在Controller中实例化
            float offset = 5.6f;
            if (isLeft)
            {
                positions = new Vector3[] {new Vector3(-1.4f,1,5.37f), new Vector3(-2.08f,1,4.92f), new Vector3(-2.07f,1,4.22f),
            new Vector3(-1.44f,1,2.72f), new Vector3(-2.1f,1,3.66f), new Vector3(-2.08f,1,3.05f)};
                this.isLeft = true;
            }
            else
            {
                positions = new Vector3[] {new Vector3(-1.4f+offset,1,5.37f), new Vector3(-2.08f+offset+0.5f,1,4.92f), new Vector3(-2.07f+offset+0.5f,1,4.22f),
            new Vector3(-1.44f+offset,1,2.72f), new Vector3(-2.1f+offset+0.5f,1,3.66f), new Vector3(-2.08f+offset+0.5f,1,3.05f)};
                this.isLeft = false;
            }
            PriestCount = 0;
            DevilCount = 0;
            slots = new bool[6];
            characters = new Character[6];
            for (int i = 0; i < 6; ++i) { slots[i] = false; characters[i] = null; }
        }
        public void LeaveLand(int id)      //离开陆地
        {
            for (int i = 0; i < 6; ++i)
            {
                if (slots[i] == true)
                {
                    if (characters[i] != null && (characters[i].id == id))
                    {
                        if (characters[i].isPriest) PriestCount--;
                        else DevilCount--;
                        characters[i] = null;
                        slots[i] = false;
                    }
                }
            }
        }
        public Vector3 JoinLand(Character c)
        {
            for (int i = 0; i < 6; ++i)
            {
                if (slots[i] == false)
                {
                    characters[i] = c;
                    if (c.isPriest) PriestCount++;
                    else DevilCount++;
                    slots[i] = true;
                    return positions[i];
                }
            }
            return Vector3.zero;
        }
        public void Reset()
        {
            PriestCount = 0;
            DevilCount = 0;
            for (int i = 0; i < 6; ++i) { slots[i] = false; characters[i] = null; }
        }

    }
    public class Boat
    {
        public GameObject obj;
        public Vector3[] positions_left, positions_right;
        public float offset = 3.0f;
        public Character[] characters;
        public bool left;
        Click click;
        public Move move;
        public int count;
        public Boat()
        {
            obj = Object.Instantiate(Resources.Load("Boat", typeof(GameObject)), new Vector3(-0.21f, 0.57f, 3.65f), Quaternion.identity) as GameObject;
            characters = new Character[2];
            positions_left = new Vector3[] { new Vector3(-0.18f, 1f, 3.99f), new Vector3(-0.18f, 1f, 3.37f) };
            positions_right = new Vector3[] { new Vector3(-0.18f + offset, 1f, 3.99f), new Vector3(-0.18f + offset, 1f, 3.37f) };
            left = true;
            click = obj.AddComponent(typeof(Click)) as Click;
            click.SetBoat(this);
            count = 0;
            move = obj.AddComponent(typeof(Move)) as Move;
        }
        public int getDevilCount()
        {
            int count = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] != null && (!characters[i].isPriest))
                {
                    ++count;
                }
            }
            return count;
        }
        public int getPriestCount()
        {
            int count = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] != null && characters[i].isPriest)
                {
                    ++count;
                }
            }
            return count;
        }
        public Vector3 JoinBoat(Character c)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] == null)
                {
                    characters[i] = c;
                    ++count;
                    if (left) return positions_left[i];
                    else return positions_right[i];
                }
            }
            return Vector3.zero;

        }
        public void leaveBoat(int id)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if ((characters[i] != null) && (characters[i].id == id))
                {
                    --count;
                    characters[i] = null;
                    return;
                }
            }
        }
        public void MoveBoat()
        {
            if (!left)
            {
                move.MoveToPosition(new Vector3(-0.21f, 0.57f, 3.65f));
            }
            else
            {
                move.MoveToPosition(new Vector3(2.79f, 0.57f, 3.65f));
            }
            for (int i = 0; i < characters.Length; i++)
            {
                if ((characters[i] != null))
                {
                    characters[i].ChangeSide();
                }
            }
            left = !left;
        }
        public void Reset()
        {
            left = true;
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i] = null;
            }
            obj.transform.position = new Vector3(-0.21f, 0.57f, 3.65f);
            count = 0;
        }
    }

    public class Move : MonoBehaviour
    {
        Vector3 dst;
        float speed = 15; //移动速度
        bool move = false;
        static bool event_ignore;
        void Update()
        {
            if (move)
            {
                transform.position = Vector3.MoveTowards(transform.position, dst, speed * Time.deltaTime);
                if (transform.position == dst) move = false;
            }
        }
        public void MoveToPosition(Vector3 dst)
        {
            this.dst = dst;
            move = true;
        }
    }

    public class Click : MonoBehaviour
    {
        IUserAction action;
        Boat boat;
        Character character;
        public void SetCharacter(Character character)
        {
            this.character = character;
            this.boat = null;
        }
        public void SetBoat(Boat boat)
        {
            this.boat = boat;
            this.character = null;
        }
        void Start()
        {
            action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
        }
        public void OnMouseDown()
        {
            if (boat == null && character == null) return;
            if (boat != null)
            {
                action.MoveBoat();
            }
            else if (character != null)
                action.MoveModel(character);
		}
    }

}