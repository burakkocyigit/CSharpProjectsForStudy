using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigateMe.Core.Abstract
{
    public abstract class Way
    {
        public  char Column { get; set; }//en kısa yolu bulmak için ascii table dan yararlanıcam bu yüzden sütun property sini char tipinde aldım
        public  int Row { get; set; }
    }
}
