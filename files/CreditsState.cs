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
	public class CreditsState : Screen{

		public override void Draw(){
			V.window.SetView(V.hud);
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);

			TelaPrincipal();
		}

		public override void OnExit(){

			V.background.Clear();

		}

		public override void OnStart(){

			V.backdrop 	= true;

			for(int i=0;i<8;i++)
				for(int j=-1;j<6;j++)
					V.background.Add(new Background((i % 2 == 0 ? 2 : 3), (j % 2 == 0 ? i : i-0.5f), j));

			Idk.windowColor = new Color(000, 000, 000);
			V.view.Center 	= new Vector2f(Screen.width/2, (Screen.height/2)+128);

		}

		public static void TelaPrincipal(){

			V.window.SetView(V.view);

				foreach(Background x in V.background.ToList())
					x.Draw();

			V.window.SetView(V.hud);

				F.Escrever("Credits", true, Screen.width/2-F.TxtWidth("Credits", 50, true)/2, 20, 50, 255, 255, 255, 255);

				F.Escrever("Game Developer", true, Screen.width/2-F.TxtWidth("Game Developer", 36, true)/2, Screen.height/2-50, 36, 255, 255, 255, 255);
				F.Escrever("André Santana Fernandes", false, Screen.width/2-F.TxtWidth("André Santana Fernandes", 32, false)/2, Screen.height/2-20, 32, 255, 255, 255, 255);
				F.Escrever("FoG", true, Screen.width/2-F.TxtWidth("FoG", 32, true)/2, Screen.height/2+30, 32, 255, 255, 255, 255);

				F.Escrever("v1.0", false, Screen.width-F.TxtWidth("v1.0", 32, false)-30, Screen.height-50, 32, 255, 255, 255, 255);
				
				if(F.Key("esc") && !F.TeclaDesativada("esc")){
					CurrentScreen.Change("mainmenu");
					F.DesativarTecla("esc", 175);
				}

		}
	}
}