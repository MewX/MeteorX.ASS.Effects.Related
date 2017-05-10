using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Toys
{
    public class Toy1
    {
        public double OX { get; set; }

        public double OY { get; set; }

        public int V { get; set; }

        public double MinR { get; set; }

        public double MaxR { get; set; }

        public double MinDAG { get; set; }

        public double MaxDAG { get; set; }

        public bool IsSort { get; set; }

        public Toy1()
        {
            IsSort = false;
        }

        protected static Random rnd = new Random();

        double[] r, ag, dag;

        public void Reset()
        {
            r = new double[V];
            ag = new double[V];
            dag = new double[V];

            for (int i = 0; i < V; i++)
            {
                r[i] = Common.RandomDouble(rnd, MinR, MaxR);
                ag[i] = Math.PI * 2.0 * (double)i / (double)V;
                dag[i] = Common.RandomDouble(rnd, MinDAG, MaxDAG);
            }
        }

        public List<ASSPointF> Next()
        {
            for (int i = 0; i < V; i++)
            {
                ag[i] += dag[i];
                while (ag[i] >= Math.PI * 2)
                    ag[i] -= Math.PI * 2;
            }

            if (IsSort)
            {
                for (int i = 0; i + 1 < V; i++)
                    for (int j = i + 1; j < V; j++)
                        if (ag[i] > ag[j])
                        {
                            double tmp = ag[i];
                            ag[i] = ag[j];
                            ag[j] = tmp;
                            tmp = r[i];
                            r[i] = r[j];
                            r[j] = tmp;
                            tmp = dag[i];
                            dag[i] = dag[j];
                            dag[j] = tmp;
                        }
            }
            List<ASSPointF> pts = new List<ASSPointF>();
            for (int i = 0; i < V; i++)
            {
                pts.Add(new ASSPointF { X = OX + r[i] * Math.Cos(ag[i]), Y = OY + r[i] * Math.Sin(ag[i]) });
            }
            return pts;
        }
    }
}
