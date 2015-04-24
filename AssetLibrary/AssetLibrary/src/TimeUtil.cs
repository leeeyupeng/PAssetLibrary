using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetLibrary
{
    class TimeUtil
    {
        public static uint Now { get { return (uint)DateTime.Now.Second; } }
    }
}
