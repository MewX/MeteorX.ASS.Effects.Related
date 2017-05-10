using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Effect
{
    /// <summary>
    /// 移动的一个对象, 移动过程中散发粒子, 用户定义移动路径, 拟合方式等等
    /// </summary>
    class MovParticle
    {
        public ASSColor MainColor { get; set; }

        public List<MovParticlePathElem> Path { get; set; }

        public MovParticle()
        {
            MainColor = new ASSColor { A = 0, R = 255, G = 255, B = 255, Index = 1 };
            Path = new List<MovParticlePathElem>();
        }

        public void AppendPoint(double t, int x, int y)
        {
            Path.Add(new MovParticlePathElem { Time = t, X = x, Y = y });
        }

        public List<ASSEvent> Create()
        {
            Path.Sort(ComparePathElemFunc);

            throw new NotImplementedException();
        }

        static int ComparePathElemFunc(MovParticlePathElem e1, MovParticlePathElem e2)
        {
            if (e1.Time < e2.Time) return -1;
            else if (e1.Time > e2.Time) return 1;
            else return 0;
        }

        public void Test()
        {
            Console.WriteLine(MainColor.A);
        }
    }

    class MovParticlePathElem
    {
        public double Time { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
