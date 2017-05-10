using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp
{
    public class KElement
    {
        public string KText { get; set; }
        public int KValue { get; set; }

        public bool IsSplit { get; set; }

        public double KStart_NoSplit { get; set; }
        public double KEnd_NoSplit { get; set; }

        public KElement()
        {
            IsSplit = false;
            KStart_NoSplit = -1;
            KEnd_NoSplit = -1;
        }
    }
}
