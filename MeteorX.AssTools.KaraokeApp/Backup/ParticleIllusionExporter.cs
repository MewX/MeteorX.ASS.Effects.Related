using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteorX.AssTools.KaraokeApp
{
    class ParticleIllusionExporter
    {
        double FrameRate { get; set; }

        double StartOffset { get; set; }

        List<KeyValuePair<int, ASSPointF>> traces = new List<KeyValuePair<int, ASSPointF>>();

        public ParticleIllusionExporter()
        {
            FrameRate = 23.976;
            StartOffset = 0;
        }

        public void Init()
        {
            Add(0, new ASSPointF { X = -200, Y = -200 });
        }

        public void Add(int frame, ASSPointF pos)
        {
            traces.Add(new KeyValuePair<int, ASSPointF>(frame, pos));
        }

        public void Add(double time, ASSPointF pos)
        {
            Add((int)((time - StartOffset) * FrameRate), pos);
        }

        public void Add(double time, double x, double y)
        {
            Add((int)((time - StartOffset) * FrameRate), new ASSPointF(x, y));
        }

        public void Teleport(int frame, ASSPointF pos)
        {
            if (traces.Count > 0)
                Add(frame - 1, traces.Last().Value);
            Add(frame, pos);
        }

        public void Teleport(double time, ASSPointF pos)
        {
            Teleport((int)((time - StartOffset) * FrameRate), pos);
        }

        public void Teleport(double time, double x, double y)
        {
            Teleport((int)((time - StartOffset) * FrameRate), new ASSPointF(x, y));
        }

        public void SaveToFile(string filename)
        {
            using (StreamWriter fout = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.Default))
            {
                foreach (KeyValuePair<int, ASSPointF> pair in traces)
                {
                    fout.WriteLine("{0}\t{1:0.000}\t{2:0.000}", pair.Key, pair.Value.X, pair.Value.Y);
                }
            }
        }
    }
}
