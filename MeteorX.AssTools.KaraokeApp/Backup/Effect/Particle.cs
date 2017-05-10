using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Effect
{
    class Particle
    {
        public ParticlePatternType ParticlePattern { get; set; }

        public double Start { get; set; }

        public double End { get; set; }

        public int XOffset { get; set; }

        public int YOffset { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int AreaHeight { get; set; }

        public int AreaWidth { get; set; }

        public int FontSize { get; set; }

        public bool IsMove { get; set; }

        /// <summary>
        /// 0 : No Move, 1 : Move Left, 2 : Move Around, 3 : 中心发散
        /// </summary>
        public int MoveStyle { get; set; }

        public int PatternScaleX { get; set; }

        public int PatternScaleY { get; set; }

        public bool IsPatternScale { get; set; }

        public ASSColor Color { get; set; }

        public bool IsRandomColor { get; set; }

        public ASSColor Color1 { get; set; }

        public ASSColor Color2 { get; set; }

        public int Count { get; set; }

        public string Style { get; set; }

        public double MinLast { get; set; }

        public double MaxLast { get; set; }

        public bool IsRotate { get; set; }

        public int BE { get; set; }

        Random rnd = new Random();

        public List<ASSEvent> Create()
        {
            List<ASSEvent> result = new List<ASSEvent>();
            
            for (int iCount = 0; iCount < Count; iCount++)
            {
                ASSEvent ev = new ASSEvent
                {
                    Layer = 0,
                    Effect = "",
                    Name = "NTP",
                    MarginL = "0000",
                    MarginR = "0000",
                    MarginV = "0000",
                    Style = Style,
                    Start = Common.RandomDouble(rnd, Start, End)
                };
                double last = Common.RandomDouble(rnd, MinLast, MaxLast);
                ev.End = Start + last;
                ev.Text = ASSEffect.an(5) + ASSEffect.fad(0, last);
                if (IsMove && MoveStyle == 1)
                {
                    int x1 = Common.RandomInt(rnd, X, X + FontSize);
                    int y1 = Common.RandomInt(rnd, Y, Y + FontSize);
                    int y2 = Common.RandomInt(rnd, Y + FontSize / 2 - AreaHeight / 2, Y + FontSize / 2 + AreaHeight / 2);
                    int x2 = Common.RandomInt(rnd, X - AreaWidth, X);
                    ev.Text += ASSEffect.move(x1 + XOffset, y1 + YOffset, x2 + XOffset, y2 + YOffset);
                }
                else if (IsMove && MoveStyle == 2)
                {
                    int x1 = Common.RandomInt(rnd, X, X + FontSize);
                    int y1 = Common.RandomInt(rnd, Y, Y + FontSize);
                    int y2 = Common.RandomInt(rnd, Y + FontSize / 2 - AreaHeight / 2, Y + FontSize / 2 + AreaHeight / 2);
                    int x2 = Common.RandomInt(rnd, X + FontSize / 2 - AreaWidth / 2, X + FontSize / 2 + AreaWidth / 2);
                    ev.Text += ASSEffect.move(x1 + XOffset, y1 + YOffset, x2 + XOffset, y2 + YOffset);
                }
                else if (IsMove && MoveStyle == 3)
                {
                    int x1 = X + FontSize / 2;
                    int y1 = Y + FontSize / 2;
                    int y2 = Common.RandomInt(rnd, y1 - AreaHeight / 2, y1 + AreaHeight / 2);
                    int x2 = Common.RandomInt(rnd, x1 - AreaWidth / 2, x1 + AreaWidth / 2);
                    ev.Text += ASSEffect.move(x1 + XOffset, y1 + YOffset, x2 + XOffset, y2 + YOffset);
                }
                else ev.Text += ASSEffect.pos(X + FontSize / 2 + XOffset, Y + FontSize / 2 + YOffset);
                if (!IsRandomColor)
                    ev.Text += ASSEffect.c(Color);
                else
                    ev.Text += ASSEffect.c(Common.RandomColor(rnd, 1, Color1, Color2));
                if (BE > 0)
                    ev.Text += ASSEffect.be(BE);
                if (IsPatternScale) ev.Text += ASSEffect.t(0, last, @"\fscx" + PatternScaleX + @"\fscy" + PatternScaleY);
                if (IsRotate) ev.Text += ASSEffect.t(0, last, ASSEffect.frz((int)(last * 1000)).t());
                ev.Text += (char)ParticlePattern;

                result.Add(ev);
            }
            return result;
        }
    }

    enum ParticlePatternType
    {
        Default = ' ',
        Circle = '○',
        Star = '☆',
        X = '×',
        A = 'A',
        a = 'a',
        h = 'h',
        o = 'o'
    };
}
