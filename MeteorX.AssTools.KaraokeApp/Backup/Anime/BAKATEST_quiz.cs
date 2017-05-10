using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class BAKATEST_quiz : BaseAnime3
    {
        public override void Run()
        {
            int dx = 0;
            int dy = 23;

            string sampleASSEvent = @"Dialogue: 0,0:23:44.77,0:23:59.75,quiz_an7,NTP,0000,0000,0000,,{\pos(594,68)}\N富\N含\N养\N分\N的\N变\N厚\N的\N叶\N，\N像\N葱\N和\N薤\N这\N些\N植\N物\N都\N长\N有\N鳞\N茎\N。";

            ASSEvent srcEv = ASSEvent.FromString(sampleASSEvent);
            string srcS = @"教\N师\N评\N语";
            srcS = srcS.Replace(@"\N", "");

            int x0 = 455;
            int y0 = 260;

            int x = x0;
            int y = y0;
            string outS = "";
            foreach (char ch in srcS)
            {
                ASSEvent ev = ASSEvent.FromString(sampleASSEvent);
                ev.Text = pos(x, y) + ch;
                x += dx;
                y += dy;
                outS += ev.ToString() + "\r\n";
            }

            StreamWriter fout = new StreamWriter(new FileStream(@"G:\Workshop\baka\01\1.txt", FileMode.Append), Encoding.Default);
            fout.WriteLine(outS);
            fout.Close();
        }
    }
}
