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
    public class Kiosk : Node
    {
        public int Number { get; set; }
        public Node Target { get; set; }
        public Kiosk(int number)
        {
            Number = number;
            WhereIsNode(Number);
        }
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
        public override string ToString()//comboboxta görünen adını düzenlemek için object sınıfındaki toString methodunu override ediyoruz
        {
            return this.GetType().Name + this.Number;
        }
    }
}
