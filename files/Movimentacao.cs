using System;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Main{
	public partial class F{
		public static List<string> 	tecla 		= new List<string>();
		public static int 			duplo 		= 0;
		public static string 		btn;
		public static DateTime 		n1;


		public static bool Key(string x){
			int keyIndex = tecla.IndexOf(x);

			return keyIndex == -1 ? false : true;
		}

		public static void MovePersonagem(Playable x) {
			if ((Key("up") && x.name == "player2") || (Key("w") && x.name == "player1")){
				x.y -= x.speed;
			}
			if ((Key("down") && x.name == "player2") || (Key("s") && x.name == "player1")){
				x.y += x.speed;
			}
			if ((Key("left") && x.name == "player2") || (Key("a") && x.name == "player1")){
				x.x -= x.speed;
			}
			if ((Key("right") && x.name == "player2") || (Key("d") && x.name == "player1")){
				x.x += x.speed;
			}
		}

		public static void AtualizarTecla(){
			tecla.Clear();

			if (Keyboard.IsKeyPressed((Keyboard.Key)73))
			   	tecla.Add("up");
			if (Keyboard.IsKeyPressed((Keyboard.Key)74))
			   	tecla.Add("down");	
			if (Keyboard.IsKeyPressed((Keyboard.Key)71))
			   	tecla.Add("left");	
			if (Keyboard.IsKeyPressed((Keyboard.Key)72))
			   	tecla.Add("right");
			if (Keyboard.IsKeyPressed((Keyboard.Key)23))
			   	tecla.Add("x");
			if (Keyboard.IsKeyPressed((Keyboard.Key)2))
			   	tecla.Add("c");
			if (Keyboard.IsKeyPressed((Keyboard.Key)36))
				tecla.Add("esc");

			if (Keyboard.IsKeyPressed(Keyboard.Key.W))
				tecla.Add("w");
			if (Keyboard.IsKeyPressed(Keyboard.Key.A))
				tecla.Add("a");
			if (Keyboard.IsKeyPressed(Keyboard.Key.S))
				tecla.Add("s");
			if (Keyboard.IsKeyPressed(Keyboard.Key.D))
				tecla.Add("d");
			if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
				tecla.Add("space");
			if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
				tecla.Add("return");
			if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
				tecla.Add("shift");
			if (Keyboard.IsKeyPressed(Keyboard.Key.RShift))
				tecla.Add("shift");

		}
	}
}