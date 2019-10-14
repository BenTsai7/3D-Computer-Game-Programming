using System;
using System.Collections.Generic;
using UnityEngine;

namespace Disk
{
    public interface IUserAction
    {
        void Reset();
        int getState();
        void GameStart();
        void hitDisk(GameObject disk);
    }
}