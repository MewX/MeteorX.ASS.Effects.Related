using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class ClannadAS_OP
    {
        public string InFileName = @"G:\Workshop\clannad\op_org.ass";
        public string OutFileName = @"G:\Workshop\clannad\op.ass";
        public int FontWidth = 21;
        public int FontHeight = 21;
        public int MarginLeft = 10;
        public int MarginRight = 10;
        public int MarginTop = 10;
        public int MarginBottom = 10;
        public int PlayResX = 640;
        public int PlayResY = 360;
        public double Shift = 0;

        public void Run()
        {
            ASS ass = ASS.FromFile(InFileName);
            ASS outass = new ASS();
            outass.Header = ass.Header;
            outass.Events = new List<ASSEvent>();

            for (int countEvent = 0; countEvent < 1; countEvent++)
            {
                ASSEvent ev = ass.Events[countEvent];
                List<KElement> kelems = ev.SplitK();
                int kSum = 0;
                for (int iKelems = 0; iKelems < kelems.Count; iKelems++)
                {
                    KElement kelem = kelems[iKelems];
                    int x = (PlayResX - MarginLeft - MarginRight - (kelems.Count * FontWidth)) / 2 + MarginLeft + iKelems * FontWidth;
                    int y = MarginTop;
                    double scale = (double)(kelems.Count - iKelems) / (double)kelems.Count;
                    double kStart = (double)kSum * 0.01;
                    double kEnd = (double)(kSum + kelem.KValue) * 0.01;
                    kSum += kelem.KValue;
                    outass.Events.Add(ev.StartOffset(-0.8 * scale).TextReplace(
                        ASSEffect.t(0, 0.6, @"\1a&HDD&\3a&H11&") + // fade in
                        ASSEffect.t(kStart + 0.8 * scale + 0.2, kStart + 0.8 * scale + 0.2 + 0.1, @"\3a&HFF&") + // remove outline
                        ASSEffect.t(ev.Last - 0.6 - 0.8 * scale, ev.Last - 0.8 * scale, @"\1a&HFF&\3a&HFF&") + // fade out
                        ASSEffect.pos(x, y) + kelem.KText));
                    outass.Events.Add(ev.LayerReplace(1).StartOffset(kStart + 0.2).TextReplace(
                        ASSEffect.a(1, "00") + ASSEffect.c(1, "333333") + ASSEffect.a(3, "FF") + // color
                        ASSEffect.fad(0.3, 0) + // fade
                        ASSEffect.t(0, 0.1, @"\3a&HFF&") + // remove outline
                        ASSEffect.t(ev.Last - (kStart + 0.2) - 0.6 - 0.8 * scale, ev.Last - (kStart + 0.2) - 0.8 * scale, @"\1a&HFF&") + // fade out
                        ASSEffect.pos(x, y) + kelem.KText));
                    outass.Events.Add(ev.LayerReplace(1).StartOffset(kStart).EndReplace(ev.Start + kStart + 0.3).TextReplace(
                        ASSEffect.a(1, "AA") + ASSEffect.c(1, "333333") + ASSEffect.a(3, "FF") + // color
                        ASSEffect.fad(0.2, 0.1) + // fade
                        ASSEffect.move(x, y - 5, x, y) + kelem.KText));
                    outass.Events.Add(ev.LayerReplace(1).StartOffset(kStart).EndReplace(ev.Start + kStart + 0.3).TextReplace(
                        ASSEffect.a(1, "AA") + ASSEffect.c(1, "333333") + ASSEffect.a(3, "FF") + // color
                        ASSEffect.fad(0.2, 0.1) + // fade
                        ASSEffect.move(x, y + 5, x, y) + kelem.KText));
                }
            }

            for (int countEvent = 1; countEvent < 4; countEvent++)
            {
                ASSEvent ev = ass.Events[countEvent];
                List<KElement> kelems = ev.SplitK();
                int kSum = 0;
                for (int iKelems = 0; iKelems < kelems.Count; iKelems++)
                {
                    KElement kelem = kelems[iKelems];
                    int x = (PlayResX - MarginLeft - MarginRight - (kelems.Count * FontWidth)) / 2 + MarginLeft + iKelems * FontWidth;
                    int y = MarginTop;
                    double scale = (double)(kelems.Count - iKelems) / (double)kelems.Count;
                    double kStart = (double)kSum * 0.01;
                    double kEnd = (double)(kSum + kelem.KValue) * 0.01;
                    kSum += kelem.KValue;
                    outass.Events.Add(ev.StartOffset(-0.8 * scale).TextReplace(
                        ASSEffect.t(0, 0.6, @"\1a&HDD&\3a&H11&") + // fade in
                        ASSEffect.t(ev.Last - 0.6 - 0.8 * scale, ev.Last - 0.8 * scale, @"\1a&HFF&\3a&HFF&") + // fade out
                        ASSEffect.pos(x, y) + kelem.KText));
                    outass.Events.Add(ev.LayerReplace(1).StartOffset(kStart + 0.2).TextReplace(
                        ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "FF") + // color
                        ASSEffect.fad(0.3, 0) + // fade
                        ASSEffect.t(ev.Last - (kStart + 0.2) - 0.6 - 0.8 * scale, ev.Last - (kStart + 0.2) - 0.8 * scale, @"\1a&HFF&") + // fade out
                        ASSEffect.pos(x, y) + kelem.KText));
                    outass.Events.Add(ev.LayerReplace(1).StartOffset(kStart).EndReplace(ev.Start + kStart + 0.3).TextReplace(
                        ASSEffect.a(1, "AA") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "FF") + // color
                        ASSEffect.fad(0.2, 0.1) + // fade
                        ASSEffect.move(x, y - 5, x, y) + kelem.KText));
                    outass.Events.Add(ev.LayerReplace(1).StartOffset(kStart).EndReplace(ev.Start + kStart + 0.3).TextReplace(
                        ASSEffect.a(1, "AA") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "FF") + // color
                        ASSEffect.fad(0.2, 0.1) + // fade
                        ASSEffect.move(x, y + 5, x, y) + kelem.KText));
                }
            }

            for (int countEvent = 4; countEvent < 8; countEvent++)
            {
                ASSEvent ev = ass.Events[countEvent];
                List<KElement> kelems = ev.SplitK();
                int kSum = 0;
                for (int iKelems = 0; iKelems < kelems.Count; iKelems++)
                {
                    KElement kelem = kelems[iKelems];
                    int x = (countEvent <= 5) ? (MarginLeft + iKelems * FontWidth) : (PlayResX - MarginRight - (kelems.Count - iKelems) * FontWidth);
                    int y = MarginTop;
                    double scale = (double)(kelems.Count - iKelems) / (double)kelems.Count;
                    double kStart = (double)kSum * 0.01;
                    double kEnd = (double)(kSum + kelem.KValue) * 0.01;
                    kSum += kelem.KValue;

                    double t1 = ev.Start - 0.8 * scale;
                    double t2 = t1 + 0.6;
                    double t3 = ev.End - 0.8 * scale;
                    double t4 = t3 + 0.6;
                    outass.Events.Add(ev.StartReplace(t1).EndReplace(t2).TextReplace(
                        ASSEffect.a(2, "00") + ASSEffect.c(2, "FFFFFF") + 
                        ASSEffect.t(0, 0.6, @"\1a&HDD&\3a&H11&") + // fade in
                        ASSEffect.move(x, y - 8, x, y) + kelem.KText));
                    outass.Events.Add(ev.StartReplace(t2).EndReplace(t3).TextReplace(
                        ASSEffect.a(2, "00") + ASSEffect.c(2, "FFFFFF") +
                        ASSEffect.a(1, "DD") + ASSEffect.a(3, "11") + // hold
                        ASSEffect.pos(x, y) + kelem.KText));
                    outass.Events.Add(ev.StartReplace(t3).EndReplace(t4).TextReplace(
                        ASSEffect.a(1, "00") + ASSEffect.c(2, "FFFFFF") + ASSEffect.a(3, "11") +
                        ASSEffect.t(0, 0.6, @"\1a&HFF&\3a&HFF&") + // fade out
                        ASSEffect.move(x, y, x, y + 8) + kelem.KText));
                    outass.Events.Add(ev.LayerReplace(1).StartReplace(ev.Start + kStart).EndReplace(t3).TextReplace(
                        ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                        ASSEffect.a(2, "DD") +
                        ASSEffect.K(kEnd - kStart) + // karaoke
                        ASSEffect.pos(x, y) + kelem.KText));
                }
            }

            for (int countEvent = 8; countEvent < 12; countEvent++)
            {
                ASSEvent ev = ass.Events[countEvent];
                List<KElement> kelems = ev.SplitK();
                int kSum = 0;
                for (int iKelems = 0; iKelems < kelems.Count; iKelems++)
                {
                    KElement kelem = kelems[iKelems];
                    int x = MarginLeft + (countEvent - 8) * FontWidth;
                    int y = MarginTop + iKelems * FontHeight;
                    double scale = (double)(kelems.Count - iKelems) / (double)kelems.Count;
                    double kStart = (double)kSum * 0.01;
                    double kEnd = (double)(kSum + kelem.KValue) * 0.01;
                    kSum += kelem.KValue;
                    outass.Events.Add(ev.EndOffset(1.0).TextReplace(
                        ASSEffect.t(0, 0.2, @"\1a&HDD&\3a&H11&") + // fade in
                        ASSEffect.t(ev.Last + 1.0 - 0.6, ev.Last + 1.0, @"\1a&HFF&\3a&HFF&") + // fade out
                        ASSEffect.pos(x, y) + kelem.KText));
                    outass.Events.Add(ev.StartOffset(kStart).EndReplace(ev.Start + kStart + 0.3).TextReplace(
                        ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                        ASSEffect.fad(0.2, 0.1) +
                        ASSEffect.pos(x, y) + kelem.KText));
                }
            }

            for (int countEvent = 12; countEvent < 16; countEvent++)
            {
                ASSEvent ev = ass.Events[countEvent];
                List<KElement> kelems = ev.SplitK();
                int kSum = 0;
                for (int iKelems = 0; iKelems < kelems.Count; iKelems++)
                {
                    KElement kelem = kelems[iKelems];
                    int x = PlayResX - MarginRight - (countEvent - 11) * FontWidth;
                    int y = MarginTop + iKelems * FontHeight;
                    double scale = (double)(kelems.Count - iKelems) / (double)kelems.Count;
                    double kStart = (double)kSum * 0.01;
                    double kEnd = (double)(kSum + kelem.KValue) * 0.01;
                    kSum += kelem.KValue;
                    outass.Events.Add(ev.EndOffset(1.0).TextReplace(
                        ASSEffect.t(0, 0.2, @"\1a&HDD&\3a&H11&") + // fade in
                        ASSEffect.t(ev.Last + 1.0 - 0.6, ev.Last + 1.0, @"\1a&HFF&\3a&HFF&") + // fade out
                        ASSEffect.pos(x, y) + kelem.KText));
                    outass.Events.Add(ev.StartOffset(kStart).EndReplace(ev.Start + kStart + 0.3).TextReplace(
                        ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                        ASSEffect.fad(0.2, 0.1) +
                        ASSEffect.pos(x, y) + kelem.KText));
                }
            }

            for (int countEvent = 16; countEvent < 20; countEvent++)
            {
                ASSEvent ev = ass.Events[countEvent];
                List<KElement> kelems = ev.SplitK();
                int kSum = 0;
                for (int iKelems = 0; iKelems < kelems.Count; iKelems++)
                {
                    KElement kelem = kelems[iKelems];
                    int x = MarginLeft + (countEvent - 16) * FontWidth;
                    int y = MarginTop + iKelems * FontHeight;
                    double scale = (double)(kelems.Count - iKelems) / (double)kelems.Count;
                    double kStart = (double)kSum * 0.01;
                    double kEnd = (double)(kSum + kelem.KValue) * 0.01;
                    kSum += kelem.KValue;
                    outass.Events.Add(ev.EndOffset(1.0).TextReplace(
                        ASSEffect.t(0, 0.2, @"\1a&HDD&\3a&H11&") + // fade in
                        ASSEffect.t(ev.Last + 1.0 - 0.6, ev.Last + 1.0, @"\1a&HFF&\3a&HFF&") + // fade out
                        ASSEffect.pos(x, y) + kelem.KText));
                    outass.Events.Add(ev.StartOffset(kStart).EndReplace(ev.Start + kStart + 0.3).TextReplace(
                        ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                        ASSEffect.fad(0.2, 0.1) +
                        ASSEffect.pos(x, y) + kelem.KText));
                }
            }

            for (int countEvent = 20; countEvent < 23; countEvent++)
            {
                ASSEvent ev = ass.Events[countEvent];
                List<KElement> kelems = ev.SplitK();
                int kSum = 0;
                for (int iKelems = 0; iKelems < kelems.Count; iKelems++)
                {
                    KElement kelem = kelems[iKelems];
                    int x = PlayResX - MarginRight - (countEvent - 19) * FontWidth;
                    int y = MarginTop + iKelems * FontHeight;
                    double scale = (double)(kelems.Count - iKelems) / (double)kelems.Count;
                    double kStart = (double)kSum * 0.01;
                    double kEnd = (double)(kSum + kelem.KValue) * 0.01;
                    kSum += kelem.KValue;
                    outass.Events.Add(ev.EndOffset(1.0).TextReplace(
                        ASSEffect.t(0, 0.2, @"\1a&HDD&\3a&H11&") + // fade in
                        ASSEffect.t(ev.Last + 1.0 - 0.6, ev.Last + 1.0, @"\1a&HFF&\3a&HFF&") + // fade out
                        ASSEffect.pos(x, y) + kelem.KText));
                    if (countEvent == 22 && iKelems == kelems.Count - 1)
                        outass.Events.Add(ev.StartOffset(kStart).EndReplace(ev.Start + kStart + 1.2).TextReplace(
                            ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                            ASSEffect.fad(0.2, 1) +
                            ASSEffect.pos(x, y) + kelem.KText));
                    else
                        outass.Events.Add(ev.StartOffset(kStart).EndReplace(ev.Start + kStart + 0.3).TextReplace(
                            ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                            ASSEffect.fad(0.2, 0.1) +
                            ASSEffect.pos(x, y) + kelem.KText));
                }
            }

            for (int countEvent = 23; countEvent < ass.Events.Count; countEvent++)
                outass.Events.Add(ass.Events[countEvent]);

            outass.Shift(Shift);
            outass.SaveFile(OutFileName);
        }
    }
}
