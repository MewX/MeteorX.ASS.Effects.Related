using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp
{
    [Serializable]
    public class ASSPoint
    {
        public int X { get; set; }

        public int Y { get; set; }

        /// <summary>
        /// 0..255
        /// </summary>
        public int Brightness { get; set; }

        public string Color { get; set; }

        public double Start { get; set; }

        public double End { get; set; }

        public double EdgeDistance { get; set; }

        public ASSPoint()
        {
            Brightness = 255;
            Color = "FFFFFF";
        }
    }

    public class ASSPointF
    {
        public ASSPointF(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public ASSPointF()
        {
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double T { get; set; }

        public double Start { get; set; }
        public double End { get; set; }

        public double Theta { get; set; }

        public double GetDis(ASSPointF p2)
        {
            return Common.GetDistance(X, Y, p2.X, p2.Y);
        }

        public ASSPoint ToASSPoint()
        {
            return new ASSPoint { X = (int)(Math.Round(X)), Y = (int)(Math.Round(Y)) };
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }

        public double Intense { get; set; }
    }
}
