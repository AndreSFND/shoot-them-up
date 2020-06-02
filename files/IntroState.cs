using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Main{
	public class IntroState : Screen{

		public static Texture logo;
		public static bool delay = false;

		public override void Draw(){
			V.window.SetView(V.hud);
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);

			TelaPrincipal();
		}

		public override void OnExit(){
			
		}

		public override void OnStart(){
			Idk.windowColor = new Color(53, 74, 95);
			Screen.width 	= 800;
			Screen.height 	= 600;
			V.window.Position =  new Vector2i((int)(VideoMode.DesktopMode.Width/2)-(int)(Screen.width/2), (int)(VideoMode.DesktopMode.Height/2)-(int)(Screen.height/2)-30);
			
			logo = new Texture("res/logo.png");

			Efeitos.opacidade = 255;
			Efeitos.Backdrop("fade-out", 0, 2.5f);
		}

		public static void TelaPrincipal(){
			V.img[0].TextureRect 	= new IntRect(0, 0, 300, 375);
			V.img[0].Texture 		= logo;
			V.img[0].Position 		= new Vector2f(Screen.width/2-150, Screen.height/2-187);
			V.window.Draw(V.img[0]);

			if(Efeitos.opacidade <= 0 && !F.TeclaDesativada("delay") && !delay){
				delay = true;
				F.DesativarTecla("delay", 750);
			}

			if(Efeitos.opacidade <= 0 && !F.TeclaDesativada("delay") && delay)
				Efeitos.Backdrop("fade-in", 255, 2.5f);

			if(Efeitos.opacidade >= 255 && delay)
				CurrentScreen.Change("mainmenu");

			if(F.Key("space")){
				Efeitos.opacidade = 255;
				CurrentScreen.Change("mainmenu");
				F.DesativarTecla("space", 500);
			}
		}
	}
}