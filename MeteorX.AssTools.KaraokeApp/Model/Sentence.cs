using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    public class Sentence
    {
        public ASSEvent BaseEvent { get; set; }

        public double Start { get; set; }

        public double End { get; set; }

        public double X7 { get; set; }

        public double Y7 { get; set; }

        List<Word> Words { get; set; }

        public int SentenceId { get; set; }

        public string OutlintString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (Word wd in Words) sb.Append(wd.OutlineString);
                return sb.ToString();
            }
        }

        public bool IsJp { get; set; }
    }
}
