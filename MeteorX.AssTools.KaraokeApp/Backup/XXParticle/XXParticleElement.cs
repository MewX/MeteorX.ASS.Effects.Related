using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.XXParticle
{
    class XXParticleElement
    {
        public ASSPointF Position { get; set; }

        public ASSPointF Speed { get; set; }

        public double Born { get; set; }

        public double Life { get; set; }

        public double Mass { get; set; }

        public double Spin { get; set; }

        public double ForceTimeOffset { get; set; }

        public XXParticleElement()
        {
            Mass = 1;
            Spin = 0;
            ForceTimeOffset = 0;
        }
    }
}
