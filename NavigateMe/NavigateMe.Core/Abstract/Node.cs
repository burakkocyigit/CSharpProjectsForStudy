using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigateMe.Core.Abstract
{
    public abstract class Node : Way
    {
        public  int Floor { get; set; }
        public override string ToString()// combobox içerisine objeleri atıp daha sonra seçimlerde kullanmak istedim fakat bunu yapınca itemlerin isminin düzgün görünmesi için object sınıfındaki tostring methodunu ezdim.
        {
            return this.GetType().Name;
        }
    }
}
