using NavigateMe.Core.Abstract;
using NavigateMe.Places.Ways;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigateMe.Places
{
    public class Kiosk : Node //kiosklarda birer noddur onlarında sol üst köşeleri referans alınır
    {
        public int Number { get; set; }
        public Kiosk(int number)
        {
            Number = number;
            WhereIsNode(Number);
        }
        // kioskların property lerini assign eder
        public void WhereIsNode(int number)
        {
            Floor = 1;
            if (number == 1)
            {
                Floor = 1;
                Column = 'F';
                Row = 1;
            }
            else if (number == 2)
            {
                Floor = 1;
                Column = 'K';
                Row = 5;
            }
            else if (number == 3)
            {
                Floor = 1;
                Column = 'F';
                Row = 11;
            }
        }
        //bu method kioskun bulunduğu noktadan hedefe olan uzaklığını x ve y koordinatı olarak hesaplar her iki kattaki koordinatları tuple tipi ile verir
        public Tuple<Point, Point> FindShortestPath(Node target)
        {
            Point firstPath;
            Point secondPath;
            var floorPath = target.Floor - Floor;
            if (floorPath != 0)
            {
                var stairs = new Stairs();
                var stairsFirstPath = Math.Abs(stairs.Column - Column) + Math.Abs(stairs.Row - Row);//ilk kattaki yolun uzunluğu (merdivenli yol)
                var stairsSecondPath = Math.Abs(target.Column - stairs.Column) + Math.Abs(target.Row - stairs.Row);//ikinci kattaki yolun uzunluğu (merdivenli yol)
                var stairsTotalPath = stairsFirstPath + stairsSecondPath;//merdivenli yolun toplam uzuluğu

                var elevator = new Elevator();
                var elevatorFirstPath = Math.Abs(elevator.Column - Column) + Math.Abs(elevator.Row - Row);//ilk kattaki yolun uzunluğu (asansörlü yol)
                var elevatorSecondPath = Math.Abs(target.Column - elevator.Column) + Math.Abs(target.Row - elevator.Row);//ikinci kattaki yolun uzunluğu (asansörlü yol)
                var elevatorTotalPath = elevatorFirstPath + elevatorSecondPath;//asansörlü yolun toplam uzuluğu
                if (elevatorTotalPath <= stairsTotalPath)//yol eşitsede asansörü kullansın yorulmasın =)
                {
                    firstPath = new Point(elevator.Column - Column, elevator.Row - Row);
                    secondPath = new Point(target.Column - elevator.Column, target.Row - elevator.Row);
                }
                else
                {
                    firstPath = new Point(stairs.Column - Column, stairs.Row - Row);
                    secondPath = new Point(target.Column - stairs.Column, target.Row - stairs.Row);
                }
                return new Tuple<Point, Point>(firstPath, secondPath);
            }
            else
            {
                firstPath =new Point( target.Column - Column, target.Row - Row);
                secondPath = new Point();
                return new Tuple<Point, Point>(firstPath, secondPath);
            }
        }
        //comboboxta görünen adını düzenlemek için object sınıfındaki toString methodunu override ediyoruz
        public override string ToString()
        {
            return this.GetType().Name + this.Number;
        }
    }
}
