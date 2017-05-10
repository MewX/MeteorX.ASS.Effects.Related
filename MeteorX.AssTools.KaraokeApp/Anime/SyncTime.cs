using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class SyncTime : BaseAnime
    {
        public string Filename1 = @"G:\Workshop\kiddy girl and\11\11.chs.ass"; // source, chs
        public string Filename2 = @"G:\Workshop\kiddy girl and\11\11.cht.ass"; // dest, cht

        public override void Run()
        {
            FileInfo fi2 = new FileInfo(Filename2);
            //fi2.CopyTo(Filename2 + ".bak");
            ASS ass1 = ASS.FromFile(Filename1);
            ASS ass2 = ASS.FromFile(Filename2);
            for (int i = 0; i < ass1.Events.Count && i < ass2.Events.Count; i++)
            {
                ass2.Events[i].Start = ass1.Events[i].Start;
                ass2.Events[i].End = ass1.Events[i].End;

                continue;
                if (ass1.Events[i].Text.Trim() != ToSimplified(ass2.Events[i].Text.Trim()))
                {
                    Console.WriteLine("----------------Warning----------------");
                    Console.WriteLine(ass1.Events[i].ToString());
                    Console.WriteLine(ass2.Events[i].ToString());
                }
            }
            ass2.SaveFile(Filename2);
        }

        internal const int LOCALE_SYSTEM_DEFAULT = 0x0800;
        internal const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        internal const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

        public static string ToSimplified(string pSource)
        {
            String tTarget = new String(' ', pSource.Length);
            int tReturn = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_SIMPLIFIED_CHINESE, pSource, pSource.Length, tTarget, pSource.Length);
            return tTarget;
        }

        public static string ToTraditional(string pSource)
        {
            String tTarget = new String(' ', pSource.Length);
            int tReturn = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_TRADITIONAL_CHINESE, pSource, pSource.Length, tTarget, pSource.Length);
            return tTarget;
        }
    }
}
