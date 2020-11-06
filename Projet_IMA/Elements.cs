using System;
using System.Collections.Generic;

namespace Projet_IMA
{
    static class Elements
    {
        public static float[,] Z_Buffer;
        public static bool raycasting = false;
        public static V3 camera_position;
        public static List<Object> objects_on_screen;
        // Ray tracer
        static int MAX_RAY_DEPTH = 20;
        static Couleur Black = new Couleur(0, 0, 0);
        static public void inicialize_Elements(V3 camera_position, bool raycasting, List<Object> objects_on_screen)
        {
            
            Elements.raycasting = raycasting;
            Elements.camera_position = camera_position;
            Elements.objects_on_screen = objects_on_screen;
            if (!raycasting)
            {
                int x_ecran = BitmapEcran.GetWidth();
                int z_ecran = BitmapEcran.GetHeight();
                Z_Buffer = new float[x_ecran, z_ecran];
                //Initializer le Z_Buffer
                for (int i = 0; i < x_ecran; i++)
                {
                    for (int j = 0; j < z_ecran; j++)
                    {
                        Z_Buffer[i, j] = int.MaxValue;
                    }
                }
            }
            

        }

        static public void RayCasting(V3 pixel_screen_pos, List<Object> objects_on_screen)
        {
            float t, t_min = float.MaxValue, y_min =0;
            V3 ray_direction = pixel_screen_pos-camera_position;
            ray_direction.Normalize();
            int i_min = 0;
            for (int i=0; i<objects_on_screen.Count; i++)
            {
                objects_on_screen[i].intersection_RayObject(camera_position, ray_direction, out V3 P, out V3 N, out t);
                if(t < t_min && t > 0)
                {
                    t_min = t;
                    y_min = P.y;
                    i_min = i; 
                }
            }
            pixel_screen_pos.y = y_min;
            if(t_min < float.MaxValue) objects_on_screen[i_min].draw_pixel(pixel_screen_pos);

        }

        private static V3 computeReflectionRay(V3 ray_direction, V3 N)
        {
            V3 reflectionRay;
            reflectionRay = ray_direction - 2 * N * (ray_direction * N);
            reflectionRay.Normalize();
            return reflectionRay;
        }

        public static V3 computeRefractionRay(float refractive_index, V3 ray_direction, V3 N)
        {
            V3 Nrefr = N;
            float cos_theta1 = N * ray_direction;

            float n_aux, n1 = 1, n2 = refractive_index; // n1 is the index of refraction of the medium and n2 is in before entering the second medium 
            if (cos_theta1 < 0)
            {
                // we are outside the surface, we want cos(theta) to be positive
                cos_theta1 = -cos_theta1;
            }
            else
            {
                // we are inside the surface, cos(theta) is already positive but reverse normal direction
                //Nrefr = -N;
                // swap the refraction indices
                n_aux = n1;
                n1 = n2;
                n2 = n_aux;
            }
            float n = n1 / n2;

            float cos_theta2 = 1 - n * n * (1 - cos_theta1 * cos_theta1);

            if(cos_theta2 < 0)
            {
                V3 r = new V3(0,0,0);
                
                return r;

            }
            else
            {
                V3 r =  n*(ray_direction + cos_theta1* N) - (float)Math.Sqrt(cos_theta2) * N;
                r.Normalize();
                return r;
            }
                
            


        }


        private static void fresnel(float reflective_index, float refractive_index, V3 ray_direction, V3 N, ref float Kr, ref float Kt)
        {
            float cos1 = N*ray_direction;
            float n_aux = 0, n1 = 1, n2 = refractive_index;
            if (cos1 > 0)
            {
                n_aux = n1;
                n1 = n2;
                n2 = n_aux;
            }
            // Compute sini using Snell's law
            float sin2 = (n1 / n2) * (float)Math.Sqrt(Math.Max(0, 1 - cos1 * cos1));
            // Total internal reflection
            if (sin2 >= 1)
            {
                Kr = 1;
            }
            else
            {
                float cos2 = (float)Math.Sqrt(Math.Max(0, 1 - sin2 * sin2));
                cos1 = Math.Abs(cos1);
                float Rs = ((n2 * cos1) - (n1 * cos2)) / ((n2 * cos1) + (n1 * cos2));
                float Rp = ((n1 * cos1) - (n2 * cos2)) / ((n1 * cos1) + (n2 * cos2));
                Kr = ((Rs * Rs + Rp * Rp) / 2)*reflective_index;
            }
            // As a consequence of the conservation of energy, transmittance is given by:
            Kt = (1 - Kr);
        }


        public static Couleur RayTracer(V3 ray_origin, V3 ray_direction, int depth)
        {
            Object obj = null;
            float t = -1, t_min = float.MaxValue;
            V3 P_test, N_test, intersection_P, intersection_N;

            intersection_P = intersection_N = new V3(0, 0, 0);
            for (int i = 0; i < objects_on_screen.Count; i++)
            {
             
                if (Elements.objects_on_screen[i].intersection_RayObject(ray_origin, ray_direction, out P_test, out N_test, out t))
                {
                    if (t < t_min && t > 0)
                    {
                        obj = Elements.objects_on_screen[i];
                        intersection_P = P_test;
                        intersection_N = N_test;
                        t_min = t;
                    }
                }
            }
            if (obj == null)
                return Black;
            // if the object material is specular, split the ray into a reflection
            // and a refraction ray
            if (obj.is_spec && depth < MAX_RAY_DEPTH)
            {
                Couleur reflectionColor = Black;
                // compute reflection
                V3 reflectionRay;
                Couleur color = obj.getPixelColor(intersection_P);
                // recurse
                if (obj.reflective_index > 0)
                {
                    reflectionRay = computeReflectionRay(ray_direction, intersection_N);
                    // recurse
                    reflectionColor = obj.reflective_index * RayTracer(intersection_P, reflectionRay, depth + 1);
                    
                }


                V3 refractionRay;

                // recurse
                if (obj.refractive_index != 0)
                {
                    refractionRay = computeRefractionRay(obj.refractive_index, ray_direction, intersection_N);

                    Couleur refractionColor = RayTracer(intersection_P, refractionRay, depth + 1);

                    float Kr = 1, Kt = 1;
                    fresnel(obj.reflective_index, obj.refractive_index, intersection_N, ray_direction, ref Kr, ref Kt);
                    color += reflectionColor * Kr + refractionColor * Kt;
                    //color += refractionColor;
                    color.check();
                    return color;
                }
                color += reflectionColor;
                color.check();

                return color;
            }
            // object is a diffuse opaque object        
            // compute color
            return obj.getPixelColor(intersection_P);
        }

    }
}