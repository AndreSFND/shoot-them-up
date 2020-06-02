using System;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Main{
	public partial class F{
		public static float catX;
		public static float catY;
		public static float sumHalfHeight;
		public static float sumHalfWidth;
		public static float overlapX;
		public static float overlapY;
		
		public static void ColideBloco(Geral x1, Geral x2, bool ajustarZindex){
			catX  = (x1.x + (x1.width / 2)) - (x2.x + (x2.width / 2));
			catY = (x1.y + (x1.height / 2)) - (x2.y + (x2.height / 2));
									
			sumHalfWidth = (x1.width / 2) + (x2.width / 2);
			sumHalfHeight = (x1.height / 2) + (x2.height / 2);

			if(Math.Abs(catX ) <= sumHalfWidth && Math.Abs(catY) <= sumHalfHeight){
				overlapX = sumHalfWidth - Math.Abs(catX);
				overlapY = sumHalfHeight - Math.Abs(catY);

				if(x2.GetType().Name == "Bullet" && V.bullets.IndexOf((Bullet)x2) != -1 && x2 != null)
					x1.Interagir(x2);
				else if(x2.GetType().Name == "Enemy" && x1.GetType().Name == "Player")
					x1.vidas = x2.vidas = 0;
				else if(x2.GetType().Name == "Bonus")
					x2.Interagir(x1);

				if(overlapX >= overlapY)
					x1.y = (catY > 0) ? x2.y + x2.height : x1.y = x2.y - x1.height;
				else
					x1.x = (catX  > 0 && x1.x + (x1.width / 2) >= x2.x) ? x2.x + x2.width : x2.x - x1.width;
			}
		}
		
	}
}