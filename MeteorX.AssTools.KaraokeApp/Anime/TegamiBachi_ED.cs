using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class TegamiBachi_ED : BaseAnime2
    {
        public TegamiBachi_ED()
        {
            this.FontHeight = 38;
            this.FontSpace = 3;
            this.IsAvsMask = true;
            this.MarginLeft = 30;
            this.MarginBottom = 30;
            this.MarginTop = 30;
            this.MarginRight = 30;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"G:\Workshop\tegami bachi\ed\ed_k.ass";
            this.OutFileName = @"G:\Workshop\tegami bachi\ed\ed.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                ASSEvent ev = ass_in.Events[iEv];
                bool isJp = true;
                this.MaskStyle = "Style: Default,DFGMaruGothic-Md,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128";

                //if (iEv != 0) continue;

                List<KElement> kelems = ev.SplitK(!isJp);
                int tw = GetTotalWidth(ev);
                int x0 = (PlayResX - MarginLeft - MarginRight - tw) / 2 + MarginLeft;
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

                    double t0 = ev.Start - 0.6 + iK * 0.08;
                    double t1 = t0 + 0.6;
                    double t2 = kStart;
                    double t3 = kEnd;
                    double t4 = ev.End - 0.6 + iK * 0.08;
                    double t5 = t4 + 0.6;

                    double xCenter = PlayResX * 0.5;
                    double dxCenter = x - xCenter;

                    ass_out.AppendEvent(50, evStyle, t0, t1,
                        pos(x, y) + fad(t1 - t0, 0) + a(1, "00") +
                        blur(10) + t(blur(0.5).t()) +
                        ke.KText);
                    ass_out.AppendEvent(50, evStyle, t1, t5,
                        pos(x, y) + fad(0, t5 - t4) + a(1, "00") +
                        blur(0.5) +
                        ke.KText);

                    ass_out.AppendEvent(48, evStyle, t0, t5,
                        pos(x + 1.5, y + 1.5) + fad(t1 - t0, t5 - t4) +
                        a(1, "22") + c(1, "000000") + blur(1) +
                        ke.KText);
                    ass_out.AppendEvent(49, evStyle, t0, t5,
                        pos(x, y) + fad(t1 - t0, t5 - t4) +
                        a(1, "55") + c(1, "000000") + blur(3) +
                        ke.KText);


                }
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}

/*
Dialogue: 0,0:00:04.81,0:00:09.81,ed_jp,NTP,0000,0000,0000,,{\K69}曖{\K99}昧{\K80}に{\K50}　{\K27}見{\K27}え{\K49}隠{\K11}れ{\K87}る
Dialogue: 0,0:00:12.25,0:00:17.29,ed_jp,NTP,0000,0000,0000,,{\K81}空{\K43}を{\K53}跨{\K92}ぐ{\K46}雲{\K21}の{\K22}よ{\K22}う{\K27}な{\K13}日{\K84}々
Dialogue: 0,0:00:19.08,0:00:25.97,ed_jp,NTP,0000,0000,0000,,{\K72}瞳{\K74}の{\K93}奥{\K83}で{\K51}　{\K25}モ{\K26}ノ{\K25}ク{\K23}ロ{\K46}の{\K48}記{\K95}憶{\K28}が
Dialogue: 0,0:00:26.13,0:00:35.44,ed_jp,NTP,0000,0000,0000,,{\K97}蘇{\K24}り{\K29}あ{\K16}ふ{\K57}れ{\K30}そ{\K29}う{\K23}な{\K97}涙{\K24}　{\K22}こ{\K22}ら{\K24}え{\K187}急{\K251}ぐ
 * 
Dialogue: 0,0:00:36.65,0:00:41.64,ed_jp,NTP,0000,0000,0000,,{\K24}過{\K17}去{\K58}と{\K34}今{\K30}と{\K28}未{\K49}来{\K21}の{\K68}狭{\K27}間{\K143}で
Dialogue: 0,0:00:44.11,0:00:53.06,ed_jp,NTP,0000,0000,0000,,{\K46}何{\K25}が{\K60}正{\K45}解{\K79}か{\K12}　{\K22}手{\K52}探{\K20}り{\K71}な{\K71}ま{\K26}ま{\K22}う{\K23}ね{\K71}る{\K250}旅
 * 
Dialogue: 0,0:00:53.52,0:01:01.35,ed_jp,NTP,0000,0000,0000,,{\K78}果{\K14}て{\K75}な{\K26}き{\K76}道{\K46}で{\K68}出{\K36}会{\K81}った{\K23}こ{\K25}の{\K59}奇{\K104}跡{\K72}は
Dialogue: 0,0:01:01.35,0:01:03.88,ed_jp,NTP,0000,0000,0000,,{\K70}Life{\K22} {\K27}This{\K0} {\K22}is{\K0} {\K46}My{\K0} {\K67}Life
Dialogue: 0,0:01:04.16,0:01:08.19,ed_jp,NTP,0000,0000,0000,,{\K48}今{\K22}あ{\K23}な{\K14}た{\K75}の{\K70}元{\K151}へ
Dialogue: 0,0:01:08.57,0:01:16.22,ed_jp,NTP,0000,0000,0000,,{\K40}声{\K30}に{\K30}な{\K22}ら{\K14}な{\K35}い{\K66}想{\K27}い{\K39}は{\K48}夜{\K64}空{\K124}の{\K70}星{\K29}へ{\K128}と
Dialogue: 0,0:01:16.22,0:01:18.92,ed_jp,NTP,0000,0000,0000,,{\K88}Sky{\K18} {\K50}into{\K0} {\K30}The{\K0} {\K85}Sky
Dialogue: 0,0:01:19.14,0:01:23.46,ed_jp,NTP,0000,0000,0000,,{\K26}生{\K24}き{\K20}て{\K42}ゆ{\K53}く{\K22}　{\K27}in{\K0} {\K35}My{\K0} {\K183}Soul
*/
