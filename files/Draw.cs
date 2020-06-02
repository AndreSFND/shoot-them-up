using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Text.RegularExpressions;


namespace Main{
	public partial class Idk{
		public static int fps;
	
		public static void Fps(){
			if(!F.TeclaDesativada("fps")){
				Console.Clear();
				Console.WriteLine("FPS: "+fps);
				fps = 0;
				F.DesativarTecla("fps", 1000);
			}

			fps++;
		}

		public static void Draw(){
			//Fps();
			V.window.SetView(V.view);
		
			F.AtualizarTecla();
			F.TeclasDesativadasF();
		}

		public static void DrawHUD(){	
			V.window.SetView(V.hud);

			if(V.backdrop)
				Efeitos.BackdropRun();
		}
	}
}