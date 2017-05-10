using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestPIExport1 : BaseAnime2
    {
        public TestPIExport1()
        {
            InFileName = @"G:\Workshop\test\7\0.ass";
            OutFileName = @"G:\Workshop\test\7\1.ass";

            this.FontWidth = 30;
            this.FontHeight = 30;
            this.FontSpace = 3;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 10;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("HGPSoeiKakugothicUB", 30, GraphicsUnit.Pixel);
            this.MaskStyle = "Style: Default,DFGMaruGothic-Md,30,&H00FF0000,&HFF600D00,&H000000FF,&HFF0A5A84,-1,0,0,0,100,100,0,0,0,2,0,5,20,20,20,128";
            this.IsAvsMask = true;
        }

        public override void Run()
        {
            for (int i = 0; i < 3; i++)
            {
                ParticleIllusionExporter pie = new ParticleIllusionExporter();
                pie.Init();
                CompositeCurve curve = new CompositeCurve { MinT = 0, MaxT = 10 };
                Circle circle = new Circle { MinT = 0 + (double)i * Math.PI * 2.0 / 3.0, MaxT = Math.PI * 2 * 10 + (double)i * Math.PI * 2.0 / 3.0, R = 100, X0 = 0, Y0 =0 };
                CompositeCurve path = new CompositeCurve { MinT = 0, MaxT = 10 };
/*                path.AddCurve(0, 2.5, new Line { X0 = 100, Y0 = 100, X1 = 500, Y1 = 100 });
                path.AddCurve(2.5, 5, new Line { X0 = 500, Y0 = 100, X1 = 500, Y1 = 300 });
                path.AddCurve(5, 7.5, new Line { X0 = 500, Y0 = 300, X1 = 100, Y1 = 300 });
                path.AddCurve(7.5, 10, new Line { X0 = 100, Y0 = 300, X1 = 100, Y1 = 100 });*/
                path.AddCurve(0, 10, new Circle { X0 = 250, Y0 = 250, R = 130 });
                CompoundCurve cc = new CompoundCurve { MinT = 0, MaxT = 10 };
                cc.AddCurve(0, 10, circle);
                cc.AddCurve(0, 10, path);
                curve.AddCurve(curve.MinT, curve.MaxT, cc);
                foreach (ASSPointF pt in curve.GetPath_DT(0.01))
                {
                    pie.Add(pt.T, pt);
                }
                pie.SaveToFile(@"g:\workshop\test\" + i + ".txt");
            }
        }
    }
}
