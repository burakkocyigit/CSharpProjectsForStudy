using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// bir class için "... is a ..." diyebiliyorsak abstract kullanabiliriz bu yüzden avm nin içerisindeki her hangi bi yeri bir yol olarak kabul ederek bi abstract oluşturdum
namespace NavigateMe.Core.Abstract
{
    public abstract class Way
    {
        public  char Column { get; set; }//en kısa yolu bulmak için ascii table dan yararlanıcam bu yüzden sütun property sini char tipinde aldım
        public  int Row { get; set; }
    }
}
