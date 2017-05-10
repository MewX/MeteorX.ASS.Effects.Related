using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class ClannadAS_ED
    {
        public string InFileName = @"G:\Workshop\clannad\ed_org.ass";
        public string OutFileName = @"G:\Workshop\clannad\ed.ass";
        public double Shift = 0;

        public void Run()
        {
            ASS ass = ASS.FromFile(InFileName);
            ass.Shift(Shift);
            ass.SaveFile(OutFileName);
        }
    }
}
