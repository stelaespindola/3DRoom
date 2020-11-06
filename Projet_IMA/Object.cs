using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    abstract class Object
    {
        public Couleur color; // color of the object
        public Texture T, bump_map; // texture of the object/texture of the bump mapping
        public float k = 2f; // intesity of the bump mapping effect
        public bool bump_activate = false; // activates the projection of the bump mapping effect
        public int object_index; // object index on the list of objects
        public float refractive_index = 0;
        public float reflective_index = 0;
        public float MIN_DISTANCE = 0.001f;

        public bool is_spec = false;



        // light calculation
        abstract public void draw_pixel(float u, float v);
        abstract public int draw_pixel(V3 pixel_pos);
        abstract public Couleur getPixelColor(V3 P);
        abstract public bool intersection_RayObject(V3 R0, V3 Rd, out V3 P, out V3 N, out float t);
        // Drawing fucntions, depends on each object
        abstract public void draw();


        /* Prints the pixel of point P on the screen:
         * 
         *  receives the color of the pixel without the lights
         *  and the normal vector.
         *  
         *  if P is set outside of the screen bounds, doesn't print anything
         *  
         *  compare its position on the Z_Buffer
         *  
         *  draws the pixel with its final color (adding the lights effects)
             */
        public int printPixel(V3 P, Couleur color)
        {
            int x, y, z;

            x = (int)P.x;
            y = (int)P.y;
            z = (int)P.z;


            if (x < 0 || x >= BitmapEcran.GetWidth() || z < 0 || z >= BitmapEcran.GetHeight())
            {
                return -1;
            }
            if (Elements.raycasting) // using ray casting
            {
                BitmapEcran.DrawPixel(x, z, color);
            }
            else
            { //using Z-Buffer
                if (y < Elements.Z_Buffer[x, z])
                {
                    Elements.Z_Buffer[x, z] = y;
                    BitmapEcran.DrawPixel(x, z, color);
                }
            }
            
            return 0;
        }
        



        /* Set the Texture of the bump_map
         * 
         * If this function is called, bump_activate becomes true
         * 
         */
        public void setBump(Texture bump_map, float K)
        {
            this.bump_map = bump_map;
            this.k = K;
            bump_activate = true;
        }


        public void setSpecularity()
        {
            is_spec = true;
        }
        public void setSpecularity(float refractive_index, float reflective_index)
        {
            is_spec = true;
            this.refractive_index = refractive_index;
            this.reflective_index = reflective_index;
            
        }

        // Default value of k
        public void setBump(Texture bump_map)
        {
            this.bump_map = bump_map;
            bump_activate = true;
        }

       

        /* Returns the color of the pixel:
         * 
         *  if a texture was defined, returns the good value from the texture file
         
             */
        public Couleur getColor(float u, float v)
        {
            if (T == null) return color;
            else
            {
                float r_x = 4f;   // repetition de la tsexture en x
                float r_y = 4f;
                if(T.r_x == 0 && T.r_y == 0)
                    return T.LireCouleur(-u * r_x, -v * r_y);
                else
                    return T.LireCouleur(-u * T.r_x, -v * T.r_y);

            }

        }

        
        // Get the partial derivate for the bump mapping effect, each object has its own formula
        abstract public void partialM(float u, float v, out V3 dMdu, out V3 dMdv);

        // Returns the new normal vector according to the bump map
        public V3 bumpN(V3 N, float u, float v)
        {
            V3 N_, dM_du, dM_dv;
            V3 dMdu, dMdv;

            float dhdu = 0, dhdv = 0;

            bump_map.Bump(u, v, out dhdu, out dhdv);

            partialM(u, v, out dMdu, out dMdv);

            //M_ = M'
            dM_du = dMdu + k * dhdu * N;

            dM_dv = dMdv + k * dhdv * N;

            N_ = dM_du ^ dM_dv;
            N_.Normalize();
            return N_;
        }
    }
}
