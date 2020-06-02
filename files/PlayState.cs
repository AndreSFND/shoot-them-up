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
	public class PlayState : Screen{

		public static int state 	= 0;
		public static int state_ 	= 0;
		public static bool especial = false;
		public static bool start 	= true;
		public static bool menu 	= false;

		public static int level 			= 1;
		public static int score 			= 0;
		public static int especial_count	= 0;
		public static int enemies_killed 	= 0;
		public static Player player1, player2;
		public static DateTime inicio, fim;

		public override void Draw(){
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);

			switch(state){

				case 0:
					if(state_ <= 1 && start){ 
						V.window.SetView(V.hud);

						V.backdrop = false; 
						string txt = level == 5 ? "Free mode" : "Level "+level;
						F.Escrever(txt, true, (Screen.width/2)-(F.TxtWidth(txt, 60, true)/2), (Screen.height/2)-100, 60, 255, 255, 255, 255, 000, 000, 47, 255, 1);

						if(state_ == 0)									{ F.DesativarTecla("state", 2000); state_++;}
						if(state_ == 1 && !F.TeclaDesativada("state"))	{ F.DesativarTecla("state", 2000); state_++; V.backdrop = true; start = false; Efeitos.Backdrop("fade-out", 0, 2.5f);}
					}

					if(state_ > 1 && state_ <= 3){ 
						V.window.SetView(V.hud);

						TelaPrincipal();
						F.Escrever("Ready?", true, (Screen.width/2)-(F.TxtWidth("Ready?", 60, true)/2), (Screen.height/2)-100, 60, 255, 255, 255, 255, 000, 000, 47, 255, 2);

						if(state_ == 2)									{ F.DesativarTecla("state", 1000); state_++;}
						if(state_ == 3 && !F.TeclaDesativada("state"))	{ F.DesativarTecla("state", 1000); state_++;}
					}

					if(state_ > 3 && state_ <= 5){
						V.window.SetView(V.hud);

						TelaPrincipal();
						F.Escrever("Go!", true, (Screen.width/2)-(F.TxtWidth("Go!", 60, true)/2), (Screen.height/2)-100, 60, 255, 255, 255, 255, 000, 000, 47, 255, 2);

						if(state_ == 4)									{ F.DesativarTecla("state", 500); state_++;}
						if(state_ == 5 && !F.TeclaDesativada("state"))	{ F.DesativarTecla("state", 500); state_++; state = 2;}
					}
				break;

				case 1:

					TelaPrincipal();
					Menu();

				break;

				case 2:

					TelaPrincipal();
				
				break;

				case 3:

					TelaPrincipal();
					GameOver();

				break;

				case 4:

					TelaPrincipal();
					LevelCompleted();

				break;

				case 5:

					TelaPrincipal();
					Fim();

				break;

			}

		}

		public override void OnExit(){

		}

		public override void OnStart(){

			ResetGame();

		}

		public static void TelaPrincipal(){

			if(F.Key("esc") && !menu && !F.TeclaDesativada("esc") && state == 2){
				V.backdrop 	= false;
				menu 		= true;
				state 		= 1;
				F.DesativarTecla("esc", 300);
				Efeitos.Backdrop("fade-in", 125, 25);
			}

			if(F.Key("shift") && especial_count == 100 && !F.TeclaDesativada("shift") && state == 2){
				for(int i=0;i<15;i++)
					V.objetos.Add(new Player("ally_"+i, 52*i+36, Screen.height+128+50, 32, 32));

				especial_count = 0;
				F.DesativarTecla("shift", 300);
			}

			Raid();

			V.window.SetView(V.view);

				foreach(Background x in V.background.ToList())
					x.Draw();

				foreach(Limite x in V.limites.ToList())
					x.Draw();

				foreach(Bullet x in V.bullets.ToList())
					x.Draw();

				foreach(Bonus x in V.bonus.ToList())
					x.Draw();

				foreach(Geral x in V.objetos.ToList())
					x.Draw();

			V.window.SetView(V.hud);

				Configuracoes.Draw();
				F.Escrever("SCORE "+score, true, Screen.width-F.TxtWidth("SCORE "+score, 32, true)-40, 20, 32, 255, 255, 255, 255, 000, 000, 47, 255, 2);

				if(especial_count > 100) especial_count = 100;

				F.DesenharShape(Screen.width-122, Screen.height-42, especial_count, 20, 181, 230, 29, 255);

				if(especial && especial_count == 100)
					F.Escrever("Press Shift!", true, Screen.width-F.TxtWidth("Press Shift!", 16, true)-30, Screen.height-64, 16, 255, 255, 255, 255, 000, 000, 47, 255, 2);

				if(!F.TeclaDesativada("especial") && especial_count == 100){
					especial = !especial;
					F.DesativarTecla("especial", 1000);
				}


		}

		public static int mY 				= -1;
		public static int mA, sY 			= 0;
		public static byte cX, cY, cA 		= 0;

		public static void Menu(){

			if(F.Key("esc") && menu && !F.TeclaDesativada("esc")){
				V.backdrop 	= true;
				menu 		= false;
				state 		= 0;
				state_ 		= 2;

				F.AtivarTecla("bullet");
				F.DesativarTecla("bullet", 500);

				F.DesativarTecla("esc", 300);
				Efeitos.Backdrop("fade-out", 0, 25);
			}

			V.window.SetView(V.hud);
			Efeitos.BackdropRun();
				
			string[] e = new string[2];
			e[0] = "Continuar";
			e[1] = "Menu Principal";
			
			if((F.Key("up") || F.Key("w")) && sY > 0 && !F.TeclaDesativada("up")){
				sY--;
				F.DesativarTecla("up", 125);
			}
			if((F.Key("down") || F.Key("s")) && sY < 1 && !F.TeclaDesativada("down")){
				sY++;
				F.DesativarTecla("down", 125);
			}

			for(int m=0;m<2;m++){
				cA = (m == mA && mY == -1) ? (byte)35 : (byte)19;
				
				if(sY == m){
					cX = 35;
					cY = 255;

					if(F.Key("space")){

						switch(m){
							case 0:
								state 	= 0;
								state_ 	= 2;
							break;
							
							case 1:
								state 	= 2;
								CurrentScreen.Change("mainmenu");
							break;
						}

						menu 	= false;
						F.DesativarTecla("esc", 300);
						F.DesativarTecla("space", 300);
						Efeitos.Backdrop("fade-out", 0, 25);

					}
				}
				else{
					cX = 19;
					cY = 130;
				}
				
				F.DesenharShape(((Screen.width - 240) / 2), (Screen.height/2)+(80*m)-80, 240, 80, cX, cX, cX, 255);
				F.Escrever(e[m], true, ((Screen.width - 240) / 2)+120-(F.TxtWidth(e[m], 32, true)/2), (Screen.height/2)+16+(80*m)-80, 32, cY, cY, cY, 230);
			}
			
		}

		public static int raid_state		= 0;
		public static int raid_loop			= 0;
		public static int enemies_alive		= 0;

		public static void Raid(){

			int dif = (V.multiplayer ? 2 : 1);

			switch(level){

				case 1:
					switch(raid_state){

						case 0:
							new Enemy(0, (Screen.width*0.25f), 0, 0, false, 100, dif, dif, dif, 32, 32);
							new Enemy(1, (Screen.width*0.75f), 0, 0, false, 100, dif, dif, dif, 32, 32);
							
							F.DesativarTecla("raid", 7000);
							raid_state++;
						break;

						case 1:
							if(!F.TeclaDesativada("raid")){

								new Enemy(3, (Screen.width*0.35f), 0, 0, true, 100, dif, dif, dif, 32, 32);
								new Enemy(4, (Screen.width*0.65f), 0, 0, false, 100, dif, dif, dif, 32, 32);

								F.DesativarTecla("raid", 4000);
								raid_state++;

							}
						break;
						
						case 2:
							if(!F.TeclaDesativada("raid")){

								new Enemy(5, (Screen.width*0.45f), 0, 0, false, 100, dif, dif, dif, 32, 32);
								new Enemy(6, (Screen.width*0.55f), 0, 0, false, 100, dif, dif, dif, 32, 32);

								F.DesativarTecla("raid", 4000);
								raid_state++;

							}
						break;

						case 3:
							if(!F.TeclaDesativada("raid")){

								new Enemy(7+raid_loop, (Screen.width*0.25f), 0, 1, (V.random.NextDouble() > 0.8f), 100, dif, dif, dif, 32, 32);
								new Enemy(13+raid_loop, (Screen.width*0.75f), 0, 2, (V.random.NextDouble() > 0.8f), 100, dif, dif, dif, 32, 32);

								F.DesativarTecla("raid", 2000);

								if(raid_loop == 5){ raid_loop = 0; raid_state++;}
								else 				raid_loop++;

							}
						break;

						case 4:
							if(enemies_alive <= 0){
							 	F.DesativarTecla("raid", 3000);
							 	raid_state++;
							}
						break;

						case 5:
							if(!F.TeclaDesativada("raid"))
							 	state = 4;
						break;

					}
				break;

				case 2:
					switch(raid_state){

						case 0:
							if(!F.TeclaDesativada("raid")){

								new Enemy(0+raid_loop, (Screen.width*0.25f), 0, raid_loop % 2 == 0 ? 1 : 0, (V.random.NextDouble() > 0.8f), 100, dif, dif, dif, 32, 32);
								new Enemy(6+raid_loop, (Screen.width*0.75f), 0, raid_loop % 2 == 0 ? 2 : 0, (V.random.NextDouble() > 0.8f), 100, dif, dif, dif, 32, 32);

								F.DesativarTecla("raid", raid_loop == 0 ? 5000 : (raid_loop % 2 == 0 ? 2000 : 4000));

								if(raid_loop == 5){ raid_loop = 0; raid_state++;}
								else 				raid_loop++;

							}
						break;

						case 1:
							if(!F.TeclaDesativada("raid")){

								new Enemy(13, (Screen.width*0.5f), 1, 0, true, 500, 3, 1, 1, 32, 32);

								F.DesativarTecla("raid", 4000);
								raid_state++;

							}
						break;

						case 2:
							if(enemies_alive <= 0){
							 	F.DesativarTecla("raid", 3000);
							 	raid_state++;
							}
						break;

						case 3:
							if(!F.TeclaDesativada("raid"))
							 	state = 4;
						break;

					}
				break;

				case 3:
					switch(raid_state){

						case 0:
							if(!F.TeclaDesativada("raid")){

								new Enemy(0+raid_loop, (Screen.width*0.25f), 0, 3, (V.random.NextDouble() > 0.8f), 100, dif, dif, dif, 32, 32);
								new Enemy(6+raid_loop, (Screen.width*0.75f), 0, 4, (V.random.NextDouble() > 0.8f), 100, dif, dif, dif, 32, 32);

								F.DesativarTecla("raid", raid_loop == 0 ? 5000 : (raid_loop % 2 == 0 ? 2000 : 4000));

								if(raid_loop == 5){ raid_loop = 0; raid_state++;}
								else 				raid_loop++;

							}
						break;

						case 1:
							if(!F.TeclaDesativada("raid")){

								new Enemy(13, (Screen.width*0.5f), 1, 0, true, 500, 6, 1, 1, 32, 32);

								F.DesativarTecla("raid", 4000);
								raid_state++;

							}
						break;

						case 2:
							if(enemies_alive <= 0){
							 	F.DesativarTecla("raid", 3000);
							 	raid_state++;
							}
						break;

						case 3:
							if(!F.TeclaDesativada("raid"))
							 	state = 4;
						break;

					}
				break;

				case 4:
					switch(raid_state){

						case 0:
							if(!F.TeclaDesativada("raid")){

								new Enemy(0+raid_loop, (Screen.width*0.25f), 0, raid_loop % 2 == 0 ? 3 : 1, (V.random.NextDouble() > 0.8f), 100, dif, dif, dif, 32, 32);
								new Enemy(6+raid_loop, (Screen.width*0.75f), 0, raid_loop % 2 == 0 ? 4 : 2, (V.random.NextDouble() > 0.8f), 100, dif, dif, dif, 32, 32);

								F.DesativarTecla("raid", raid_loop == 0 ? 5000 : (raid_loop % 2 == 0 ? 2000 : 4000));

								if(raid_loop == 5){ raid_loop = 0; raid_state++;}
								else 				raid_loop++;

							}
						break;

						case 1:
							if(!F.TeclaDesativada("raid")){

								new Enemy(12, (Screen.width*0.45f), 0, 0, true, 100, dif, dif, dif, 32, 32);
								new Enemy(13, (Screen.width*0.55f), 0, 0, false, 100, dif, dif, dif, 32, 32);

								F.DesativarTecla("raid", 3000);
								raid_state++;

							}
						break;

						case 2:
							if(!F.TeclaDesativada("raid")){

								new Enemy(14, (Screen.width*0.35f), 0, 0, true, 100, dif, dif, dif, 32, 32);
								new Enemy(15, (Screen.width*0.65f), 0, 0, false, 100, dif, dif, dif, 32, 32);

								F.DesativarTecla("raid", 3000);
								raid_state++;

							}
						break;
						
						case 3:
							if(!F.TeclaDesativada("raid")){

								new Enemy(16, (Screen.width*0.25f), 0, 0, false, 100, dif, dif, dif, 32, 32);
								new Enemy(17, (Screen.width*0.75f), 0, 0, false, 100, dif, dif, dif, 32, 32);

								F.DesativarTecla("raid", 3000);
								raid_state++;

							}
						break;

						case 4:
							if(!F.TeclaDesativada("raid")){

								new Enemy(18, (Screen.width*0.45f), 1, 0, true, 500, 6, 1, 1, 32, 32);
								new Enemy(19, (Screen.width*0.55f), 1, 0, true, 500, 6, 1, 1, 32, 32);

								F.DesativarTecla("raid", 4000);
								raid_state++;

							}
						break;

						case 5:
							if(enemies_alive <= 0){
							 	F.DesativarTecla("raid", 3000);
							 	raid_state++;
							}
						break;

						case 6:
							if(!F.TeclaDesativada("raid"))
							 	state = 5;
						break;

					}
				break;

				case 5:
					switch(raid_state){

						case 0:
							if(!F.TeclaDesativada("raid")){

								new Enemy(raid_loop, (float)(Screen.width*V.random.NextDouble()), 0, V.random.Next(0, 5), (V.random.NextDouble() > 0.8f), 100, dif, dif, dif, 32, 32);
								raid_loop++;

								new Enemy(raid_loop, (float)(Screen.width*V.random.NextDouble()), 0, V.random.Next(0, 5), (V.random.NextDouble() > 0.8f), 100, dif, dif, dif, 32, 32);

								F.DesativarTecla("raid", V.multiplayer ? 2000 : 2500);

							}
						break;

					}
				break;

			}

		}

		public static int transition_state	= 0;
		public static bool gameover_option	= true;
		public static float txt_op			= 0;

		public static void GameOver(){

			V.window.SetView(V.hud);
			Efeitos.BackdropRun();

			switch(transition_state){

				case 0:
					V.bullets.Clear();
					V.backdrop = false;
					fim = DateTime.Now;

					F.DesativarTecla("transition_state", 750);

					transition_state++;
				break;

				case 1:
					if(!F.TeclaDesativada("transition_state")){
						Efeitos.Backdrop("fade-in", 255, 2.5f);

						transition_state++;
					}
				break;

				case 2:
					if(Efeitos.opacidade == 255){
						F.Escrever("Game Over", true, (Screen.width/2)-(F.TxtWidth("Game Over", 60, true)/2), (Screen.height/2)-200, 60, 255, 255, 255, (byte)txt_op, 000, 000, 47, 255, 1);

						txt_op += 2.5f;
					}
					if(txt_op == 255){
						txt_op = 0;
						transition_state++;
					}
				break;

				case 3:
					if(!F.TeclaDesativada("a") && (F.Key("a") || F.Key("d")) && txt_op >= 255){
						gameover_option = !gameover_option;
						F.DesativarTecla("a", 250);
					}

					if(F.Key("space") && txt_op >= 255){
						if(gameover_option)
							ResetGame();
						else
							CurrentScreen.Change("mainmenu");

						F.DesativarTecla("space", 300);
					}
					else{

						F.Escrever("Game Over", true, (Screen.width/2)-(F.TxtWidth("Game Over", 60, true)/2), (Screen.height/2)-200, 60, 255, 255, 255, 255);

						F.Escrever("Level........................................... "+level, false, (Screen.width/4), (Screen.height/2)-100, 36, 255, 255, 255, (byte)txt_op);
						F.Escrever("Score.......................................... "+score, false, (Screen.width/4), (Screen.height/2)-65, 36, 255, 255, 255, (byte)txt_op);
						F.Escrever("Enemies Killed........................... "+enemies_killed, false, (Screen.width/4), (Screen.height/2)-30, 36, 255, 255, 255, (byte)txt_op);
						F.Escrever("Time Alive................................... "+String.Format("{0}:{1}", (fim-inicio).Minutes, (fim-inicio).Seconds), false, (Screen.width/4), (Screen.height/2)+5, 36, 255, 255, 255, (byte)txt_op);

						if(gameover_option){

							F.DesenharShape((Screen.width / 2) - (F.TxtWidth("Main Menu", 26, true))-6, (Screen.height/2)+103, F.TxtWidth("Restart", 26, true)+12, 30, 255, 255, 255, (byte)txt_op);
							F.Escrever("Restart", true, (Screen.width/2)-F.TxtWidth("Restart", 26, true)-30, (Screen.height/2)+100, 26, 000, 000, 000, (byte)txt_op);
							F.Escrever("Main Menu", true, (Screen.width/2)+10, (Screen.height/2)+100, 26, 255, 255, 255, (byte)txt_op);

						}
						else{

							F.DesenharShape((Screen.width / 2) + 4, (Screen.height/2)+103, F.TxtWidth("Main Menu", 26, true)+12, 30, 255, 255, 255, (byte)txt_op);
							F.Escrever("Main Menu", true, (Screen.width/2)+10, (Screen.height/2)+100, 26, 000, 000, 000, (byte)txt_op);
							F.Escrever("Restart", true, (Screen.width/2)-F.TxtWidth("Restart", 26, true)-30, (Screen.height/2)+100, 26, 255, 255, 255, (byte)txt_op);

						}

						txt_op += txt_op >= 255 ? 0 : 5;
					}
				break;

			}

		}

		public static void LevelCompleted(){

			V.window.SetView(V.hud);
			Efeitos.BackdropRun();

			switch(transition_state){

				case 0:
					F.DesativarTecla("transition_state", 1500);

					V.bullets.Clear();
					V.backdrop = false;
					fim = DateTime.Now;

					transition_state++;
				break;

				case 1:
					if(!F.TeclaDesativada("transition_state")){
						if(!V.multiplayer){
							if(player1.y+player1.height > 0 || player1.x+player1.width/2 != Screen.width/2){
								player1.y -= (player1.y+player1.height > 0 ? (player1.x+player1.width/2 == Screen.width/2 ? player1.speed*2 : player1.speed/2) : 0);
								player1.x += (player1.x+player1.width/2 == Screen.width/2 ? 0 : (player1.x+player1.width/2 > Screen.width/2 ? -player1.speed/2 : player1.speed/2));

								if(Math.Abs(player1.x+player1.width/2 - Screen.width/2) < 5) player1.x = Screen.width/2-player1.width/2;

								break;
							}
						}
						else
							if(player1.y+player1.height > 0 || player2.y+player2.height > 0 || player1.x+player1.width/2 != Screen.width*0.25f || player2.x+player2.width/2 != Screen.width*0.75f){
								player1.y -= (player1.y+player1.height > 0 ? (player1.x+player1.width/2 == Screen.width*0.25f ? player1.speed*2 : player1.speed/2) : 0);
								player1.x += (player1.x+player1.width/2 == Screen.width*0.25f ? 0 : (player1.x+player1.width/2 > Screen.width*0.25f ? -player1.speed/2 : player1.speed/2));

								player2.y -= (player2.y+player2.height > 0 ? (player2.x+player2.width/2 == Screen.width*0.75f ? player2.speed*2 : player2.speed/2) : 0);
								player2.x += (player2.x+player2.width/2 == Screen.width*0.75f ? 0 : (player2.x+player2.width/2 > Screen.width*0.75f ? -player2.speed/2 : player2.speed/2));

								if(Math.Abs(player1.x+player1.width/2 - Screen.width*0.25f) < 5) player1.x = Screen.width*0.25f-player1.width/2;
								if(Math.Abs(player2.x+player2.width/2 - Screen.width*0.75f) < 5) player2.x = Screen.width*0.75f-player2.width/2;

								break;
							}

						F.DesativarTecla("transition_state", 750);
						transition_state++;
					}
				break;

				case 2:
					if(!F.TeclaDesativada("transition_state")){
						Efeitos.Backdrop("fade-in", 255, 5f);

						transition_state++;
					}
				break;

				case 3:
					if(Efeitos.opacidade == 255){
						F.Escrever("Level Completed!", true, (Screen.width/2)-(F.TxtWidth("Level Completed!", 60, true)/2), (Screen.height/2)-200, 60, 255, 255, 255, (byte)txt_op, 000, 000, 47, 255, 1);

						txt_op += 2.5f;
					}
					if(txt_op == 255){
						txt_op = 0;
						transition_state++;
					}
				break;

				case 4:
					if(F.Key("space") && txt_op >= 255){
						level++;

						player1.x = V.multiplayer ? Screen.width*0.25f : (Screen.width/2)-32;
						player1.y = Screen.height+46;

						if(V.multiplayer){
							player2.x = Screen.width*0.75f;
							player2.y = Screen.height+46;
						}

						ResetVars(false);

						F.DesativarTecla("space", 300);
					}
					else{

						F.Escrever("Level Completed!", true, (Screen.width/2)-(F.TxtWidth("Level Completed!", 60, true)/2), (Screen.height/2)-200, 60, 255, 255, 255, 255);

						F.Escrever("Level........................................... "+level, false, (Screen.width/4), (Screen.height/2)-100, 36, 255, 255, 255, (byte)txt_op);
						F.Escrever("Score.......................................... "+score, false, (Screen.width/4), (Screen.height/2)-65, 36, 255, 255, 255, (byte)txt_op);
						F.Escrever("Enemies Killed........................... "+enemies_killed, false, (Screen.width/4), (Screen.height/2)-30, 36, 255, 255, 255, (byte)txt_op);
						F.Escrever("Time Alive................................... "+String.Format("{0}:{1}", (fim-inicio).Minutes, (fim-inicio).Seconds), false, (Screen.width/4), (Screen.height/2)+5, 36, 255, 255, 255, (byte)txt_op);

						F.DesenharShape((Screen.width / 2)-(F.TxtWidth("Continue", 26, true)/2)-6, (Screen.height/2)+103, F.TxtWidth("Continue", 26, true)+16, 30, 255, 255, 255, (byte)txt_op);
						F.Escrever("Continue", true, (Screen.width/2)-F.TxtWidth("Continue", 26, true)/2, (Screen.height/2)+100, 26, 000, 000, 000, (byte)txt_op);

						txt_op += txt_op >= 255 ? 0 : 5;
					}
				break;

			}

		}

		public static void Fim(){

			V.window.SetView(V.hud);
			Efeitos.BackdropRun();

			switch(transition_state){

				case 0:
					F.DesativarTecla("transition_state", 1500);

					V.bullets.Clear();
					V.backdrop = false;
					fim = DateTime.Now;

					transition_state++;
				break;

				case 1:
					if(!F.TeclaDesativada("transition_state")){
						if(!V.multiplayer){
							if(player1.y+player1.height > 0 || player1.x+player1.width/2 != Screen.width/2){
								player1.y -= (player1.y+player1.height > 0 ? (player1.x+player1.width/2 == Screen.width/2 ? player1.speed*2 : player1.speed/2) : 0);
								player1.x += (player1.x+player1.width/2 == Screen.width/2 ? 0 : (player1.x+player1.width/2 > Screen.width/2 ? -player1.speed/2 : player1.speed/2));

								if(Math.Abs(player1.x+player1.width/2 - Screen.width/2) < 5) player1.x = Screen.width/2-player1.width/2;

								break;
							}
						}
						else
							if(player1.y+player1.height > 0 || player2.y+player2.height > 0 || player1.x+player1.width/2 != Screen.width*0.25f || player2.x+player2.width/2 != Screen.width*0.75f){
								player1.y -= (player1.y+player1.height > 0 ? (player1.x+player1.width/2 == Screen.width*0.25f ? player1.speed*2 : player1.speed/2) : 0);
								player1.x += (player1.x+player1.width/2 == Screen.width*0.25f ? 0 : (player1.x+player1.width/2 > Screen.width*0.25f ? -player1.speed/2 : player1.speed/2));

								player2.y -= (player2.y+player2.height > 0 ? (player2.x+player2.width/2 == Screen.width*0.75f ? player2.speed*2 : player2.speed/2) : 0);
								player2.x += (player2.x+player2.width/2 == Screen.width*0.75f ? 0 : (player2.x+player2.width/2 > Screen.width*0.75f ? -player2.speed/2 : player2.speed/2));

								if(Math.Abs(player1.x+player1.width/2 - Screen.width*0.25f) < 5) player1.x = Screen.width*0.25f-player1.width/2;
								if(Math.Abs(player2.x+player2.width/2 - Screen.width*0.75f) < 5) player2.x = Screen.width*0.75f-player2.width/2;

								break;
							}

						F.DesativarTecla("transition_state", 750);
						transition_state++;
					}
				break;

				case 2:
					if(!F.TeclaDesativada("transition_state")){
						Efeitos.Backdrop("fade-in", 255, 5f);

						transition_state++;
					}
				break;

				case 3:
					if(Efeitos.opacidade == 255){
						F.Escrever("Congratulations!", true, (Screen.width/2)-(F.TxtWidth("Congratulations!", 60, true)/2), (Screen.height/2)-200, 60, 255, 255, 255, (byte)txt_op, 000, 000, 47, 255, 1);
						F.Escrever("You have finished the game!", false, (Screen.width/2)-(F.TxtWidth("You have finished the game!!", 32, false)/2), (Screen.height/2)-140, 32, 255, 255, 255, (byte)txt_op, 000, 000, 47, 255, 1);

						txt_op += 2.5f;
					}
					if(txt_op == 255){
						txt_op = 0;
						transition_state++;
					}
				break;

				case 4:
					if(!F.TeclaDesativada("a") && (F.Key("a") || F.Key("d")) && txt_op >= 255){
						gameover_option = !gameover_option;
						F.DesativarTecla("a", 250);
					}

					if(F.Key("space") && txt_op >= 255){
						if(gameover_option){
							level++;

							player1.x = V.multiplayer ? Screen.width*0.25f : (Screen.width/2)-32;
							player1.y = Screen.height+46;

							if(V.multiplayer){
								player2.x = Screen.width*0.75f;
								player2.y = Screen.height+46;
							}

							ResetVars(false);

							F.DesativarTecla("space", 300);
						}
						else
							CurrentScreen.Change("mainmenu");

						F.DesativarTecla("space", 300);
					}
					else{

						F.Escrever("Congratulations!", true, (Screen.width/2)-(F.TxtWidth("Congratulations!", 60, true)/2), (Screen.height/2)-200, 60, 255, 255, 255, (byte)255, 000, 000, 47, 255, 1);
						F.Escrever("You have finished the game!", false, (Screen.width/2)-(F.TxtWidth("You have finished the game!!", 32, false)/2), (Screen.height/2)-140, 32, 255, 255, 255, (byte)255, 000, 000, 47, 255, 1);

						F.Escrever("Level........................................... "+level, false, (Screen.width/4), (Screen.height/2)-90, 36, 255, 255, 255, (byte)txt_op);
						F.Escrever("Score.......................................... "+score, false, (Screen.width/4), (Screen.height/2)-55, 36, 255, 255, 255, (byte)txt_op);
						F.Escrever("Enemies Killed........................... "+enemies_killed, false, (Screen.width/4), (Screen.height/2)-20, 36, 255, 255, 255, (byte)txt_op);
						F.Escrever("Time Alive................................... "+String.Format("{0}:{1}", (fim-inicio).Minutes, (fim-inicio).Seconds), false, (Screen.width/4), (Screen.height/2)+15, 36, 255, 255, 255, (byte)txt_op);

						if(gameover_option){

							F.DesenharShape((Screen.width / 2) - (F.TxtWidth("Main Menu", 26, true))-6, (Screen.height/2)+103, F.TxtWidth("Continue", 26, true)+12, 30, 255, 255, 255, (byte)txt_op);
							F.Escrever("Continue", true, (Screen.width/2)-F.TxtWidth("Continue", 26, true)-20, (Screen.height/2)+100, 26, 000, 000, 000, (byte)txt_op);
							F.Escrever("Main Menu", true, (Screen.width/2)+10, (Screen.height/2)+100, 26, 255, 255, 255, (byte)txt_op);

						}
						else{

							F.DesenharShape((Screen.width / 2) + 4, (Screen.height/2)+103, F.TxtWidth("Main Menu", 26, true)+12, 30, 255, 255, 255, (byte)txt_op);
							F.Escrever("Main Menu", true, (Screen.width/2)+10, (Screen.height/2)+100, 26, 000, 000, 000, (byte)txt_op);
							F.Escrever("Continue", true, (Screen.width/2)-F.TxtWidth("Continue", 26, true)-20, (Screen.height/2)+100, 26, 255, 255, 255, (byte)txt_op);

						}

						txt_op += txt_op >= 255 ? 0 : 5;
					}
				break;

			}

		}

		public static void ResetVars(bool resetgame){

			foreach(Geral x in V.objetos.ToList()) if(x.GetType().Name != "Player" || resetgame)
				V.objetos.RemoveAt(V.objetos.IndexOf(x));

			V.bullets.Clear();
			V.bonus.Clear();

			F.AtivarTecla("state");
			F.AtivarTecla("transition_state");
			F.AtivarTecla("raid");
			F.AtivarTecla("bullet");

			state = state_ = score = raid_state = transition_state = raid_loop = 0;
			start = gameover_option	= true;
			menu = especial			= false;
			txt_op 			= 0;
			enemies_killed 	= 0;
			enemies_alive 	= 0;

			inicio 			= DateTime.Now;
			mY = -1;
			mA = sY = cX = cY = cA 	= 0;

		}

		public static void ResetGame(){

			ResetVars(true);

			V.background.Clear();
			V.limites.Clear();

			level 			= 1;
			especial_count 	= 0;
			
			Idk.windowColor = new Color(000, 000, 000);
			V.view.Center 	= new Vector2f(Screen.width/2, (Screen.height/2)+128);

			player1 	= new Player("player1", V.multiplayer ? Screen.width*0.25f : (Screen.width/2), Screen.height+46, 32, 32);
			V.objetos.Add(player1);

			if(V.multiplayer){
				player2 = new Player("player2", Screen.width*0.75f, Screen.height+46, 32, 32);
				V.objetos.Add(player2);
			}

			V.limites.Add(new Limite(0, 128, Screen.width, 1));
			V.limites.Add(new Limite(0, 128, 1, Screen.height));
			V.limites.Add(new Limite(Screen.width, 128, 1, Screen.height));
			V.limites.Add(new Limite(0, Screen.height+128, Screen.width, 1));

			for(int i=0;i<8;i++)
				for(int j=-1;j<6;j++)
					V.background.Add(new Background((i % 2 == 0 ? 0 : 1), (j % 2 == 0 ? i : i-0.5f), j));

		}
	}
}