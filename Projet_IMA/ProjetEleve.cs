using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    static class ProjetEleve
    {
      
        public static void Go()
        {

            int screen_w = BitmapEcran.GetWidth(), screen_h = BitmapEcran.GetHeight();
            V3 camera_position = new V3(screen_w/2, -400, screen_h/2);


            /* Lights */

            // Light sources
            V3 key_light_dir = new V3(-1,1,-1);
            Couleur key_light_color = new Couleur(0.8f, 0.8f, 0.8f);
            //Lights.add_light(key_light_dir, key_light_color);

            V3 fill_light_dir = new V3(1,1,-1);
            Couleur fill_light_color = new Couleur(0.5f, 0.5f, 0.5f);
            //Lights.add_light(fill_light_dir, fill_light_color);

            V3 point_light_position = new V3(screen_w*0.5f, Scene.max_y_position/2, screen_h-43);
            Couleur point_light_color = new Couleur(1f, 1f, 1f);
            Lights.add_light(point_light_position, point_light_color, true);

            /* Objects */
            // list of all objects appearing on screen
            List<Object> objects_on_screen = new List<Object>();

            Scene.Room(ref objects_on_screen);
            Scene.Boxes(ref objects_on_screen);
            Scene.Mirror(ref objects_on_screen);
            Scene.Bed(ref objects_on_screen);
            Scene.Ball(ref objects_on_screen);
            Scene.Shelf(ref objects_on_screen);

            // Creates and initialize Z_buffer/ray casting activate + determines the camera position
            Elements.inicialize_Elements(camera_position, true, objects_on_screen);

            // Plot all objects on the screen with the ray casting method
            
            /*for (int i = 0; i < screen_w; i++)
            {
                for(int j = 0; j < screen_h; j++)
                {
                    Elements.RayCasting(new V3(i, 0, j), objects_on_screen);
                }
            }*/
            
            V3 ray_direction;
            Couleur c;
            for (int i = 0; i < screen_w; i++)
            {
                for (int j = 0; j < screen_h; j++)
                {
                    ray_direction = new V3(i, 0, j) - Elements.camera_position;
                    ray_direction.Normalize();
                    c = Elements.RayTracer(Elements.camera_position, ray_direction, 0);
                    BitmapEcran.DrawPixel(i, j, c);
                }
            }
        }
    }
}
