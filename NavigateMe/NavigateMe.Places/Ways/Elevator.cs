using NavigateMe.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigateMe.Places.Ways
{
    public class Elevator : Way
    {
        public Elevator()
        {
            Column = 'E';
            Row = 5;
        }
    }
}
