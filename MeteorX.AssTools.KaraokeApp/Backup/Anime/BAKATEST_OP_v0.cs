using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class BAKATEST_OP_v0 : BaseAnime3
    {
        public BAKATEST_OP_v0()
        {
            InFileName = @"G:\Workshop\baka\01\01.op.ass";
            OutFileName = @"G:\Workshop\baka\01\01.op_fixed.ass";

            this.FontWidth = 44;
            this.FontHeight = 44;
            this.FontSpace = 7;

            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.MarginBottom = 20;
            this.MarginLeft = 35;
            this.MarginRight = 35;
            this.MarginTop = 35;

            this.IsAvsMask = true;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            Random rnd = new Random();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = iEv <= 23;
                ASSEvent ev = ass_in.Events[iEv];
                if (!isJp)
                {
                    ev.Start = ass_in.Events[iEv - 24].Start;
                    ev.End = ass_in.Events[iEv - 24].End;
                }
                ass_out.Events.Add(ev);
            }
            /*
            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                int jEv = -1;
                if (iEv >= 535 && iEv <= 558) jEv = iEv - 535;
                if (iEv >= 559 && iEv <= 582) jEv = iEv - 559;
                if (jEv < 0) continue;
                ASSEvent ev = ass_in.Events[iEv];
                ev.Text = ev.Text.Replace(@"{\fad(150,150)}", "");
                string stmp = "";
                bool inb = false;
                foreach (char ch in ev.Text)
                {
                    if (ch == '{') inb = true;
                    if (!inb) stmp += ch;
                    if (ch == '}') inb = false;
                }
                ev.Text = stmp;
                Console.WriteLine(ev.Text);

                string c0 = "DA9500";
                string c1 = "5DFA49";

                if (jEv == 6)
                {
                    c0 = "B967FF";
                    c1 = "F1E3FE";
                }
                if (jEv == 5)
                {
                    c0 = "FFDF5D";
                    c1 = "FFF5CA";
                }
                if (jEv == 7)
                {
                    c0 = "60FF5C";
                    c1 = "CEFFCF";
                }
                if (jEv == 8)
                {
                    c0 = "5BB0FF";
                    c1 = "DAEEFD";
                }
                if (jEv == 9)
                {
                    c0 = "7E76FF";
                    c1 = "D6D2FF";
                }
                if (jEv == 10)
                {
                    c0 = "FF59C0";
                    c1 = "FFD8F1";
                }
                if (jEv == 11)
                {
                    c0 = "0CC6FD";
                    c1 = "B3EDFD";
                }
                if (jEv == 12)
                {
                    c0 = "E76298";
                    c1 = "E1D3DA";
                }
                if (jEv >= 5 && jEv <= 12) c1 = c0;

                StringBuilder sb = new StringBuilder();
                sb.Append(fad(0.25, 0.25) + a(1, "00") + c(1, "FFFFFF") + a(4, "FF") + a(3, "33") + bord(2.5) + blur(3.2));
                for (int iCh = 0; iCh < ev.Text.Length; iCh++)
                {
                    char ch = ev.Text[iCh];
                    string cc = Common.scaleColor(c0, c1, (double)iCh / (double)(ev.Text.Length - 1));
                    sb.Append(c(3, cc) + ch);
                }

                //ass_out.AppendEvent(0, ev.Style, ev.Start, ev.End, ev.Text);
                ass_out.AppendEvent(0, ev.Style, ev.Start, ev.End, sb.ToString());
            }
             * */

            ass_out.SaveFile(OutFileName);
        }
    }
}
