using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MeteorX.AssTools.KaraokeApp
{
    public class ASSEvent
    {
        public int Layer { get; set; }
        public double Start { get; set; }
        public double End { get; set; }
        public string Style { get; set; }
        public string Name { get; set; }
        public string MarginL { get; set; }
        public string MarginR { get; set; }
        public string MarginV { get; set; }
        public string Effect { get; set; }
        public string Text { get; set; }

        public double Last { get { return End - Start; } }

        public override string ToString()
        {
            return string.Format("Dialogue: {0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                Layer, Common.ToTimeString(Start), Common.ToTimeString(End), Style, Name, MarginL, MarginR, MarginV,
                Effect, Text);
        }

        public ASSEvent Last_ChangeEnd(double last)
        {
            ASSEvent result = this.MemberwiseClone() as ASSEvent;
            result.End = result.Start + last;
            return result;
        }

        public ASSEvent StyleReplace(string style)
        {
            ASSEvent result = this.MemberwiseClone() as ASSEvent;
            result.Style = style;
            return result;
        }

        public ASSEvent LayerReplace(int layer)
        {
            ASSEvent result = this.MemberwiseClone() as ASSEvent;
            result.Layer = layer;
            return result;
        }

        public ASSEvent TextReplace(string text)
        {
            ASSEvent result = this.MemberwiseClone() as ASSEvent;
            result.Text = text;
            return result;
        }

        public ASSEvent StartReplace(double start)
        {
            ASSEvent result = this.MemberwiseClone() as ASSEvent;
            result.Start = start;
            return result;
        }

        public ASSEvent EndReplace(double end)
        {
            ASSEvent result = this.MemberwiseClone() as ASSEvent;
            result.End = end;
            return result;
        }

        public ASSEvent StartOffset(double offset)
        {
            ASSEvent result = this.MemberwiseClone() as ASSEvent;
            result.Start += offset;
            return result;
        }

        public ASSEvent EndOffset(double offset)
        {
            ASSEvent result = this.MemberwiseClone() as ASSEvent;
            result.End += offset;
            return result;
        }

        public List<KElement> SplitK(bool splitWord)
        {
            string text = this.Text;

            if (text.Trim().Length == 0) return new List<KElement>();
            
            /// 如果没有\K的话，均分之
            if (text.IndexOf(@"\K") < 0 && text.IndexOf(@"\k") < 0)
            {
                if (splitWord)
                {
                    text = text.Trim();
                    List<KElement> result = new List<KElement>();
                    foreach (char ch in text)
                        result.Add(new KElement { KText = ch.ToString(), KValue = (int)((double)(this.End - this.Start) * 100.0 / (double)text.Length), IsSplit = true });
                    return result;
                }
                else
                {
                    text = text.Trim();
                    List<KElement> result = new List<KElement>();
                    result.Add(new KElement { KText = text, KValue = (int)((double)(this.End - this.Start) * 100.0), IsSplit = false });
                    return result;
                }
            }

            /// 解析所有的\K，并抛弃其它所有标记
            List<int> kValues = new List<int>();
            List<string> textValues = new List<string>();
            bool inBra = false;
            int k = 0;
            string t = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (!inBra && text[i] != '{') t = t + text[i];
                if (text[i] == '{') inBra = true;
                if (text[i] == '}') inBra = false;
                if (text[i] == '\\' && inBra)
                {
                    if (text[i + 1] == 'K' || text[i + 1] == 'k')
                    {
                        if (t != "")
                        {
                            kValues.Add(k);
                            textValues.Add(t);
                        }
                        k = 0;
                        t = "";
                        int j = i + 2;
                        while (char.IsDigit(text[j]))
                        {
                            k = k * 10 + Convert.ToInt32(text[j].ToString());
                            j++;
                        }
                    }
                }
            }
            if (t != "")
            {
                kValues.Add(k);
                textValues.Add(t);
            }

            /// 切割单字
            List<int> newks = new List<int>();
            List<string> newtexts = new List<string>();
            List<bool> issplit = new List<bool>();
            List<bool> iskstart = new List<bool>();
            List<bool> iskend = new List<bool>();
            if (!splitWord)
            {
                newks = kValues;
                newtexts = textValues;
                issplit = new List<bool>();
                foreach (int tmp in newks) issplit.Add(false);
            }
            else
            {
                for (int i = 0; i < kValues.Count; i++)
                {
                    if (textValues[i].Length == 1)
                    {
                        newks.Add(kValues[i]);
                        newtexts.Add(textValues[i]);
                        issplit.Add(false);
                        iskstart.Add(true);
                        iskend.Add(true);
                        continue;
                    }
                    int k1 = (int)(kValues[i] / textValues[i].Length);
                    for (int j = 0; j + 1 < textValues[i].Length; j++)
                    {
                        newks.Add(k1);
                        newtexts.Add(textValues[i][j].ToString());
                        issplit.Add(true);
                        iskstart.Add(j == 0);
                        iskend.Add(false);
                    }
                    newks.Add(kValues[i] - k1 * (textValues[i].Length - 1));
                    newtexts.Add(textValues[i][textValues[i].Length - 1].ToString());
                    issplit.Add(true);
                    iskstart.Add(false);
                    iskend.Add(true);
                }
            }

            List<KElement> result1 = new List<KElement>();
            for (int i = 0; i < newks.Count; i++)
                result1.Add(new KElement { KText = newtexts[i], KValue = newks[i], IsSplit = issplit[i] });

            if (splitWord)
            {
                result1[0].KStart_NoSplit = this.Start;
                double kSum = result1[0].KValue;
                double lastStart = this.Start;
                for (int i = 1; i < result1.Count; i++)
                {
                    if (iskstart[i] || result1[i].IsSplit != result1[i - 1].IsSplit || (Char.IsWhiteSpace(result1[i].KText[0]) && result1[i].KText.Length == 1))
                        result1[i].KStart_NoSplit = lastStart = kSum * 0.01 + this.Start;
                    else
                        result1[i].KStart_NoSplit = lastStart;
                    kSum += result1[i].KValue;
                }
                double lastEnd = result1[result1.Count - 1].KEnd_NoSplit = this.Start + kSum * 0.01;
                kSum -= result1[result1.Count - 1].KValue;
                for (int i = result1.Count - 2; i >= 0; i--)
                {
                    if (iskend[i] || result1[i + 1].IsSplit != result1[i].IsSplit || (Char.IsWhiteSpace(result1[i].KText[0]) && result1[i].KText.Length == 1))
                        result1[i].KEnd_NoSplit = lastEnd = kSum * 0.01 + this.Start;
                    else
                        result1[i].KEnd_NoSplit = lastEnd;
                    kSum -= result1[i].KValue;
                }
            }

            return result1;
        }

        public List<KElement> SplitK()
        {
            return SplitK(true);
        }

        public static ASSEvent FromString(string src)
        {
            Regex regex = new Regex(@"Dialogue:\s*(?<Layer>\d+),(?<Start>[:\.\d]+),(?<End>[:\.\d]+),(?<Style>[\*\w]+),(?<Name>\w+),(?<MarginL>\w+),(?<MarginR>\w+),(?<MarginV>\w+),(?<Effect>\w*),(?<Text>.*)\s*");
            Match match = regex.Match(src);
            if (!match.Success) return null;
            return new ASSEvent
            {
                Layer = Convert.ToInt32(match.Result("${Layer}")),
                Start = Common.ToTime(match.Result("${Start}")),
                End = Common.ToTime(match.Result("${End}")),
                Style = match.Result("${Style}"),
                Name = match.Result("${Name}"),
                MarginL = match.Result("${MarginL}"),
                MarginR = match.Result("${MarginR}"),
                MarginV = match.Result("${MarginV}"),
                Effect = match.Result("${Effect}"),
                Text = match.Result("${Text}"),
            };
        }
    }
}
