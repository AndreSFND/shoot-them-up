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
	public class MainMenuState : Screen{

		public override void Draw(){
			V.window.SetView(V.hud);
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);

			switch(TelaPrincipal()){
				case 0:
					V.multiplayer = false;
					CurrentScreen.Change("instrucoes");
				break;

				case 1:
					V.multiplayer = true;
					CurrentScreen.Change("instrucoes");
				break;

				case 2:
					CurrentScreen.Change("credits");
				break;

				case 3:
					V.window.Close();
				break;
			}
		}

		public override void OnExit(){

			V.background.Clear();

		}

		public override void OnStart(){
			Efeitos.Backdrop("fade-out", 0, 2.5f);

			for(int i=0;i<8;i++)
				for(int j=-1;j<6;j++)
					V.background.Add(new Background((i % 2 == 0 ? 0 : 1), (j % 2 == 0 ? i : i-0.5f), j));

			Idk.windowColor = new Color(000, 000, 000);
			V.view.Center 	= new Vector2f(Screen.width/2, (Screen.height/2)+128);

		}

		public static int TelaPrincipal(){

			V.window.SetView(V.view);

				foreach(Background x in V.background.ToList())
					x.Draw();

			V.window.SetView(V.hud);

				string[] opcoesMenu = {"Single Player", "Multiplayer", "Credits", "Exit"};

				F.Escrever("Shoot them up", true, Screen.width/2-F.TxtWidth("Shoot them up", 50, true)/2, 20, 50, 255, 255, 255, 255);			
				F.DesenharShape(Screen.width/2-F.TxtWidth(opcoesMenu[option], 32, false)/2-5, Screen.height/2-35+40*option, F.TxtWidth(opcoesMenu[option], 32, false)+15, 35, 255, 255, 255, 255);

				for(int m=0;m<opcoesMenu.Count();m++){
					byte b = option == m ? (byte)000 : (byte)255;
					F.Escrever(opcoesMenu[m], false, Screen.width/2-F.TxtWidth(opcoesMenu[m], 32, false)/2, Screen.height/2-40+40*m, 32, b, b, b, 255);
				}

				F.Escrever("v1.0", false, Screen.width-F.TxtWidth("v1.0", 32, false)-30, Screen.height-50, 32, 255, 255, 255, 255);

				if((F.Key("s") && !F.TeclaDesativada("option")) && option < opcoesMenu.Length-1){
					option++;
					F.DesativarTecla("option", 175);
				}
				else if((F.Key("w") && !F.TeclaDesativada("option"))  && option > 0){
					option--;
					F.DesativarTecla("option", 175);
				}
				else if(F.Key("space") && !F.TeclaDesativada("space")){
					F.DesativarTecla("space", 175);
					F.DesativarTecla("mouseLeft", 175);
					return option;
				}

				return -1;

		}

		public static int option 	= 0;
	}
}