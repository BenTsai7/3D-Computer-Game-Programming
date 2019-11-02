using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public class PatrolmanFactory
    {
        private static PatrolmanFactory _instance;
        List<Patrolman> used = new List<Patrolman>();
        List<Patrolman> free = new List<Patrolman>();
        public Patrolman getPatrolman(int area)
        {
            Patrolman newPatrolman;
            if (free.Count > 0)
            {
                newPatrolman = free[0];
                free.RemoveAt(0);
                newPatrolman.setArea(area);
            }
            else
            {
                newPatrolman = new Patrolman(area);
            }
            used.Add(newPatrolman);
            newPatrolman.obj.SetActive(true);
            return newPatrolman;
        }
        public void freePatrolman(Patrolman patrolman)
        {
            free.Add(patrolman);
            if (!used.Remove(patrolman))
            {
                return;
            }
            patrolman.obj.SetActive(false);
        }
        // get instance anytime anywhare!
        public static PatrolmanFactory getInstance()
        {
            if (_instance == null)
            {
                _instance = new PatrolmanFactory();
            }
            return _instance;
        }
        public void reset()
        {
            foreach (Patrolman temp in used)
            {
                temp.obj.SetActive(false);
                free.Add(temp);
            }
            used.Clear();
        }
    }

    public class ScoreController
    {
        private int score;
        public void addScore(int score)
        {
            this.score += score;
        }
        public int getScore()
        {
            return score;
        }
        public void reset()
        {
            score = 0;
        }
    }
}
