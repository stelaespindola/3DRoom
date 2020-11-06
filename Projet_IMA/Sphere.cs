using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    class Sphere : Object
    {
        private float R;
        private V3 Center;
        
        public Sphere(float R, V3 center, Couleur c, int index)
        {
            this.R = R;
            Center = center;
            color = c;
            object_index = index;
        }
        public Sphere(float R, V3 center, Texture T1, int index)
        {
            this.R = R;
            Center = center;
            T = T1;
            object_index = index;
        }

        /* getPixelSphereNormal(float u, float v)
         * Returns the normal vector of the sphere pixel
         * 
         */

        public V3 getPixelSphereNormal(float u, float v)
        {
            V3 N;
            N.x = (float)(Math.Cos(v) * Math.Cos(u));
            N.y = (float)(Math.Cos(v) * Math.Sin(u));
            N.z = (float)(Math.Sin(v));
            // calculate the new normal vector for the bump mapping effect
            if (bump_activate)
                N = bumpN(N, u, v);
            return N;
        }
        public V3 getPixelSphereNormal(V3 P)
        {
            V3 N = P - Center;

            IMA.Invert_Coord_Spherique(N, R, out float u, out float v);
            N.Normalize();
            // calculate the new normal vector for the bump mapping effect
            if (bump_activate)
                N = bumpN(N, u, v);
            return N;
        }

        /* getPixelColorSphere(V3 P)
         * Returns the color of the pixel when there is a texture te bu projected
         */
        override
        public Couleur getPixelColor(V3 P) {
            float u, v;
            V3 d = new V3(Center-P);
            d.Normalize();

            u = (float)(0.5f + Math.Atan2(d.x,d.y) / (2*Math.PI));
            v = (float)(0.5f - Math.Asin(d.z) / Math.PI);

            return Lights.getPixelColor(P, getColor(u, v), getPixelSphereNormal(P), object_index);

        }

        /* partialM(float u, float v, out V3 dMdu, out V3 dMdv)
         * 
         * Returns the partial derivate of the point M in the sphere
         */
        override
        public void partialM(float u, float v, out V3 dMdu, out V3 dMdv)
        {
            dMdu.x = (float)(Math.Cos(v) * (-1) * Math.Sin(u)) * R;
            dMdu.y = (float)(Math.Cos(v) * Math.Cos(u)) * R;
            dMdu.z = 0;

            dMdv.x = (float)((-1) * Math.Sin(v) * Math.Cos(u)) * R;
            dMdv.y = (float)((-1) * Math.Sin(v) * Math.Sin(u)) * R;
            dMdv.z = (float)(Math.Cos(v)) * R;
        }

        override
        public bool intersection_RayObject(V3 R0, V3 Rd, out V3 P, out V3 N, out float t)
        {
            V3 CR0;
            float A, B, C, delta, t1, t2;
            P = new V3(0, 0, 0);
            N = new V3(0, 0, 0);
            t = -1;

            CR0 = R0 - Center;

            // (t^2) rd^2 + 2*t*rd*b +b^2 = R^2
            A = Rd.Norme2();
            B = 2f * V3.prod_scal(ref Rd, ref CR0);
            C = CR0.Norme2() -(R * R);

            delta = (B * B) - (4 * A * C);
            if (delta < 0) return false;

            t1 = (-B + (float)Math.Sqrt(delta)) / (2 * A);
            t2 = (-B - (float)Math.Sqrt(delta)) / (2 * A);
            t = (t1 > 0 && t1 < t2) ? t1 : t2;
            if (t < MIN_DISTANCE) return false;

            P = R0 + t * Rd;
            N = getPixelSphereNormal(P);
            

            return true;

        }
     
        /* draw()
         * Draw the sphere
         */
        override
        public int draw_pixel(V3 pixel_pos)
        {
            V3 N;
            Couleur c;
            // get the normal vector
            N = getPixelSphereNormal(pixel_pos);
            // get pixel's color
            c = getPixelColor(pixel_pos);
            // print
            printPixel(pixel_pos, c);
            return 0;
        }


        override
        public void draw_pixel(float u, float v)
        {
            V3 N, M;
            Couleur c;
            // get the normal vector
            N = getPixelSphereNormal(u, v);

            // get the position of the 3D point
            M = R * N + Center;

            // get pixel's color
            c = getPixelColor(M);
            // calculate the new normal vector for the bump mapping effect
            if (bump_activate)
                N = bumpN(N, u, v);
            // print
            printPixel(M, c);
        }

        override
        public void draw()
        {
            float step = 0.007f; // distance between pixels

            for (float u = 0; u <= 2 * Math.PI; u += step)
            {
                for (float v = (float)(-Math.PI / 2); v < Math.PI / 2; v += step)
                {
                    draw_pixel(u, v);

                }

            }
        }
    }
}
