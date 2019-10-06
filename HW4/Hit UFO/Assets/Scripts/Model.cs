using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Disk
{
    public class Ruler
    {
        public Vector3 scale;
        public float speed;
        public int level;
        public int score;
        public Color color;
        public Ruler(int level)
        {
            switch (level)
            {
                case 1:
                    scale = new Vector3(2f, 0.2f, 2f);
                    speed = 7;
                    color = Color.black;
                    score = 1;
                    break;
                case 2:
                    scale = new Vector3(1.5f, 0.15f, 1.5f);
                    speed = 8;
                    color = Color.white;
                    score = 2;
                    break;
                case 3:
                    scale = new Vector3(1f, 0.1f, 1f);
                    speed = 9;
                    color = Color.red;
                    score = 3;
                    break;
                case 4:
                    scale = new Vector3(0.9f, 0.09f, 0.9f);
                    speed = 10;
                    color = Color.yellow;
                    score = 4;
                    break;
                case 5:
                    scale = new Vector3(0.7f, 0.07f, 0.7f);
                    speed = 11;
                    color = Color.green;
                    score = 5;
                    break;
            }
            this.level = level;
        }
    }
    public class DiskData
    {
        public GameObject obj;
        public Ruler ruler;
        public DiskData(Ruler ruler)
        {
            obj = Object.Instantiate(Resources.Load("Disk", typeof(GameObject)), new Vector3(100f, 100f, 100f), Quaternion.identity) as GameObject;
            reset(ruler);
        }
        public void reset(Ruler ruler)
        {
            obj.GetComponent<Renderer>().material.color = ruler.color;
            obj.transform.localScale = ruler.scale;
            this.ruler = ruler;
            obj.SetActive(true);
        }
        public void fly()
        {
            var actionManager = ((FirstController)SSDirector.GetInstance().CurrentSceneController).actionManager;
            actionManager.playDisk(this);
        }
    }
}