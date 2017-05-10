using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Asura2_OP : BaseAnime2
    {
        public Asura2_OP()
        {
            this.FontHeight = 38;
            this.FontSpace = 0;
            this.IsAvsMask = true;
            this.MarginLeft = 30;
            this.MarginBottom = 30;
            this.MarginTop = 30;
            this.MarginRight = 30;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"g:\workshop\asura2\op\op_k.ass";
            this.OutFileName = @"g:\workshop\asura2\op\op.ass";
        }

        public override Size GetSize(string s)
        {
            if (s.Length >= 3) return base.GetSize(s);
            return new Size { Width = s.Length * FontHeight + (s.Length - 1) * FontSpace, Height = FontHeight };
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                ASSEvent ev = ass_in.Events[iEv];
                bool isJp = iEv <= 18;
                this.MaskStyle = "Style: Default,DFKoIn-W4,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128";

                //if (iEv != 7) continue;
                //if (isJp) continue;
                //if (!((iEv <= 5 || (iEv >= 19 && iEv <= 21)))) continue;

                if (!isJp)
                {
                    double t0 = ev.Start;
                    double t1 = ev.End;
                    if (iEv <= 21)
                    {
                        t0 -= 0.6;
                        t1 += 0.5;
                    }
                    ass_out.AppendEvent(30, "op_cn", t0, t1,
                        pos(PlayResX / 2, MarginTop + FontHeight / 2) + an(5) + fad(0.3, 0.3) +
                        a(1, "22") + blur(1) +
                        ev.Text);
                    ass_out.AppendEvent(0, "op_cn", t0, t1,
                        pos(PlayResX / 2 + 3, MarginTop + FontHeight / 2 + 3) + an(5) + fad(0.3, 0.3) +
                        a(1, "22") + blur(1.3) + c(1, "000000") +
                        ev.Text);
                    for (int i = 0; i < 10; i++)
                    {
                        ass_out.AppendEvent(20, "op_cn", t0, t1,
                             fad(0.3, 0.3) + pos(PlayResX / 2 + Common.RandomDouble(rnd, -1, 1), MarginTop + FontHeight / 2 + Common.RandomDouble(rnd, -1, 1)) + an(5) +
                             a(1, "AA") + blur(2) + (iEv <= 21 ? "" : c(1,Common.scaleColor("FF0A00", "FFFFFF", 0.83))) +
                            ev.Text);
                    }
                    continue;
                }

                List<KElement> kelems = ev.SplitK(!isJp);
                int tw = GetTotalWidth(ev);
                int x0 = (PlayResX - MarginLeft - MarginRight - tw) / 2 + MarginLeft;
                if (iEv == 5 || iEv == 7) x0 = MarginLeft;
                if (iEv == 6 || iEv == 8) x0 = (PlayResX - MarginRight - tw);
                int y0 = isJp ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int x0_bak = x0;

                int kSum = 0;

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    Size sz = GetSize(ke.KText);
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    int x_an7 = x0;
                    int y_an7 = y0;
                    StringMask mask = GetMask(ke.KText, x, y);
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;

                    string evStyle = ev.Style;

                    //ass_out.AppendEvent(50, evStyle, ev.Start, ev.End, pos(x, y) + a(1, "00") + ke.KText);

                    if (iEv >= 5)// && iEv <= 8)
                    {
                        double t0 = ev.Start - 0.5 + iK * 0.08;
                        double t1 = t0 + 0.35;
                        double t2 = kStart;
                        double t3 = kEnd;
                        double t4 = ev.End - 0.5 + iK * 0.08;
                        double t5 = t4 + 0.35;

                        double xCenter = tw / 2 + x0_bak;
                        double dx = x - xCenter;

                        string mainColor = "FFFFFF";
                        //mainColor = "000000";
                        string mainColor2 = "FFFFFF";
                        mainColor = mainColor2 = Common.scaleColor("FF0A00", "E8FF00", (double)iK / (double)kelems.Count);
                        mainColor2 = mainColor = Common.scaleColor(mainColor, "FFFFFF", 0.6);
                        mainColor2 = "FFFFFF";
                        //swap(
                        Func<int, string> fMainColor = ti => Common.scaleColor(mainColor2, mainColor, (double)ti / 5);
                        string shadColor = "000000";

                        double tmpSc = 0.99;

                        if (iEv > 8)
                        {
                            double rt0 = 7;
                            double rt1 = 0;
                            double rt2 = -7;
                            if (iEv == 11 || iEv == 14 || iEv == 17 || iEv == 18) rt2 = -4.5;
                            for (int i = 1; i <= 30; i++)
                            {
                                string alpha = Common.scaleAlpha("AA", "FF", (double)i / 30);
                                string alpha2 = Common.scaleAlpha(alpha, "FF", 0.2);
                                ass_out.AppendEvent(60, evStyle, t0, t2,
                                    fad(t1 - t0, 0) + move(x + i * rt0, y, x + i * rt1, y) +
                                    a(1, alpha) + c(1, "FFFFFF") + blur((double)(i + 1) * 0.5) +
                                    fsc(100 + i * 2) + t(a(1, alpha2).t()) +
                                    ke.KText);
                                ass_out.AppendEvent(60, evStyle, t2, t5,
                                    fad(0, t5 - t4) + move(x + i * rt1, y, x + i * rt2, y) +
                                    a(1, alpha2) + c(1, "FFFFFF") + blur((double)(i + 1) * 0.5) +
                                    fsc(100 + i * 2) + t(a(1, alpha).t()) +
                                    ke.KText);
                            }

                            int[,] bombs =
                                {
                                    {Common.RandomInt(rnd, x - 40,x  + 40), Common.RandomInt(rnd, y - 40, y - 20)},
                                    {Common.RandomInt(rnd, x - 40,x  + 40), Common.RandomInt(rnd, y + 20, y + 40)}
                                };

                            for (int iBomb = 0; iBomb < bombs.GetLength(0); iBomb++)
                            {
                                for (int i = 0; i < 200; i++)
                                {
                                    string sFrz = frz(Common.RandomInt(rnd, 0, 359));
                                    string sMoveT = t(0, 1, 0.5, fscx(Common.RandomInt(rnd, 5, 120) * 100).t());
                                    ass_out.AppendEvent(80, "pt", t2, t2 + 1,
                                        org(bombs[iBomb, 0], bombs[iBomb, 1]) + pos(bombs[iBomb, 0], bombs[iBomb, 1]) + sMoveT + sFrz +
                                        p(1) + "m 0 0 l 1 0 1 1 0 1" + r() + sFrz + fad(0, 0.4) +
                                        t(0, 0.3, fscx(700).t()) + t(0.3, 1, fscx(100).t()) +
                                        blur(1) + a(1, "00") + c(1, "FFFFFF") +
                                        p(1) + "m 0 0 l 1 0 1 1 0 1");
                                    ass_out.AppendEvent(20, "pt", t2, t2 + 1,
                                        org(bombs[iBomb, 0], bombs[iBomb, 1]) + pos(bombs[iBomb, 0], bombs[iBomb, 1]) + sMoveT + sFrz +
                                        p(1) + "m 0 0 l 1 0 1 1 0 1" + r() + sFrz + fad(0, 0.4) +
                                        t(0, 0.3, fscx(1000).t()) + t(0.3, 1, fscx(150).t()) +
                                        fscy(150) +
                                        blur(2) + a(1, "00") + c(1, mainColor) +
                                        p(1) + "m 0 0 l 1 0 1 1 0 1");
                                }
                            }

                        }

                        for (int i = 0; i < 40; i++)
                        {
                            string tString = "";
                            if (i < 5) tString = t(a(1, "22").t() + blur((double)(i + 1) * 0.5).t());
                            else tString = t(a(1, "FF").t());
                            ass_out.AppendEvent(50, evStyle, t0, t1,
                                move(xCenter + dx * (1.0 + (double)(i + 20) * 0.010), y, x, y) + fad(t1 - t0, 0) +
                                a(1, "AA") + c(1, fMainColor(i)) + blur(2 + i * 0.05) + tString +
                                fsc(120 + i * 2) + t(fsc(100).t()) +
                                ke.KText);
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            string aString = a(1, "22");
                            if (i >= 5) aString = a(1, Common.scaleAlpha("AA", "FF", tmpSc));
                            string blurString = blur((double)(i + 1) * 0.5);
                            if (i >= 5) blurString = blur(2 + i * 0.05);
                            ass_out.AppendEvent(50, evStyle, t1, t4,
                                pos(Common.scaleDouble(xCenter + dx * (1.0 + (double)(i + 20) * 0.010), x, tmpSc), y) +
                                aString + c(1, fMainColor(i)) + blurString +
                                ke.KText);
                        }
                        ass_out.AppendEvent(51, evStyle, t2 - 0.06, t2 + 0.5 - 0.06,
                            pos(Common.scaleDouble(xCenter + dx * (1.0 + (double)(0 + 20) * 0.010), x, tmpSc), y) +
                            fad(0.05, 0.3) +
                            a(1, "00") + c(1, "FFFFFF") + a(3, "00") + c(3, "FFFFFF") + bord(5) + blur(6) +
                            ke.KText);
                        ass_out.AppendEvent(52, evStyle, t2 - 0.06, t2 + 0.5 - 0.06,
                            pos(Common.scaleDouble(xCenter + dx * (1.0 + (double)(0 + 20) * 0.010), x, tmpSc), y) +
                            fad(0.05, 0.3) +
                            a(1, "00") + c(1, "FFFFFF") + blur(2) +
                            ke.KText);

                        ass_out.AppendEvent(49, evStyle, t0, t5,
                            fad(1, 1) + pos(Common.scaleDouble(xCenter + dx * (1.0 + (double)(0 + 20) * 0.010), x, tmpSc) + 4, y + 4) + a(1, "00") + c(1, shadColor) + blur(1.2) +
                            ke.KText);
                        ass_out.AppendEvent(49, evStyle, t0, t5,
                            fad(1, 1) + pos(Common.scaleDouble(xCenter + dx * (1.0 + (double)(0 + 20) * 0.010), x, tmpSc) + 4, y + 4) + a(1, "00") + c(1, shadColor) + blur(0.7) +
                            ke.KText);
                        for (int i = 0; i < 40; i++)
                        {
                            string startString = "";
                            if (i < 5) startString = a(1, "22") + blur((double)(i + 1) * 0.5);
                            else startString = a(1, Common.scaleAlpha("AA", "FF", tmpSc)) + blur(2 + i * 0.05);
                            ass_out.AppendEvent(50, evStyle, t4, t5,
                                move(x, y, xCenter + dx * (1.0 + (double)(i + 20) * 0.010), y) + fad(0, t5 - t4) +
                                c(1, fMainColor(i)) + startString +
                                t(blur(2 + i * 0.05).t() + a(1, "AA").t() + fsc(120 + i * 2).t()) +
                                ke.KText);
                        }

                        if (iEv <= 8)
                        {
                            for (int i = 0; i < 200; i++)
                            {
                                string sFrz = frz(Common.RandomInt(rnd, 0, 359));
                                string sMove = move(x, y, x + Common.RandomInt(rnd, 20, 120), y);
                                string sMoveT = t(0, 1, 0.5, fscx(Common.RandomInt(rnd, 5, 120) * 100).t());
                                ass_out.AppendEvent(80, "pt", t2, t2 + 1,
                                    org(x, y) + pos(x, y) + sMoveT + sFrz +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1" + r() + sFrz + fad(0, 0.4) +
                                    t(0, 0.3, fscx(700).t()) + t(0.3, 1, fscx(100).t()) +
                                    blur(1) + a(1, "00") + c(1, "FFFFFF") +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                                ass_out.AppendEvent(20, "pt", t2, t2 + 1,
                                    org(x, y) + pos(x, y) + sMoveT + sFrz +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1" + r() + sFrz + fad(0, 0.4) +
                                    t(0, 0.3, fscx(1000).t()) + t(0.3, 1, fscx(150).t()) +
                                    fscy(150) +
                                    blur(2) + a(1, "00") + c(1, mainColor) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");

                                continue;
                                // backup
                                ass_out.AppendEvent(20, "pt", t2, t2 + 1,
                                    org(x, y) + sMove + sFrz +
                                    t(0, 0.3, fscx(700).t()) + t(0.3, 1, fscx(100).t()) +
                                    blur(1) + a(1, "00") + c(1, "FFFFFF") +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                                ass_out.AppendEvent(19, "pt", t2, t2 + 1,
                                    org(x, y) + sMove + sFrz +
                                    t(0, 0.3, fscx(900).t()) + t(0.3, 1, fscx(100).t()) +
                                    fscy(120) +
                                    blur(2) + a(1, "00") + c(1, mainColor) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                            }
                        }
                    }

                    if (iEv >= 0 && iEv <= 4)
                    {
                        double t0 = ev.Start - 0.6;
                        double t1 = ev.Start;
                        double t2 = kStart;
                        double t3 = kEnd;
                        double t4 = ev.End;
                        double t5 = t4 + 0.6;

                        string mainColor = "FFFFFF";

                        Func<double, int> fScale = ti => (int)((ti <= t1 ? (Common.scaleDouble(150, 110, (ti - t0) / (t1 - t0))) :
                            (ti <= t4 ? (Common.scaleDouble(110, 90, (ti - t1) / (t4 - t1))) : (Common.scaleDouble(90, 50, (ti - t4) / (t5 - t4))))));
                        Func<double, double, string> tsScale = (_t0, _t1) => (fsc(fScale(_t0)) + t(fsc(fScale(_t1)).t()));

                        double dx = (x - this.PlayResX / 2);
                        Func<double, double> fKdx = ti => (ti <= t1 ? 1.8 : (ti <= t4 ? (Common.scaleDouble(1.8, 1, (ti - t1) / (t4 - t1))) :
                            Common.scaleDouble(1, 0.8, (ti - t4) / (t5 - t4))));
                        Func<double, double> fPosX = ti => (dx * fKdx(ti) + this.PlayResX / 2);

                        {
                            double rt = 0.5 + Math.Abs(dx) / 150;
                            for (int i = 0; i < 20 * rt; i++)
                            {
                                double tx0 = dx * (fKdx(t0) + (double)i * 0.05 / rt) + this.PlayResX / 2;
                                double tx1 = dx * (fKdx(t1) + (double)i * 0.03 / rt) + this.PlayResX / 2;
                                ass_out.AppendEvent(50, evStyle, t0, t1,
                                    fad(t1 - t0, 0) + move(tx0, y, tx1, y) + a(1, Common.scaleAlpha("AA", "FF", (double)i / (40 * rt))) + c(1, mainColor) + blur((double)(i + 1) * 0.5) +
                                    tsScale(t0, t1) +
                                    ke.KText);
                            }
                            for (int i = 0; i < 20 * rt; i++)
                            {
                                double tx0 = dx * (fKdx(t1) + (double)i * 0.03 / rt) + this.PlayResX / 2;
                                double tx1 = dx * (fKdx(t4) + (double)i * 0.00) + this.PlayResX / 2;
                                ass_out.AppendEvent(50, evStyle, t1, t4,
                                    move(tx0, y, tx1, y) + a(1, Common.scaleAlpha("AA", "FF", (double)i / (40 * rt))) + c(1, mainColor) + blur((double)(i + 1) * 0.5) +
                                    tsScale(t1, t4) +
                                    ke.KText);
                            }
                            for (int i = 0; i < 20 * rt; i++)
                            {
                                double tx0 = dx * (fKdx(t4) + (double)i * 0.00) + this.PlayResX / 2;
                                double tx1 = dx * (fKdx(t5) + (double)i * -0.05 / rt) + this.PlayResX / 2;
                                ass_out.AppendEvent(50, evStyle, t4, t5,
                                    fad(0, t5 - t4) +
                                    move(tx0, y, tx1, y) + a(1, Common.scaleAlpha("AA", "FF", (double)i / (40 * rt))) + c(1, mainColor) + blur((double)(i + 1) * 0.5) +
                                    tsScale(t4, t5) +
                                    ke.KText);
                            }
                        }
                    }
                }
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}

// 0 - 4
//Dialogue: 0,0:00:00.00,0:00:09.80,op_jp,NTP,0000,0000,0000,,{\K71}い{\K65}く{\K77}つ{\K78}も{\K144}の{\K25}　{\K32}岐{\K32}路{\K31}に{\K208}立{\K60}っ{\K157}て
//Dialogue: 0,0:00:10.73,0:00:18.87,op_jp,NTP,0000,0000,0000,,{\K141}悩{\K64}み{\K0}　{\K70}も{\K166}が{\K34}い{\K339}て
//Dialogue: 0,0:00:21.43,0:00:30.84,op_jp,NTP,0000,0000,0000,,{\K152}選{\K57}ば{\K49}れ{\K171}し{\K34}　{\K37}イ{\K18}バ{\K18}ラ{\K102}の{\K175}道{\K128}は
//Dialogue: 0,0:00:31.95,0:00:36.97,op_jp,NTP,0000,0000,0000,,{\K28}正{\K26}し{\K25}い{\K26}な{\K74}の{\K90}　{\K76}過{\K21}ち{\K35}な{\K101}の
//Dialogue: 0,0:00:38.34,0:00:44.33,op_jp,NTP,0000,0000,0000,,{\K144}教{\K96}え{\K359}て

// 5 - 8
//Dialogue: 0,0:00:46.96,0:00:50.00,op_jp,NTP,0000,0000,0000,,{\K42}大{\K36}胆{\K48}過{\K42}酷{\K37}な{\K48}選{\K36}択{\K16}を
//Dialogue: 0,0:00:50.00,0:00:53.35,op_jp,NTP,0000,0000,0000,,{\K29}強{\K41}い{\K21}ら{\K21}れ{\K41}る{\K22}た{\K8}び{\K12}に{\K0}　{\K20}逃{\K19}げ{\K37}道{\K22}絶{\K22}っ{\K20}て
//Dialogue: 0,0:00:53.55,0:00:56.05,op_jp,NTP,0000,0000,0000,,{\K57}弱{\K29}さ{\K38}の{\K38}裏{\K38}返{\K27}し{\K24}は
//Dialogue: 0,0:00:56.05,0:01:02.79,op_jp,NTP,0000,0000,0000,,{\K41}攻{\K29}撃{\K51}す{\K19}る{\K21}こ{\K33}と{\K31}し{\K21}か{\K0}　{\K82}思{\K82}い{\K143}が{\K10}出{\K10}な{\K101}い

// 9 - 
//Dialogue: 0,0:01:03.46,0:01:05.07,op_jp,NTP,0000,0000,0000,,{\K10}生{\K10}き{\K10}抜{\K10}く{\K20}た{\K10}め{\K21}に{\K21}君{\K12}が{\K19}必{\K19}要
//Dialogue: 0,0:01:05.07,0:01:06.73,op_jp,NTP,0000,0000,0000,,{\K28}貫{\K10}く{\K19}た{\K10}び{\K28}に{\K19}胸{\K11}は{\K19}動{\K22}揺
//Dialogue: 0,0:01:06.73,0:01:10.01,op_jp,NTP,0000,0000,0000,,{\K87}Dive{\K19}　{\K68}失{\K17}う{\K72}痛{\K25}み{\K22}へ{\K18}と
//Dialogue: 0,0:01:10.01,0:01:11.60,op_jp,NTP,0000,0000,0000,,{\K15}い{\K9}つ{\K8}の{\K9}間{\K19}に{\K10}か{\K25}投{\K18}げ{\K10}出{\K18}さ{\K9}れ{\K9}た
//Dialogue: 0,0:01:11.60,0:01:13.30,op_jp,NTP,0000,0000,0000,,{\K17}戦{\K24}場{\K18}の{\K30}全{\K26}て{\K15}に{\K10}お{\K10}い{\K10}て{\K10}の
//Dialogue: 0,0:01:13.30,0:01:15.75,op_jp,NTP,0000,0000,0000,,{\K65}Feeling{\K32}　{\K22}慣{\K23}れ{\K11}た{\K92}い
//Dialogue: 0,0:01:16.56,0:01:18.23,op_jp,NTP,0000,0000,0000,,{\K21}完{\K21}全{\K21}に{\K10}二{\K28}者{\K29}択{\K25}一{\K12}の
//Dialogue: 0,0:01:18.23,0:01:19.74,op_jp,NTP,0000,0000,0000,,{\K11}知{\K10}ら{\K10}ぬ{\K10}間{\K17}に{\K39}kiss{\K14}を{\K11}さ{\K18}れ{\K11}た
//Dialogue: 0,0:01:19.74,0:01:22.96,op_jp,NTP,0000,0000,0000,,{\K89}Sign{\K29}　{\K40}時{\K41}が{\K40}満{\K43}ち{\K40}て
//Dialogue: 0,0:01:22.96,0:01:27.90,op_jp,NTP,0000,0000,0000,,{\K93}白{\K25}い{\K42}悪{\K25}魔{\K0}　{\K83}逃{\K87}げ{\K89}な{\K50}い
