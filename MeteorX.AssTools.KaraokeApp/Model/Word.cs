using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteorX.AssTools.KaraokeApp.Anime;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    public class Word
    {
        public KElement BaseKElement { get; set; }

        public double KStart { get; set; }

        public double KEnd { get; set; }

        public string Text { get; set; }

        public double X5 { get; set; }

        public double Y5 { get; set; }

        public double Start { get; set; }

        public double End { get; set; }

        public StringMask Mask { get; set; }

        public string OutlineString { get; set; }

        public double Width { get { return Mask.Width; } }
    }
}
