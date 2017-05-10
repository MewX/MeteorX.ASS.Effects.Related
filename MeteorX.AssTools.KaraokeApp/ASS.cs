using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteorX.AssTools.KaraokeApp
{
    public class ASS
    {
        public List<string> Header { get; set; }
        public List<ASSEvent> Events { get; set; }

        public void Shift(double offset)
        {
            foreach (ASSEvent ev in Events)
            {
                ev.Start += offset;
                ev.End += offset;
            }
        }

        public void AppendEvent(int layer, string style, double start, double end, string text)
        {
            if (end <= start) return;

            this.Events.Add(
                new ASSEvent
                {
                    Effect = "",
                    Layer = layer,
                    MarginL = "0000",
                    MarginR = "0000",
                    MarginV = "0000",
                    Name = "NTP",
                    Style = style,
                    Start = start,
                    End = end,
                    Text = text
                });
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lineSize"></param>
        public void SaveFiles(string name, int lineSize)
        {
            string batFile = @"G:\Workshop\natsume2\01\test_op.bat";
            string lastFileName = @"G:\Workshop\natsume2\01\(2009Q1) 続 夏目友人帳 - 第01話 【 奪われた友人帳 】 TX 1280x720 x264.mp4";
            using (StreamWriter bat = new StreamWriter(new FileStream(batFile, FileMode.Create), Encoding.Default))
            {
                for (int i = 0; i < Events.Count; i += lineSize)
                {
                    string idStr = (i / lineSize).ToString().PadLeft(5, '0');
                    using (StreamWriter avs = new StreamWriter(new FileStream(@"G:\Workshop\natsume2\01\test_op" + idStr + ".avs", FileMode.Create), Encoding.Default))
                    {
                        avs.WriteLine(@"fin = """ + lastFileName + "\"");
                        avs.WriteLine("fms(fin).a24()");
                        if (i == 0) avs.WriteLine("rz(1696, 960)");
                        string filename = name + idStr + ".ass";
                        avs.WriteLine("ts(\"" + filename + "\")");
                        if (i + lineSize >= Events.Count) avs.WriteLine("rz(848, 480)");
                        avs.WriteLine("trim(0, 2157).yv12()");
                        using (StreamWriter fout = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.Unicode))
                        {
                            foreach (string line in Header)
                                fout.WriteLine(line);
                            for (int j = i; j < Events.Count && j - i < lineSize; j++)
                                fout.WriteLine(Events[j]);
                        }
                    }
                    lastFileName = @"G:\Workshop\natsume2\01\test_op" + idStr + ".mp4";
                    bat.WriteLine(@"start /B /wait /low d:\tools\megui\tools\x264\x264.exe --crf 10 --keyint 120 --min-keyint 1 --ref 3 --mixed-refs --no-fast-pskip --trellis 2 --psy-rd 0:0 --partitions all  --8x8dct --threads auto --thread-input --aq-mode 0 --progress --no-dct-decimate --no-psnr --no-ssim --output " + lastFileName + " " + @"G:\Workshop\natsume2\01\test_op" + idStr + ".avs");
                }
            }
        }

        public void SaveFile(string filename)
        {
            using (StreamWriter fout = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.Unicode))
            {
                foreach (string line in Header)
                    fout.WriteLine(line);
                foreach (ASSEvent ev in Events)
                    fout.WriteLine(ev);
            }
        }

        public static ASS FromFile(string filename)
        {
            using (StreamReader fin = new StreamReader(new FileStream(filename, FileMode.Open), Encoding.Unicode))
            {
                ASS ass = new ASS();
                ass.Header = new List<string>();
                ass.Events = new List<ASSEvent>();
                bool isHeader = true;
                while (true)
                {
                    string line = fin.ReadLine();
                    if (line == null) break;
                    ASSEvent ev = ASSEvent.FromString(line);
                    if (ev == null && isHeader)
                        ass.Header.Add(line);
                    else
                    {
                        if (ev != null) ass.Events.Add(ev);
                        isHeader = false;
                    }
                }
                return ass;
            }
        }
    }
}
