using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    class Light_Source
    {
        public Couleur color;
        public V3 direction;
        public V3 position;
        public bool point_light = false;

        public Light_Source(V3 direction, Couleur color)
        {;
            this.direction = direction;
            this.direction.Normalize();
            this.color = color;
        }

        public Light_Source(V3 position, Couleur color, bool point_light)
        {
            ;
            this.position = position;
            this.color = color;
            this.point_light = true;
        }
    }
}
