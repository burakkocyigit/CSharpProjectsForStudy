using NavigateMe.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigateMe.Places.Ways
{
    public class Elevator : Way//merdiven ve asansörün tek bir katta tek bir nodu yoktur yani sadece avm nin içerisinde yoldur
    {
        public Elevator()
        {
            Column = 'E';
            Row = 5;
        }
    }
}
