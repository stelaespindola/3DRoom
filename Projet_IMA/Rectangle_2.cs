using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    class Rectangle_2:Object
    {
        private V3 A, B, C;

        public Rectangle_2(V3 a, V3 b, V3 c, Couleur color, int index) {
            A = a;
            B = b;
            C = c;
            this.color = color;
            object_index = index;
        }
        public Rectangle_2(V3 a, V3 b, V3 c, Texture T1, int index)
        {
            A = a;
            B = b;
            C = c;
            this.T = T1;
            object_index = index;
        }

        /* partialM(float u, float v, out V3 dMdu, out V3 dMdv)
         * 
         * Returns the partial derivate of the point R in the rectangle
         */
        override
        public void partialM(float u, float v, out V3 dMdu, out V3 dMdv)
        {
            dMdu = B-A;
            dMdv = C-A; 
        }

        public void getUV(V3 P, out float u, out float v)
        {
            V3 AB = B - A;
            V3 AC = C - A;

            V3 AP = P - A;
            //alfa
            u = V3.prod_scal(ref AP, ref AB) / AB.Norme2();
            //beta
            V3 aux = AP - u * AB;
            v = V3.prod_scal(ref aux, ref AC) / AC.Norme2();

        }
        public V3 getRectangleNormal(V3 P)
        {
            V3 N = new V3(0,0,0);
            V3 AB = B - A;
            V3 AC = C - A;

            getUV(P, out float u, out float v);
            if (u < 0 || u > 1 || v < 0 || v > 1) return N;
            // get the normal vector
            N = AB * u ^ AC * v;
            N.Normalize();
            // calculate the new normal vector for the bump mapping effect
            if (bump_activate)
                N = bumpN(N, u, v);

            return N;

        }

        /* Calculates the point P of the intersection between the ray of light and the rectangle
         * if there is no intersection returns false */
        override
        public bool intersection_RayObject(V3 R0, V3 Rd, out V3 P, out V3 N, out float t)
        {
            V3 R0A;
            P = new V3(0, 0, 0);
            float  aux1;

            V3 AB = B - A;
            V3 AC = C - A;

            N = AB ^ AC;
            N.Normalize();

            R0A = A - R0;

            t = V3.prod_scal(ref R0A, ref N);
            aux1 = V3.prod_scal(ref Rd, ref N);

            if (aux1 == 0) {
                t = -1;
                return false;
            }
            
            t /= aux1;

            if (t < MIN_DISTANCE) {
                return false;
            }
            
            P = R0 + t * Rd;
                
            getUV(P, out float u, out float v);
            if (u < 0 || u > 1 || v < 0 || v > 1) {
                P = new V3(0, 0, 0);
                t = -1;
                return false;
            }
            return true;

        }

        // RayCasting for drawing
        override

        public Couleur getPixelColor(V3 P)
        {
            V3 N = getRectangleNormal(P);
            // get pixel's color
            getUV(P, out float u, out float v);
            
            Couleur c = getColor(u, v);
            if (bump_activate)
                N = bumpN(N, u, v);
            c = Lights.getPixelColor(P, c, N, object_index);
            return c;
        }

        // Raycasting plotting
        override
        public int draw_pixel(V3 pixel_pos)
        {
            Couleur c;
            
            c = getPixelColor(pixel_pos);
            // print
            printPixel(pixel_pos, c);
            return 0;

        }

        // Original plotting
        override
        public void draw_pixel(float u, float v)
        {
            V3 N, R;
            Couleur c;
            V3 AB = B - A;
            V3 AC = C - A;

            // get the normal vector
            N = AB * u ^ AC * v;
            // get the position of the 3D point
            R = A + u * AB + v * AC;
            // calculate the new normal vector for the bump mapping effect
            if (bump_activate)
                N = bumpN(N, u, v);
            // get pixel's color
            c = getColor(u, v);
            // print
            printPixel(R, c);
        }

        /* draw()
         *  draw the ractangle
         */
        override
        public void draw()
        {
            float step = 0.001f; // distance between pixels

            for (float u = 0; u <= 1; u += step)
            {
                for (float v = 0; v <= 1; v += step)
                {
                    draw_pixel(u, v);
                }

            }
        }
    }
}
