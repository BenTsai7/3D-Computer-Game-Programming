using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Disk
{
    public class DiskFactory
    {
        private static DiskFactory _instance;
        List<DiskData> used = new List<DiskData>();
        List<DiskData> free = new List<DiskData>();
        public DiskData getDisk(Ruler ruler)
        {
            DiskData newDisk;
            if (free.Count > 0)
            {
                newDisk = free[0];
                newDisk.reset(ruler);
                free.RemoveAt(0);
            }
            else
            {
                newDisk = new DiskData(ruler);
            }
            used.Add(newDisk);
            newDisk.obj.SetActive(true);
            return newDisk;
        }
        public void freeDisk(DiskData disk)
        {
            free.Add(disk);
            if (!used.Remove(disk))
            {
                return;
            }
            disk.obj.SetActive(false);
        }
        // get instance anytime anywhare!
        public static DiskFactory getInstance()
        {
            if (_instance == null)
            {
                _instance = new DiskFactory();
            }
            return _instance;
        }
        public void reset()
        {
            foreach (DiskData temp in used)
            {
                temp.obj.SetActive(false);
                free.Add(temp);
            }
            used.Clear();
        }
        public DiskData getHitDisk(GameObject obj)
        {
            foreach (DiskData d in used)
            {
                if (d.obj == obj)
                {
                    return d;
                }
            }
            return null;
        }
    }

    public class ScoreController
    {
        public int score = 0;
        public void addScore(int score)
        {
            this.score += score;
        }
        public void reset()
        {
            score = 0;
        }
    }

}
