using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class KIDDY_GiRL_AND_OP : BaseAnime2
    {
        public KIDDY_GiRL_AND_OP()
        {
            this.FontHeight = 40;
            this.FontSpace = 0;
            this.IsAvsMask = true;
            this.MarginLeft = 35;
            this.MarginBottom = 35;
            this.MarginTop = 35;
            this.MarginRight = 35;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"G:\Workshop\kiddy girl and\op1\op_k.ass";
            this.OutFileName = @"G:\Workshop\kiddy girl and\op1\op.ass";
        }

        public override Size GetSize(string s)
        {
            if (s.Length == 0) return new Size(0, 0);
            if (s.Trim().Length == 0)
            {
                if (s == " ") return new Size { Height = this.FontHeight, Width = this.FontHeight / 2 };
                else return new Size { Height = this.FontHeight, Width = this.FontHeight };
            }
            if (Common.IsLetter(s[0])) // ENG
            {
                Size sz = base.GetSize(s);
                sz.Width = (int)(sz.Width * 1.2);
                return sz;
            }
            return new Size { Height = this.FontHeight, Width = this.FontHeight * s.Length + this.FontSpace * (s.Length - 1) };
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                //if (iEv > 0) continue;

                bool isJp = true;

                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);
                int x0 = MarginLeft;
                int totalWidth = 0;
                if (isJp)
                {
                    foreach (KElement ke in kelems)
                    {
                        this.MaskStyle = "Style: Default,宋体,40,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,7,0,0,0,0";
                        string outlineFontname = "DFSoGei-W5";
                        int outlineEncoding = isJp ? 128 : 136;
                        int yoffset = 279;
                        int ox_offset = 0;
                        string outlineString = GetOutline(10, 10, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, yoffset);
                        //Size sz = GetMask(p(4) + outlineString, 0, 0).GetSize();
                        //if (ke.KText.Trim().Length == 0) sz.Width = this.FontHeight;
                        Size sz = GetSize(ke.KText);
                        totalWidth += sz.Width + FontSpace;
                    }
                    x0 = (PlayResX - MarginLeft - MarginRight - totalWidth) / 2 + MarginLeft;
                    if (iEv == 11) x0 = MarginLeft;
                    if (iEv == 12) x0 = PlayResX - MarginRight - totalWidth;
                }
                else
                {
                    totalWidth = GetTotalWidth(ev);
                }
                int bakx0 = x0;
                int y0 = isJp ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int kSum = 0;
                string outlines = "";
                double lastx0 = 0;
                double lastt0 = 0;
                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    this.MaskStyle = "Style: Default,宋体,40,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,7,0,0,0,0";
                    string outlineFontname = "DFSoGei-W5";
                    int outlineEncoding = isJp ? 128 : 136;
                    int yoffset = 279;
                    int ox_offset = 0;
                    string outlineString = "";
                    Size sz = new Size();
                    int xoffset = 0;
                    if (ke.KText == "i") xoffset = -3;
                    if (ke.KText == "e" && kelems[iK + 1].KText.Trim() == "") xoffset = 3;
                    ox_offset = xoffset;
                    if (ke.KText.Trim().Length > 0)
                    {
                        outlineString = GetOutline(10, 10, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, yoffset);
                    }
                    sz = GetSize(ke.KText);
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    if (ke.IsSplit)
                    {
                        kStart = ke.KStart_NoSplit;
                        kEnd = ke.KEnd_NoSplit;
                    }
                    kSum += ke.KValue;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    int x_an7 = x0;
                    int y_an7 = y0;
                    lastx0 = x0;
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;
                    outlineString = GetOutline(ox_offset + x - sz.Width / 2, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, yoffset);
                    outlines += outlineString;

                    string evStyle = "op_jp";

                    ass_out.AppendEvent(60, "pt", kStart, kEnd + 0.2,
                        pos(0, 0) + a(1, "00") + fad(0.05, kEnd + 0.2 - kStart - 0.05 - 0.08) + a(3, "22") + bord(5) + blur(5) +
                        p(4) + outlineString);
                    ass_out.AppendEvent(61, "pt", kStart, kEnd + 0.2,
                        pos(0, 0) + a(1, "00") + fad(0.05, kEnd + 0.2 - kStart - 0.05 - 0.08) + a(3, "22") + bord(3) + blur(3) +
                        p(4) + outlineString);

                    // shadow
                    ass_out.AppendEvent(48, "pt", ev.Start - 0.2, ev.End + 0.2,
                        fad(0.3, 0.3) + pos(3.5, 3.5) + a(1, "00") + c(1, "222222") + blur(1.2) +
                        p(4) + outlineString);
                    // border
                    ass_out.AppendEvent(49, "pt", ev.Start - 0.2, ev.End + 0.2,
                        fad(0.3, 0.3) + pos(0, 0) + a(3, "22") + c(3, "FFFFFF") + bord(2.5) + blur(1) +
                        p(4) + outlineString);

                    double pC = (kEnd - kStart) * 50 * 4 * sz.Width / this.FontHeight;
                    if (iEv > 8 || (iEv == 8 && Common.IsLetter(ke.KText[0]))) pC *= 2.1;
                    for (int iP = 0; iP < pC; iP++)
                    {
                        double ptx0 = Common.RandomDouble(rnd, x - 20, x + 20);
                        double ptx1 = Common.RandomDouble(rnd, x - 50, x + 50);
                        double pty = y;
                        double ptt0 = Common.RandomDouble(rnd, kStart, kEnd);

                        int startag = Common.RandomInt(rnd, 0, 90);

                        string hStr = "m 0 0 l 0 -70 1 0";
                        ass_out.AppendEvent(40, "pt", ptt0, ptt0 + 1.3,
                            move(ptx0, pty, ptx1, pty) + a(1, "00") + c(1, "FFBD00") +
                            blur(0.8) + frx(90) + t(0, 0.25, frx(startag).t()) + t(0.25, 1.3, frx(90).t()) +
                            p(1) + hStr);
                        hStr = "m 0 0 l 0 70 1 0";
                        ass_out.AppendEvent(40, "pt", ptt0, ptt0 + 1.3,
                            move(ptx0, pty, ptx1, pty) + a(1, "00") + c(1, "FFBD00") +
                            blur(0.8) + frx(90) + t(0, 0.25, frx(startag).t()) + t(0.25, 1.3, frx(90).t()) +
                            p(1) + hStr);

                        hStr = "m 0 0 l 0 -50 1 0";
                        ass_out.AppendEvent(41, "pt", ptt0, ptt0 + 1.3,
                            move(ptx0, pty, ptx1, pty) + a(1, "77") + c(1, "FFFFFF") +
                            blur(0.8) + frx(90) + t(0, 0.25, frx(startag).t()) + t(0.25, 1.3, frx(90).t()) +
                            p(1) + hStr);
                        hStr = "m 0 0 l 0 50 1 0";
                        ass_out.AppendEvent(41, "pt", ptt0, ptt0 + 1.3,
                            move(ptx0, pty, ptx1, pty) + a(1, "77") + c(1, "FFFFFF") +
                            blur(0.8) + frx(90) + t(0, 0.25, frx(startag).t()) + t(0.25, 1.3, frx(90).t()) +
                            p(1) + hStr);
                    }
                }

                ass_out.AppendEvent(51, "pt", ev.Start - 0.2, ev.End + 0.2,
                    pos(0, y0) + clip(4, outlines) + a(1, "00") + fad(0.3, 0.3) + c(1, "B7AA32") + blur(18) +
                    p(1) + "m 0 -20 l 1280 -20 1280 20 0 20");
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
