using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.XXParticle
{
    class XXParticleSystem
    {
        public IXXEmitter Emitter { get; set; }

        public IXXForceField ForceField { get; set; }

        public double StartTime { get; set; }

        public double EndTime { get; set; }

        public double InterpolationPrecision { get; set; }

        public double Resistance { get; set; }

        public double Repulsion { get; set; }

        public double Gravity { get; set; }

        public IXXGravityPosition GravityPosition { get; set; }

        public XXParticleSystem()
        {
            InterpolationPrecision = 0.01;
            Resistance = 0;
            Repulsion = 0;
            Gravity = 0;
        }

        Random rnd = new Random();

        /// <summary>
        /// \t, X, Y. 默认pos(-100, -100)
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<XXParticleElement, List<string>>> RenderT()
        {
            List<KeyValuePair<XXParticleElement, List<string>>> result = new List<KeyValuePair<XXParticleElement, List<string>>>();
            foreach (KeyValuePair<XXParticleElement, List<ASSPointF>> pair in RenderPoint())
            {
                foreach (ASSPointF pt in pair.Value)
                {
                    pt.X += 100;
                    pt.Y += 100;
                }
                StringBuilder sbx = new StringBuilder();
                StringBuilder sby = new StringBuilder();
                for (int i = 0; i < pair.Value.Count; i += 10)
                {
                    List<ASSPointF> ptList = new List<ASSPointF>();
                    for (int j = i; j < i + 10 && j < pair.Value.Count; j++)
                        ptList.Add(pair.Value[j]);
                    if (ptList.Count > 0 && i == 0)
                    {
                        sbx.Append(ASSEffect.fscx((int)(ptList[0].X * 100)));
                        sby.Append(ASSEffect.fscy((int)(ptList[0].Y * 100)));
                    }
                    sbx.Append(ASSEffect.t(ptList[0].T - pair.Key.Born, ptList[ptList.Count - 1].T - pair.Key.Born + InterpolationPrecision, ASSEffect.fscx((int)(ptList[ptList.Count - 1].X * 100)).t()));
                    sby.Append(ASSEffect.t(ptList[0].T - pair.Key.Born, ptList[ptList.Count - 1].T - pair.Key.Born + InterpolationPrecision, ASSEffect.fscy((int)(ptList[ptList.Count - 1].Y * 100)).t()));
                }
                List<string> tmp = new List<string>();
                tmp.Add(sbx.ToString());
                tmp.Add(sby.ToString());
                result.Add(new KeyValuePair<XXParticleElement, List<string>>(pair.Key, tmp));
            }
            return result;
        }

        /// <summary>
        /// 以InterpolationPrecision为时间间隔, 绘制每一个粒子的位置
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public List<KeyValuePair<XXParticleElement, List<ASSPointF>>> RenderPoint()
        {
            Dictionary<XXParticleElement, List<ASSPointF>> parDic = new Dictionary<XXParticleElement, List<ASSPointF>>();
            Queue<XXParticleElement> bornList = new Queue<XXParticleElement>();
            for (double born = StartTime; born <= EndTime; born += 1.0 / Emitter.NumberPerSecond)
            {
                XXParticleElement par = Emitter.GenerateParticleElement(born);
                if (par == null) continue;
                par.Born = born;
                bornList.Enqueue(par);
            }
            List<XXParticleElement> liveList = new List<XXParticleElement>();
            for (double time = StartTime; liveList.Count > 0 || bornList.Count > 0; time += InterpolationPrecision)
            {
                while (bornList.Count > 0 && bornList.Peek().Born <= time)
                {
                    XXParticleElement newPar = bornList.Dequeue();
                    liveList.Add(newPar);
                    parDic[newPar] = new List<ASSPointF>();
                }
                liveList = liveList.Where(x => x.Born + x.Life > time).ToList();
                List<ASSPointF> newPosition = new List<ASSPointF>();
                List<ASSPointF> newSpeed = new List<ASSPointF>();
                ASSPointF gravPos = null;
                if (GravityPosition != null) gravPos = GravityPosition.GetGravityPosition(time);
                foreach (XXParticleElement par in liveList)
                {
                    double intense = 0;
                    ASSPointF force = ForceField.GetForceField(time + par.ForceTimeOffset);
                    if (force == null) force = new ASSPointF { X = 0, Y = 0 };
                    ASSPointF rforce = new ASSPointF { X = Common.Sqr(par.Speed.X) * Resistance, Y = Common.Sqr(par.Speed.Y) * Resistance };
                    if (par.Speed.X > 0) rforce.X = -rforce.X;
                    if (par.Speed.Y > 0) rforce.Y = -rforce.Y;
                    force.X += rforce.X;
                    force.Y += rforce.Y;
                    if (Gravity != 0)
                    {
                        double r = Common.GetDistance(par.Position.X, par.Position.Y, gravPos.X, gravPos.Y);
                        r = 1;
                        //if (Math.Abs(r) > 2)
                        {
                            double gravr = par.Mass / Common.Sqr(r) * Gravity;
                            //double gravr = par.Mass * r * Gravity / 100;
                            double gravag = -Common.GetAngle(par.Position.X, par.Position.Y, gravPos.X, gravPos.Y);
                            double gravx = gravr * Math.Cos(gravag);
                            double gravy = gravr * Math.Sin(gravag);
                            force.X -= gravx;
                            force.Y -= gravy;
                        }
                    }
                    if (Repulsion != 0)
                    {
                        foreach (XXParticleElement par2 in liveList)
                        {
                            if (par != par2)
                            {
                                double r = Common.GetDistance(par.Position.X, par.Position.Y, par2.Position.X, par2.Position.Y);
                                if (r < 4)
                                {
                                    if (r < 1) r = 1;
                                    intense += 1;
                                }
                                if (Math.Abs(r) < 2)
                                {
                                    continue;
                                }
                                double repr = par.Mass * par2.Mass / Common.Sqr(r) * Repulsion;
                                double repag = -Common.GetAngle(par.Position.X, par.Position.Y, par2.Position.X, par2.Position.Y);
                                double repx = repr * Math.Cos(repag);
                                double repy = repr * Math.Sin(repag);
                                force.X += repx;
                                force.Y += repy;
                            }
                        }
                    }
                    double vx1 = force.X * InterpolationPrecision / par.Mass + par.Speed.X;
                    double vy1 = force.Y * InterpolationPrecision / par.Mass + par.Speed.Y;
                    double sx = par.Speed.X * InterpolationPrecision;
                    double sy = par.Speed.Y * InterpolationPrecision;
                    newPosition.Add(new ASSPointF { X = par.Position.X + sx, Y = par.Position.Y + sy });
                    double vag1 = -Common.GetAngle(vx1, vy1, 0, 0);
                    double vr1 = Common.GetDistance(vx1, vy1, 0, 0);
                    vag1 += par.Spin * InterpolationPrecision;
                    vx1 = vr1 * Math.Cos(vag1);
                    vy1 = vr1 * Math.Sin(vag1);
                    newSpeed.Add(new ASSPointF { X = vx1, Y = vy1 });
                    parDic[par].Add(new ASSPointF { X = par.Position.X, Y = par.Position.Y, T = time, Intense = intense });
                }
                for (int i = 0; i < liveList.Count; i++)
                {
                    liveList[i].Position = newPosition[i];
                    liveList[i].Speed = newSpeed[i];
                }
            }
            return parDic.ToList();
        }
    }
}
