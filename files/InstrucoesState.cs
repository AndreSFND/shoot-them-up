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
	public class InstrucoesState : Screen{

		public static int start = 0;

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
			start 		= 0;

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

				F.DesenharShape(Screen.width/2-F.TxtWidth("Start", 32, false)/2-10, Screen.height-117, F.TxtWidth("Start", 32, false)+25, 35, 255, 255, 255, 255);

				F.Escrever("How to play", true, Screen.width/2-F.TxtWidth("How to play", 50, true)/2, 20, 50, 255, 255, 255, 255);
				F.Escrever("Start", true, Screen.width/2-F.TxtWidth("Start", 32, true)/2, Screen.height-122, 32, 000, 000, 000, 255);
				F.Escrever("v1.0", false, Screen.width-F.TxtWidth("v1.0", 32, false)-30, Screen.height-50, 32, 255, 255, 255, 255);

				V.img[0].Scale 			= new Vector2f(2, 2);

				V.img[0].Texture 		= V.IMG_CAT[1][8];
				V.img[0].TextureRect	= new IntRect(0, 0, 130, 55);
				V.img[0].Position 		= new Vector2f(!V.multiplayer ? Screen.width/2-130 : Screen.width*0.3f-130, 200);
				V.window.Draw(V.img[0]);

				V.img[0].Texture 		= V.IMG_CAT[1][9];
				V.img[0].TextureRect	= new IntRect(0, 0, 48, 37);
				V.img[0].Position 		= new Vector2f(!V.multiplayer ? Screen.width/2-130 : Screen.width*0.3f-130, 350);
				V.window.Draw(V.img[0]);

				F.Escrever("Move", 		false, !V.multiplayer ? Screen.width/2-105 : Screen.width*0.3f-105, 282, 24, 255, 255, 255, 255);
				F.Escrever("Special", 	false, !V.multiplayer ? Screen.width/2-110 : Screen.width*0.3f-110, 396, 24, 255, 255, 255, 255);
				F.Escrever("Fire", 		false, !V.multiplayer ? Screen.width/2+30  : Screen.width*0.3f+30, 282, 24, 255, 255, 255, 255);

				if(V.multiplayer){

					V.img[0].Texture 		= V.IMG_CAT[1][7];
					V.img[0].TextureRect	= new IntRect(0, 0, 130, 56);
					V.img[0].Position 		= new Vector2f(Screen.width*0.7f-130, 200);
					V.window.Draw(V.img[0]);

					F.Escrever("Move", 	false, Screen.width*0.7f-105, 284, 24, 255, 255, 255, 255);
					F.Escrever("Fire", 	false, Screen.width*0.7f+50, 284, 24, 255, 255, 255, 255);

					F.Escrever("Player 1", true, Screen.width*0.3f-F.TxtWidth("Player 1", 32, true)/2, 130, 32, 255, 255, 255, 255);
					F.Escrever("Player 2", true, Screen.width*0.7f-F.TxtWidth("Player 2", 32, true)/2, 130, 32, 255, 255, 255, 255);

				}

				V.img[0].Scale 			= new Vector2f(1, 1);
				
				if(start == 1 && Efeitos.opacidade == 255)
					CurrentScreen.Change("play");

				if(F.Key("space") && !F.TeclaDesativada("space") && start == 0){
					Efeitos.Backdrop("fade-in", 255, 2.5f);

					start = 1;
					F.DesativarTecla("space", 175);
				}

				if(F.Key("esc") && !F.TeclaDesativada("esc") && start == 0){
					CurrentScreen.Change("mainmenu");
					F.DesativarTecla("esc", 175);
				}

		}
	}
}