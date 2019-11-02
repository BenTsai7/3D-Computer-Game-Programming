using System;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public interface IUserAction
    {
        void Reset();
        void GameStart();
        void moveRole(DIRECTION direction);
    }
}