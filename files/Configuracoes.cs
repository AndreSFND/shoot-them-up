using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Text.RegularExpressions;

namespace Main{
	public static class Configuracoes{
		public static Playable 	controle;
		public static int 		defaultSpeed	= 1;

		public static void Draw(){

		}
		
		public static void Set(Playable x){
			controle 	= x;
		}

		public static int DefaultSpeed(){
			return defaultSpeed;
		}

	}
}