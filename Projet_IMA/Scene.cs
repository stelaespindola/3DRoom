using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_IMA
{
    static class Scene
    {
        static Couleur Green = new Couleur(0.0f, 1.0f, 0.0f);
        static Couleur Green2 = new Couleur(0.04f, 0.4f, 0.13f);
        static Couleur Red = new Couleur(1.0f, 0.0f, 0.0f);
        static Couleur Blue = new Couleur(0.0f, 0.0f, 1.0f);

        static Couleur Orange = new Couleur(1.0f, 0.64f, 0.0f);


        static Couleur Cyan = new Couleur(0.0f, 1.0f, 1.0f);
        static Couleur Magenta = new Couleur(1.0f, 0.0f, 1.0f);
        static Couleur Pink1 = new Couleur(1.0f, 0.33f, 0.65f);
        static Couleur Pink2 = new Couleur(0.89f, 0.14f, 0.61f);

        static Couleur Yellow = new Couleur(1.0f, 1.0f, 0.0f);

        static Couleur Yellow2 = new Couleur(0.9f, 0.9f, 0.8f);

        static Couleur Black = new Couleur(0.0f, 0.0f, 0.0f);
        static Couleur White = new Couleur(1.0f, 1.0f, 1.0f);


        static Texture bump_gold = new Texture("gold_bump.jpg");
        static Texture T_A = new Texture("A.jpg", 1, 1);
        static Texture T_A_bump = new Texture("A_bump.jpg", 1, 1);
        static Texture bump_1 = new Texture("bump1.jpg");
        static Texture T_test = new Texture("test.jpg", 1.5f, 1);
        static Texture T_gold = new Texture("gold.jpg");
        static Texture bump_lead = new Texture("lead_bump.jpg");
        static Texture T_lead = new Texture("lead.jpg");
        static Texture T_bricks = new Texture("brick01.jpg");

        static Texture T_wood = new Texture("wood.jpg");
        static Texture T_clouds = new Texture("wallpaper.jpg");

        static public int max_y_position = 600;
        static int screen_w = BitmapEcran.GetWidth(), screen_h = BitmapEcran.GetHeight();


        static public void Rect(ref List<Object> objects_on_screen)
        {
            V3 A, B, C, D, E, F, G, H;
            float y = 400;
            float box_lenght = 150;
            float starting_w_point = screen_w * 0.2f;
            float starting_h_point = screen_h * 0.4f;

            //Box 1

            //front
            A = new V3(starting_w_point, y, starting_h_point);
            B = new V3(starting_w_point + box_lenght, y + box_lenght, starting_h_point);
            C = new V3(starting_w_point, y, starting_h_point + box_lenght);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Blue, objects_on_screen.Count));
            objects_on_screen.Last<Object>().setSpecularity(-1.5f, 0f);

        }
        static public void Boxes(ref List<Object> objects_on_screen) {
            V3 A, B, C, D, E, F, G, H;
            float y = 400;
            float box_lenght = 150;
            float starting_w_point = screen_w * 0.8f;
            float starting_h_point = screen_h * 0f;

            //Box 1

            //front
            A = new V3(starting_w_point, y, starting_h_point);
            B = new V3(starting_w_point+box_lenght, y, starting_h_point);
            C = new V3(starting_w_point, y, starting_h_point+box_lenght);

            objects_on_screen.Add(new Rectangle_2(A, B, C, T_A, objects_on_screen.Count));

            //right side
            D = B + new V3(0, box_lenght, 0);
            E = B + new V3(0, 0, box_lenght);

            objects_on_screen.Add(new Rectangle_2(B, D, E, T_A, objects_on_screen.Count));

            //top
            F = C + new V3(box_lenght, 0, 0);
            G = C + new V3(0, box_lenght, 0);

            objects_on_screen.Add(new Rectangle_2(C, F, G, T_A, objects_on_screen.Count));

            //left side
            H = A + new V3(0, box_lenght, 0); 
            objects_on_screen.Add(new Rectangle_2(A, H, C, T_A, objects_on_screen.Count));

            // back
            objects_on_screen.Add(new Rectangle_2(H, D, G, T_A, objects_on_screen.Count));

            //bottom
            objects_on_screen.Add(new Rectangle_2(A, B, H, T_A, objects_on_screen.Count));


            starting_w_point += box_lenght/3;
            starting_h_point = screen_h * 0f + box_lenght;
            box_lenght = 75;

            SnowGlobe(ref objects_on_screen, starting_w_point, y, box_lenght);
        }
        static public void SnowGlobe(ref List<Object> objects_on_screen, float w, float y, float h)
        {
            // test sphere
            V3 center =  new V3(w+h/2, y+h, 2.5f*h);
            objects_on_screen.Add(new Sphere(70, center, Black, objects_on_screen.Count));
            objects_on_screen.Last<Object>().setSpecularity(1.5f, 0f);

             Snowman(ref objects_on_screen, center);

        }

        static public void Ball(ref List<Object> objects_on_screen)
        {
            // test sphere
            V3 center = new V3(screen_w*0.7f,max_y_position/4, 50);
            objects_on_screen.Add(new Sphere(50, center, T_test, objects_on_screen.Count));
            
        }

        static public void Mirror(ref List<Object> objects_on_screen)
        {
            V3 A, B, C;
            float y = 300;
            float mirror_lenght = 400;
            float starting_w_point = screen_w * 0.99f;
            float starting_h_point = screen_h * 0f;

            A = new V3(starting_w_point, y, starting_h_point);
            B = new V3(starting_w_point , y + 2*mirror_lenght / 3, starting_h_point);
            C = new V3(starting_w_point, y, starting_h_point +mirror_lenght);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Black, objects_on_screen.Count));
            objects_on_screen.Last<Object>().setSpecularity(0, 0.8f);

        }


        static public void Bed(ref List<Object> objects_on_screen)
        {
            V3 A, B, C;
            float y = 100;
            float bed_lenght = 500;
            float starting_w_point = screen_w * 0f;
            float starting_h_point = screen_h * 0.15f;

            // Matress

            A = new V3(starting_w_point + bed_lenght -10, y + 10, starting_h_point + 20);
            B = new V3(starting_w_point + bed_lenght -10, y + bed_lenght / 2 -10, starting_h_point + 20);
            C = new V3(starting_w_point+10, y + 10, starting_h_point +20);

            objects_on_screen.Add(new Rectangle_2(A, B, C, White, objects_on_screen.Count));
            objects_on_screen.Last<Object>().setBump(bump_1, 400f);

            A = new V3(starting_w_point + bed_lenght - 10, y + 10, starting_h_point);
            B = new V3(starting_w_point + bed_lenght - 10, y + 10, starting_h_point + 20);
            C = new V3(starting_w_point + 10, y + 10, starting_h_point);

            objects_on_screen.Add(new Rectangle_2(A, B, C, White, objects_on_screen.Count));

            A = new V3(starting_w_point + bed_lenght - 10, y +10, starting_h_point);
            B = new V3(starting_w_point + bed_lenght - 10, y +bed_lenght / 2 -10, starting_h_point + 20);
            C = new V3(starting_w_point + bed_lenght - 10, y + 10, starting_h_point + 20);

            objects_on_screen.Add(new Rectangle_2(A, B, C, White, objects_on_screen.Count));

            // Pillow
            V3 center;
            float R = 20;
            for (int i = 30; i < bed_lenght / 2 - 80; i++)
            {
                center = new V3(starting_w_point + R, y + i, starting_h_point + R + 20);
                objects_on_screen.Add(new Sphere(R, center, Pink1, objects_on_screen.Count));
            }

            // Bed
            // top
            A = new V3(starting_w_point + bed_lenght, y, starting_h_point);
            B = new V3(starting_w_point + bed_lenght, y + bed_lenght/2, starting_h_point);
            C = new V3(starting_w_point, y, starting_h_point);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Blue, objects_on_screen.Count));
            //objects_on_screen.Last<Object>().setBump(bump_1, 400f);

            // side shown
            A = new V3(starting_w_point + bed_lenght, y, 0);
            B = new V3(starting_w_point + bed_lenght, y, starting_h_point);
            C = new V3(starting_w_point, y, 0);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Blue, objects_on_screen.Count));
           

            // side hidden
            A = new V3(starting_w_point + bed_lenght, y + bed_lenght/2, 0);
            B = new V3(starting_w_point + bed_lenght, y + bed_lenght / 2, starting_h_point);
            C = new V3(starting_w_point, y + bed_lenght / 2, 0);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Blue, objects_on_screen.Count));
            

            // front
            A = new V3(starting_w_point + bed_lenght, y + bed_lenght/2, 0);
            B = new V3(starting_w_point + bed_lenght, y + bed_lenght/2, starting_h_point);
            C = new V3(starting_w_point + bed_lenght, y, 0);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Blue, objects_on_screen.Count));
      

        }


        static public void Shelf(ref List<Object> objects_on_screen)
        {
            V3 A, B, C;
            float y = max_y_position-100;
            float shelf_lenght = 400;
            float starting_w_point = screen_w * 0f;
            float starting_h_point = screen_h * 0.65f;

            // top
            A = new V3(starting_w_point + shelf_lenght, y, starting_h_point);
            B = new V3(starting_w_point + shelf_lenght, y + shelf_lenght / 3, starting_h_point);
            C = new V3(starting_w_point, y, starting_h_point);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Red, objects_on_screen.Count));

            // bottom
            A = new V3(starting_w_point + shelf_lenght, y, starting_h_point-20);
            C = new V3(starting_w_point + shelf_lenght, y + shelf_lenght / 3, starting_h_point-20);
            B = new V3(starting_w_point, y, starting_h_point-20);

            objects_on_screen.Add(new Rectangle_2(A, B, C, White, objects_on_screen.Count));

            //front
            A = new V3(starting_w_point + shelf_lenght, y, starting_h_point-20);
            B = new V3(starting_w_point + shelf_lenght, y, starting_h_point);
            C = new V3(starting_w_point, y, starting_h_point-20);

            objects_on_screen.Add(new Rectangle_2(A, B, C, White, objects_on_screen.Count));

            //side

            A = new V3(starting_w_point + shelf_lenght, y, starting_h_point - 20);
            B = new V3(starting_w_point + shelf_lenght, y + shelf_lenght, starting_h_point - 20);
            C = new V3(starting_w_point + shelf_lenght, y, starting_h_point);

            objects_on_screen.Add(new Rectangle_2(A, B, C, White, objects_on_screen.Count));

            float book_size = 84;
            y = max_y_position - 50;
            
            // book 1

            A = new V3(starting_w_point + 40, y, starting_h_point);
            B = new V3(starting_w_point + 40, y + shelf_lenght-3, starting_h_point);
            C = new V3(starting_w_point + 30, y, starting_h_point+ book_size);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Yellow, objects_on_screen.Count));

            B = new V3(starting_w_point + 30, y, starting_h_point + book_size);
            C = new V3(starting_w_point + 20, y, starting_h_point);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Yellow, objects_on_screen.Count));

            A = new V3(starting_w_point + 20, y, starting_h_point);
            C = new V3(starting_w_point + 20, y + shelf_lenght - 3, starting_h_point);
            B = new V3(starting_w_point, y, starting_h_point + book_size);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Yellow, objects_on_screen.Count));

            // book 2
            book_size = 150;

            A = new V3(starting_w_point + book_size +50, y, starting_h_point + 35);
            B = new V3(starting_w_point + book_size +50, y + shelf_lenght, starting_h_point+35);
            C = new V3(starting_w_point + 50, y, starting_h_point+35);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Red, objects_on_screen.Count));

            A = new V3(starting_w_point + book_size + 50, y, starting_h_point);
            B = new V3(starting_w_point + book_size + 50, y, starting_h_point + 35);
            C = new V3(starting_w_point + 50, y, starting_h_point);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Red, objects_on_screen.Count));

            A = new V3(starting_w_point + book_size + 45, y, starting_h_point);
            B = new V3(starting_w_point + book_size + 45, y + shelf_lenght-5, starting_h_point);
            C = new V3(starting_w_point + book_size + 45, y, starting_h_point +35);

            objects_on_screen.Add(new Rectangle_2(A, B, C, Yellow2, objects_on_screen.Count));
            objects_on_screen.Last<Object>().setBump(bump_lead, 40f);

        }

        static public void Snowman(ref List<Object> objects_on_screen, V3 center_g)
        {
            
            float R_max = 20;
            /** Snowman **/
            //Body
            center_g.z -= 10;
            V3 center =  center_g;
            objects_on_screen.Add(new Sphere(R_max, center, White, objects_on_screen.Count));

            center += new V3(0, 0, R_max);
            objects_on_screen.Add(new Sphere((R_max/3)*2.25f, center, White, objects_on_screen.Count));
        
            center += new V3(0, 0, R_max*0.8f);
            objects_on_screen.Add(new Sphere(R_max/2, center, White, objects_on_screen.Count));

            // Buttons
            float R_button = 3;
            V3 center2 = center_g + new V3(0, 0, R_max);
            // 1 button
            center2 += new V3(0, -R_max*0.75f , 0);
            objects_on_screen.Add(new Sphere(R_button, center2, Black, objects_on_screen.Count));
            objects_on_screen.Last<Object>().setSpecularity();
            // 2 button
            center2 += new V3(0, R_max * 0.05f, 6);
            objects_on_screen.Add(new Sphere(R_button, center2, Black, objects_on_screen.Count));
            objects_on_screen.Last<Object>().setSpecularity();


            // Eyes

            // left
            center2 = center_g +  new V3(-3, -R_max/2 + 7, R_max * 2.2f); ;
            objects_on_screen.Add(new Sphere(2, center2, Black, objects_on_screen.Count));
            objects_on_screen.Last<Object>().setSpecularity();

            // right
            center2 += new V3(+6,0, 0); ;
            objects_on_screen.Add(new Sphere(2, center2, Black, objects_on_screen.Count));
            objects_on_screen.Last<Object>().setSpecularity();


            // Nose

            center2 += new V3(-3,-5, -3); ;
            objects_on_screen.Add(new Sphere(2, center2, Orange, objects_on_screen.Count));


            // Hat
            center2 += new V3(1, 11, 4); ;
            objects_on_screen.Add(new Sphere(5, center2, Green2, objects_on_screen.Count));
            objects_on_screen.Last<Object>().setSpecularity();

            



        }

        static public void Room(ref List<Object> objects_on_screen)
        {
            /** Room **/

            V3 A, B, C;
            float y = max_y_position;

            //Back wall
                       
            A = new V3(screen_w * 0f, y, screen_h * 0f);
            B = new V3(screen_w * 1f, y, screen_h * 0f);
            C = new V3(screen_w * 0f, y, screen_h * 1f);

           objects_on_screen.Add(new Rectangle_2(A, B, C, T_clouds, objects_on_screen.Count));

            // Left wall
            A = new V3(screen_w * 0f, 0, screen_h * 0f);
            B = new V3(screen_w * 0f, y, screen_h * 0f);
            C = new V3(screen_w * 0f, 0, screen_h * 1f);


            objects_on_screen.Add(new Rectangle_2(A, B, C, T_bricks, objects_on_screen.Count));

            // Right wall
            A = new V3(screen_w * 1f, y, screen_h * 0f);
            C = new V3(screen_w * 1f, y, screen_h * 1f);
            B = new V3(screen_w * 1f, 0, screen_h * 0f);


            objects_on_screen.Add(new Rectangle_2(A, B, C, T_bricks, objects_on_screen.Count));

            // Floor

            A = new V3(screen_w * 0f, 0, screen_h * 0f);
            B = new V3(screen_w * 1f, 0, screen_h * 0f);
            C = new V3(screen_w * 0f, y, screen_h * 0f);

            objects_on_screen.Add(new Rectangle_2(A, B, C, T_wood, objects_on_screen.Count));

            // Celling

            A = new V3(screen_w * 0f, y, screen_h * 1f);
            B = new V3(screen_w * 1f, y, screen_h * 1f);
            C = new V3(screen_w * 0f, 0, screen_h * 1f);

            objects_on_screen.Add(new Rectangle_2(A, B, C, White, objects_on_screen.Count));

            // Front wall
            A = new V3(screen_w * 0f, 0, screen_h * 0f);
            C = new V3(screen_w * 0f, 0, screen_h * 1f);
            B = new V3(screen_w * 1f, 0, screen_h * 0f);

            //objects_on_screen.Add(new Rectangle_2(A, B, C, Green, objects_on_screen.Count));

            // Lamp

            V3 center = new V3(screen_w*0.5f, y/2, screen_h*1.14f);
            objects_on_screen.Add(new Sphere(100, center, White, objects_on_screen.Count));
            objects_on_screen.Last<Object>().setSpecularity(1.2f, 0.1f);

        }
    }
}
