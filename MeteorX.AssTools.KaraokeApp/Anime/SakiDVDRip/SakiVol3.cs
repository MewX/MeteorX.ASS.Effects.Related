using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteorX.AssTools.KaraokeApp.Anime.SakiDVDRip
{
    class SakiVol3 : BaseAnime
    {
        public string TcFilename = @"G:\Workshop\saki\DVDRIP\Vol.4\10\10.tc.ass";
        public string ScFilename = @"G:\Workshop\saki\DVDRIP\Vol.4\10\10.sc.ass";

        public override void Run()
        {
            //PreConvert(ScFilename);
            //PreConvert(TcFilename);
            // 将ass中所有 {\fs24\an8} 置换为 Style:an8
            // "PopSub注释..." 去掉
            // 非 an8 的 Dialog，去除所有逗号和句号
            //return;
            SyncTime sync = new SyncTime();
            sync.Filename1 = ScFilename;
            sync.Filename2 = TcFilename;
            sync.Run();
        }

        void PreConvert(string infile)
        {
            ASS ass1 = ASS.FromFile(infile);
            FileInfo tcfi = new FileInfo(infile);
            string inbakname = infile + ".bak";
            if (File.Exists(inbakname)) File.Delete(inbakname);
            tcfi.CopyTo(inbakname);

            ASS ass2 = ASS.FromFile(infile);
            ass2.Events.Clear();
            for (int i = 0; i < ass1.Events.Count; i++)
            {
                if (ass1.Events[i].Text.IndexOf("PopSub注释") >= 0) continue;
                if (ass1.Events[i].Text.IndexOf("PopSub注釋") >= 0) continue;

                if (ass1.Events[i].Text.IndexOf(@"{\fs24\an8}") == 0)
                {
                    ass1.Events[i].Text = ass1.Events[i].Text.Replace(@"{\fs24\an8}", "");
                    ass1.Events[i].Style = "an8";
                }

                if (ass1.Events[i].Style.Contains("Default"))
                {
                    ass1.Events[i].Text = ass1.Events[i].Text.Replace("，", " ").Replace("。", " ");
                }
                ass2.Events.Add(ass1.Events[i]);
            }
            ass2.SaveFile(infile);
        }
    }
}
