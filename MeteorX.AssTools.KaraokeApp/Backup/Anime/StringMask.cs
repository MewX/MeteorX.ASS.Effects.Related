using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    [Serializable]
    public class StringMask
    {
        public List<ASSPoint> Points { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int X0 { get; set; }

        public int Y0 { get; set; }



        int[,] map, edge;

        public Size GetSize()
        {
            return new Size { Width = this.Width, Height = this.Height };
        }

        void CalculateEdgeDistance_DFS(int x, int y)
        {
            if (edge[x, y] == 0) return;
            edge[x, y] = 0;
            if (map[x, y] != -1) return;
            if (x > 0) CalculateEdgeDistance_DFS(x - 1, y);
            if (y > 0) CalculateEdgeDistance_DFS(x, y - 1);
            if (x + 1 < map.GetLength(0)) CalculateEdgeDistance_DFS(x + 1, y);
            if (y + 1 < map.GetLength(1)) CalculateEdgeDistance_DFS(x, y + 1);
        }

        public void CalculateEdgeDistance()
        {
            StringMask mask = this;
            if (mask.Points.Count == 0) return;

            int mask_minx = 100000;
            int mask_miny = 100000;
            int mask_maxx = -100000;
            int mask_maxy = -100000;

            foreach (ASSPoint pt in mask.Points)
            {
                if (mask_minx > pt.X) mask_minx = pt.X;
                if (mask_miny > pt.Y) mask_miny = pt.Y;
                if (mask_maxx < pt.X) mask_maxx = pt.X;
                if (mask_maxy < pt.Y) mask_maxy = pt.Y;
            }

            foreach (ASSPoint pt in mask.Points)
            {
                pt.X -= mask_minx - 1;
                pt.Y -= mask_miny - 1;
                pt.EdgeDistance = -1;
            }
            map = new int[mask_maxx - mask_minx + 2, mask_maxy - mask_miny + 2];
            edge = new int[mask_maxx - mask_minx + 2, mask_maxy - mask_miny + 2];
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = -1;
                    edge[i, j] = -1;
                }
            for (int i = 0; i < mask.Points.Count; i++)
            {
                ASSPoint pt = mask.Points[i];
                map[pt.X, pt.Y] = i;
                edge[pt.X, pt.Y] = -2;
            }

            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (edge[i, j] == -1 && map[i, j] == -1)
                        CalculateEdgeDistance_DFS(i, j);

            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (map[i, j] != -1 && edge[i, j] == 0)
                        mask.Points[map[i, j]].EdgeDistance = 0;

            foreach (ASSPoint pt in mask.Points)
            {
                pt.X += mask_minx - 1;
                pt.Y += mask_miny - 1;
            }

            for (int i = 0; i < mask.Points.Count; i++)
            {
                ASSPoint pt = mask.Points[i];
                if (pt.EdgeDistance == 0) continue;
                pt.EdgeDistance = 1e8;
                for (int j = 0; j < mask.Points.Count; j++)
                {
                    ASSPoint pt2 = mask.Points[j];
                    if (pt2.EdgeDistance != 0) continue;
                    double dis = Common.GetDistance(pt.X, pt.Y, pt2.X, pt2.Y);
                    if (pt.EdgeDistance > dis) pt.EdgeDistance = dis;
                }
            }

            map = edge = null;
        }

    }

}
