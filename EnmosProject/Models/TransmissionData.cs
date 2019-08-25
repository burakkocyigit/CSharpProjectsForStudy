using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class TransmissionData
    {
        #region Properties
        public string Text { get; set; }
        public int Number { get; set; }
        public float FractionalNumber { get; set; }
        public int ID { get; set; }
        #endregion
        #region Methods
        public TransmissionData(bool random)
        {
            if (random)
            {
                Text = RandomString(5, false);
                Number = RandomNumber(0, 50);
                FractionalNumber = RandomFloatNumber(0, 50);
            }
        }
        public object[] TakeParameterForServer(TransmissionData transmissionData)
        {
            return new object[] { new { transmissionData.Text }, new { transmissionData.Number }, new { transmissionData.FractionalNumber }, new { DateTime.Now }, new { transmissionData.ID } };
        }
        public object[] TakeParameterForClient(TransmissionData transmissionData)
        {
            return new object[] { new { transmissionData.Text }, new { transmissionData.Number }, new { transmissionData.FractionalNumber }, new { DateTime.Now } };
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private float RandomFloatNumber(float min, float max)
        {
            Random random = new Random();
            return (float)random.NextDouble() * (max - min) + min;
        } 
        #endregion
    }
}
