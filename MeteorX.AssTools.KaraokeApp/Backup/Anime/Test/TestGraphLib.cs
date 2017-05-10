using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;
using MeteorX.AssTools.KaraokeApp.Model;
using System.Runtime.InteropServices;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestGraphLib : BaseAnime2
    {
        public TestGraphLib()
        {
            InFileName = @"G:\Workshop\test\7\0.ass";
            OutFileName = @"G:\Workshop\test\7\1.ass";

            this.FontWidth = 30;
            this.FontHeight = 30;
            this.FontSpace = 2;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 10;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("ＤＦＰまるもじ体W3", 30, GraphicsUnit.Pixel);
            this.MaskStyle = "Style: Default,ＤＦＰまるもじ体W3,30,&H00FFFFFF,&HFF000000,&H00000000,&HFF000000,0,0,0,0,100,100,2,0,1,2,0,5,30,30,10,128";
            this.IsAvsMask = true;
        }

        [DllImport("GraphLib.dll", CharSet = CharSet.Unicode)]
        static extern bool GetOutline(char[] buf, ref int buflen, char thisChar, char[] fontName, byte fontCharset, int fontHeight, int fontWeight, short yOffset, short OX, short OY);

        [DllImport("GraphLib.dll", CharSet = CharSet.Unicode)]
        static extern string TestHelloWorld();

        [DllImport("GraphLib.dll", CharSet = CharSet.Unicode)]
        static extern int IntAdd(int a, int b);

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            int c = IntAdd(2, 3);

            Random rnd = new Random();

            //string s = TestHelloWorld();
            string fn = "ＦＡ 瑞筆行書Ｍ";
            string ss = "";
            char[] cc = new char[10000];
            int cclen = 1;
            bool b1 = GetOutline(cc, ref cclen, '々', Encoding.Unicode.GetChars(Encoding.Unicode.GetBytes(fn)), 128, 30 * 8, 1000, 564, 424 * 8, 240 * 8);
            for (int i = 0; i < cclen; i++)
                ss += cc[i];

            //ass_out.AppendEvent(0, "pt", 0, 1, ASSEffect.pos(424, 240) + @"{\an7\p4}" + ss);
            ass_out.AppendEvent(0, "pt", 0, 10, ASSEffect.pos(0, 0) + @"{\an7\clip(4," + ss + @")\1a&H77&\p1} m -1000 -1000 l 1000 -1000 1000 1000 -1000 1000");
            for (int i = 0; i < 10; i++)
            {
                ASSColor co = Common.RandomColor(rnd, 1, ASSColor.FromBBGGRR(1, "000000"), ASSColor.FromBBGGRR(1, "FFFFFF"));
                ass_out.AppendEvent(1, "pt", (double)i * 0.5, (double)i * 0.5 + 1, ASSEffect.move(400, 0, 500, 0) + ASSEffect.c(1, co.ToColString()) + ASSEffect.c(3, co.ToColString()) + @"{\an7\clip(4," + ss + @")\1a&H00&\3a&H00&\bord25\blur25\p1} m 0 0 l 1 0 1 480 0 480");
            }
/*            ass_out.AppendEvent(0, "pt", 0, 10,
                ASSEffect.pos(0, 0) + @"{\an7\p1}m -1000 240 l 1000 240 1000 241 -1000 241"
                );
            ass_out.AppendEvent(0, "pt", 0, 10,
                ASSEffect.pos(0, 0) + @"{\an7\p1}m 424 -1000 l 425 -1000 425 1000 424 1000"
                );
            ass_out.AppendEvent(0, "Default", 1, 10,
                ASSEffect.pos(0, 0) + ASSEffect.an(7) + '々');*/

            ass_out.SaveFile(OutFileName);
        }
    }
}
