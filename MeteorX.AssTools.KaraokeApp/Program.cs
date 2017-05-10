using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteorX.AssTools.KaraokeApp.Anime;
using MeteorX.AssTools.KaraokeApp.Effect;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Anime.Test;
using MeteorX.AssTools.KaraokeApp.Anime.SakiDVDRip;

namespace MeteorX.AssTools.KaraokeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            BaseAnime a = new Toradora_OP2();
            a.Run();

            Console.WriteLine("~~~");
            Console.ReadLine();
        }
    }
}
