using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Remoting : MarshalByRefObject
    {
        private static int _counter;
        public int GetID()
        {
            return _counter++;
        }
    }
}
