using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Main{
	public class Efeitos{
		public static string 	efeito 		 = "";
		public static float 	opacidade 	 = 0;
		public static float 	opacidade2 	 = 0;
		public static float 	s 			 = 0;
		public static float 	q 			 = 0;
		public static bool		decisao		 = false;

		public static float		cX 			 = 0;
		public static string	word 		 = "";
		public static bool		respawn 	 = false;
		public static bool		allowRespawn = false;

		public static bool 		efeitoTela	 = false;
		public static float		efeitoTelaN  = 0;
		
		public static void Backdrop(string e_, float q_, float s_){
			efeito 	= e_;
			q 		= q_;
			s 		= s_;
		}
		public static void BackdropRun(){
			if(opacidade > 255) opacidade = 255;
			if(opacidade < 0)	opacidade = 0;

			if(efeito == "sem-efeito")
				F.DesenharShape(0, 0, Screen.width, Screen.height, 000, 000, 000, q);

			else if(efeito == "fade-in"){
				if(opacidade < q) opacidade += s;
				if(opacidade >= q) opacidade = q;
				
				F.DesenharShape(0, 0, Screen.width, Screen.height, 000, 000, 000, opacidade);
			}
			else if(efeito == "fade-out"){			
				if(opacidade > q) opacidade -= s;
				if(opacidade <= q) opacidade = q;
				
				F.DesenharShape(0, 0, Screen.width, Screen.height, 000, 000, 000, opacidade);
			}
			else if(efeito == "fade-in-white"){
				if(opacidade < q) opacidade += s;
				if(opacidade >= q) opacidade = q;
				
				F.DesenharShape(0, 0, Screen.width, Screen.height, 255, 255, 255, opacidade);
			}
			else if(efeito == "fade-out-white"){
				if(opacidade > q) opacidade -= s;
				if(opacidade <= q) opacidade = q;
				
				F.DesenharShape(0, 0, Screen.width, Screen.height, 255, 255, 255, opacidade);
			}
		}
		public static void GameOver(Playable x){
			Console.WriteLine("Game Over");
		}

		public static void TremerTela(Geral x){
			V.view.Move(new Vector2f(-10, 0));
		}
	} 


	public class Interacoes{
		
		public static void Andar(Playable x, int dir, float qtd){
			x.direcao 		= dir;
			x.movimentar 	= qtd;
		}

		public static void Movimentar(Playable x){
			if(x.movimentar > 0){

				if(x.direcao == 0)
					x.y += x.speed;
				if(x.direcao == 1)
					x.y -= x.speed;
				if(x.direcao == 2)
					x.x += x.speed;
				if(x.direcao == 3)
					x.x -= x.speed;

				x.movimentar -= x.speed;
				
			}
		}
	}
}