using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Text.RegularExpressions;

namespace Main{
	public class V{

		public static RenderWindow window 	= new RenderWindow(new VideoMode(Screen.width, Screen.height), "Shoot them up v1.0", Styles.Titlebar);
		public static View view 			= new View(new Vector2f(Screen.width/2, Screen.height/2), new Vector2f(Screen.width, Screen.height));
		public static View hud 				= new View(new Vector2f(Screen.width/2, Screen.height/2), new Vector2f(Screen.width, Screen.height));
		public static Font eBit				= new Font("res/fonts/PixelOperator.ttf");
		public static Font eBitBold			= new Font("res/fonts/PixelOperatorMono-Bold.ttf");
		public static Event evento;
		public static string tecla			= "";
		public static Random random			= new Random();

		public static float letraTempo;
		public static bool 	backdrop 		= true;
		public static bool 	multiplayer		= false;

		public static List<Geral> 					objetos 			= new List<Geral>();

		public static List<Limite> 					limites 			= new List<Limite>();
		public static List<Bullet> 					bullets 			= new List<Bullet>();
		public static List<Bonus> 					bonus 				= new List<Bonus>();
		public static List<Background> 				background 			= new List<Background>();	
		public static List<Controle> 				controles 			= new List<Controle>();

		public static List<TeclaDesativada>			teclasDesativadas 	= new List<TeclaDesativada>();
		public static List<Texture>[] 				IMG_CAT 			= new List<Texture>[7];

		public static Sprite[] img 						= new Sprite[2];

		public static void ImgLoad(){
			int m;

			img[0]		= new Sprite();
			img[1]		= new Sprite();

			string[] diretorios = new string[]{
				"player",
				"icone",
				"background",
				"enemy",
				"bonus",
				"player_bullet",
				"enemy_bullet"
			};

			for(m=0;m<IMG_CAT.Count();m++)
				IMG_CAT[m] 	= new List<Texture>();

			for(int d=0;d<diretorios.Count();d++){
				m = 0;
				while(System.IO.File.Exists(@"res\img\"+diretorios[d]+@"\"+m+".png")){ 
					IMG_CAT[d].Add(new Texture("res/img/"+diretorios[d]+"/"+m+".png"));
					m++;
				}
			}

		}

		public static Player player1;

	}

	public partial class F{

		public static void AtivarTecla(string tecla){
			TeclaDesativada it = V.teclasDesativadas.Find(item => item.keyCode == tecla);
			if(it != null)
				V.teclasDesativadas.Remove(it);	
		}

		public static void DesativarTecla(string tecla, int mls){
			TeclaDesativada it = V.teclasDesativadas.Find(item => item.keyCode == tecla);
			if(it == null)
				V.teclasDesativadas.Add(new TeclaDesativada(tecla, mls));		
		}

		public static bool TeclaDesativada(string tecla){
			TeclaDesativada it = V.teclasDesativadas.Find(item => item.keyCode == tecla);

			return it != null ? true : false;
		}

		public static void TeclasDesativadasF(){
			int l = V.teclasDesativadas.Count;
			for(int x=0;x<l;x++){
				TimeSpan time = DateTime.Now - V.teclasDesativadas[x].inicio;

				if(time.TotalMilliseconds > V.teclasDesativadas[x].ms){
					V.teclasDesativadas.RemoveAt(x);
					break;
				}
			}
		}

		public static void DesenharShape(float x, float y, float width, float height, byte r, byte g, byte b, float op){
			RectangleShape shape 	= new RectangleShape();
			shape.Position 			= new Vector2f(x, y);
			shape.Size 				= new Vector2f(width, height);
			shape.FillColor 		= new Color(r, g, b, (byte)op);
			V.window.Draw(shape);
		}

		public static void DesenharShape(float x, float y, float width, float height, byte r, byte g, byte b, float op, byte r_, byte g_, byte b_, byte op_, float thickness){
			RectangleShape shape 	= new RectangleShape();
			shape.Position 			= new Vector2f(x, y);
			shape.Size 				= new Vector2f(width, height);
			shape.OutlineColor 		= new Color(r_, g_, b_, op_);
			shape.OutlineThickness 	= thickness;
			shape.FillColor 		= new Color(r, g, b, (byte)op);
			V.window.Draw(shape);
		}
		
		public static void DesenharTriangulo(float x, float y, float size, byte r, byte g, byte b, byte op){
			CircleShape triangle	= new CircleShape(size, 3);
			triangle.Position 		= new Vector2f(x, y);
			triangle.FillColor 		= new Color(r, g, b, (byte)op);
			triangle.Rotation		= 180;
			V.window.Draw(triangle);
		}

		public static void DesenharCirculo(float x, float y, float raio, float points, byte r, byte g, byte b, float op){
			CircleShape shape 		= new CircleShape();
			shape.FillColor 		= new Color(r,g,b,(byte)op);
			shape.Position 			= new Vector2f(x, y);
			shape.Radius 			= raio;
			shape.SetPointCount((uint)points);
			V.window.Draw(shape);
		}

		public static void Escrever(string text, bool bold, float x, float y, uint fontsize, byte r, byte g, byte b, byte op){
			Font font = bold ? V.eBitBold : V.eBit;

			Text texto 				= new Text(text, font, fontsize);
            texto.Position 			= new Vector2f((int)x, (int)y);
            texto.FillColor 		= new Color(r, g, b, op);
			
			V.window.Draw(texto);
		}

		public static void Escrever(string text, bool bold, float x, float y, uint fontsize, byte r, byte g, byte b, byte op, byte r_, byte g_, byte b_, byte op_, float t){
			Font font = bold ? V.eBitBold : V.eBit;

			Text texto 				= new Text(text, font, fontsize);
            texto.Position 			= new Vector2f((int)x, (int)y);
            texto.FillColor 		= new Color(r, g, b, op);
            texto.OutlineColor 		= new Color(r_, g_, b_, op_);
            texto.OutlineThickness 	= t;
			
			V.window.Draw(texto);
		}

		public static float TxtWidth(string text, uint fontsize, bool bold){
			Text texto 				= new Text();
			texto.Font 				= bold ? V.eBitBold : V.eBit;
			texto.DisplayedString 	= text;
			texto.CharacterSize 	= fontsize;
			return texto.GetGlobalBounds().Width;
		}

		public static float TxtHeight(string text, uint fontsize, bool bold){
			Text texto 				= new Text();
			texto.Font 				= bold ? V.eBitBold : V.eBit;
			texto.DisplayedString 	= text;
			texto.CharacterSize 	= fontsize;
			return texto.GetGlobalBounds().Height;
		}

	}
}