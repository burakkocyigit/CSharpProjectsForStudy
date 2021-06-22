using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigateMe.Core.Abstract
{

    public abstract class Node : Way//avm nin içindeki bütün noktalar bir yoldan geçtiği için bu şekilde implement ettim
    {
        public  int Floor { get; set; }
        // combobox içerisine objeleri atıp daha sonra seçimlerde kullanmak istedim fakat bunu yapınca itemlerin isminin düzgün görünmesi için object sınıfındaki tostring methodunu ezdim.
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
