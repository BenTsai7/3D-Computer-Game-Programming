using System;

namespace PAD
{
    public interface IUserAction
    {
        void Restart();
        void MoveBoat();
        int CheckGameOver();
        void MoveModel(Character Model);
		string tips();
	}
}