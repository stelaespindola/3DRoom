using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    
    static class Lights
    {
        public static List<Light_Source> lights = new List<Light_Source>();
        public static float MIN_DISTANCE = 0.001f;
        static Couleur Black = new Couleur(0, 0, 0);
        public static void add_light(V3 direction, Couleur c)
        {
            Light_Source l = new Light_Source(direction, c);
            lights.Add(l);
        }

        public static void add_light(V3 position, Couleur c, bool point)
        {
            Light_Source l = new Light_Source(position, c, true);
            lights.Add(l);
        }

        /* 
         * getPixelColor adds all lights and "shadows" at each point P of the image
         */
        public static Couleur getPixelColor(V3 P, Couleur c, V3 N, int object_index)
        {
            Couleur pixel_color = c;
            float decay = 1;
            pixel_color = Color_Ambient(c);
            
            foreach (Light_Source light in lights)
            {
                if (light.point_light)
                {
                    Elements.objects_on_screen[object_index].intersection_RayObject(light.position, P - light.position,out V3 P1, out V3 N1, out float t);
                    decay = (float)Math.PI / (float)(Math.Pow(t, 2));
                    if (decay > 1) decay = 1;
                }
                pixel_color += decay*Color_Diffused(light, c, P, N);
                decay = 1;
                if (Elements.objects_on_screen[object_index].is_spec)
                { // if the light creates a specular effect
                    pixel_color += Color_Specular(light, P, N, 200);
                }
                
            }
            pixel_color.check();

            return pixel_color;
        }

        public static float getShadow(V3 P, Light_Source light)
        {
            float t, t_max = 200;
            V3 direction = -light.direction;
            if (light.point_light)
            {
                direction = light.position - P;
                direction.Normalize();
            }
                
            foreach (Object obj in Elements.objects_on_screen)
            {
                
                obj.intersection_RayObject(P, direction, out V3 P1, out V3 N1, out t);
                if (t > MIN_DISTANCE && t < t_max)
                {
                    if (-direction * N1 > 0.1f)
                        return 0.3f;
                }
            }
            return 1;
        }

        public static Couleur Color_Ambient(Couleur px_color)
        {
            Couleur ambient_light_color = new Couleur(0.1f, 0.1f, 0.1f);
            return (ambient_light_color * px_color);
        }

        public static Couleur Color_Diffused(Light_Source light, Couleur px_color, V3 P, V3 N)
        {
            V3 L = -light.direction;
            if (light.point_light)
            {
                L = light.position-P;
                //L = P-light.position;
                L.Normalize();
            }
            float cos_theta = N * L;

           
            if (cos_theta < 0) cos_theta = 0;
            
            
            
            float on_shadow = getShadow(P, light);
            Couleur c = cos_theta * (light.color * px_color);
            return c*on_shadow;
             
        }

        public static Couleur Color_Specular(Light_Source light, V3 P, V3 N, int k)
        {

            V3 L = -light.direction;
            if (light.point_light)
            {
                L = light.position - P;
                L.Normalize();
            }
            float cos_alpha = (N * L);
            if (light.point_light && cos_alpha < 0)
                cos_alpha = -cos_alpha;

            V3 R = (N* 2*cos_alpha) - L;
            R.Normalize();

            V3 D = Elements.camera_position-P;

            D.Normalize();

            float f = (R*D);

            if (f < 0) f = 0; // if cos is negative there won't be any specularity
            Couleur c = (float)Math.Pow(f, k) * (light.color);
            return c;

        }

        
        

    }
}
