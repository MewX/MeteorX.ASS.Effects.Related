using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.XXParticle
{
    interface IXXEmitter
    {
        XXParticleElement GenerateParticleElement(double time);

        double NumberPerSecond { get; }
    }
}
