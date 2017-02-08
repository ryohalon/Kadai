using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityClassLibrary1
{
    [Serializable]
    public class ItemNum
    {
        private int num;
        public int Num {
            get { return num; }
            set { num = value; }
        }
    }
}
