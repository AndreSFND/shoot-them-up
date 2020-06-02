using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Text.RegularExpressions;

namespace Main{
	public abstract class Screen{
		public static uint width		= 500;
		public static uint height		= 400;
		public static bool fullscreen 	= false;

		public abstract void Draw();
		public abstract void OnExit();
		public abstract void OnStart();
		
		public static void Fullscreen(bool v){
			Screen.fullscreen = v;
			V.window.Close();
			V.window = null;
			
			if(v){
				Screen.width 	= VideoMode.DesktopMode.Width;
				Screen.height 	= VideoMode.DesktopMode.Height;

				V.hud.Size 		= new Vector2f(Screen.width, Screen.height);
				V.view.Size 	= new Vector2f(Screen.width, Screen.height);

				V.hud.Center 	= new Vector2f(Screen.width/2, Screen.height/2);
				V.window 		= new RenderWindow(new VideoMode(Screen.width, Screen.height), "Shoot them up v1.0", Styles.Fullscreen);
			}
			else{
				Screen.width 	= 800;
				Screen.height 	= 600;

				V.hud.Size 		= new Vector2f(Screen.width, Screen.height);
				V.view.Size 	= new Vector2f(Screen.width, Screen.height);
				
				V.hud.Center 	= new Vector2f(Screen.width/2, Screen.height/2);
				V.window 		= new RenderWindow(new VideoMode(Screen.width, Screen.height), "Shoot them up v1.0");
			}
			
			V.window.Closed 	+= new EventHandler(Idk.OnClose);
			V.window.Resized 	+= new EventHandler<SizeEventArgs>(Idk.OnResize);
		}
	}
}