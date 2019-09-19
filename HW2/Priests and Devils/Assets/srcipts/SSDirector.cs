using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAD
{
    public class SSDirector : System.Object
    {
        // singlton instance
        private static SSDirector _instance;

        public ISceneController CurrentSceneController { get; set; }

        // get instance anytime anywhare!
        public static SSDirector GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SSDirector();
            }
            return _instance;
        }
    }
}
