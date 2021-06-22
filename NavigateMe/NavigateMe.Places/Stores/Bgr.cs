using NavigateMe.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigateMe.Places.Stores
{
    public class Bgr : Node// bgr mağazası ve diğer mağazaların hepsinin girişi avmnin içerisinde birer node dur aslında
    {
        public Bgr()
        {
            Floor = 2;
            Column = 'C';
            Row = 6;
        }
    }
}
