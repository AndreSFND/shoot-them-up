using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Main{
	public partial class Idk{
		public static Color windowColor;

		[STAThread]
		static void Main() {
			V.window.SetMouseCursorVisible(false);

			V.window.Closed 				+= new EventHandler(OnClose);
			V.window.Resized 				+= new EventHandler<SizeEventArgs>(OnResize);

			V.ImgLoad();
			windowColor = new Color(255, 255, 255);

			CurrentScreen.Add("intro",   	new IntroState());
			CurrentScreen.Add("mainmenu",   new MainMenuState());
			CurrentScreen.Add("instrucoes", new InstrucoesState());
			CurrentScreen.Add("credits", 	new CreditsState());
			CurrentScreen.Add("play", 		new PlayState());

			CurrentScreen.Set("intro");

			Image image = new Image("res/icon.png");
			V.window.SetIcon(image.Size.X, image.Size.Y, image.Pixels);
			V.window.SetFramerateLimit((uint)120);
			
			while (V.window.IsOpen) {

				V.window.DispatchEvents();
				V.window.Clear(windowColor);

					try{

						Draw();
						CurrentScreen.Draw();
						DrawHUD();
					
					}
					catch(Exception e){
					
						Console.WriteLine(e);
					
					}

				V.window.Display();

			}
		}
		
		public static void OnClose(object sender, EventArgs e) {
			RenderWindow window = (RenderWindow)sender;
			window.Close();
		}

		public static void OnResize(object sender, SizeEventArgs e){
			Screen.width 	= e.Width;
			Screen.height 	= e.Height;

			V.hud.Size 				= new Vector2f(e.Width, e.Height);
			V.view.Size 			= new Vector2f(e.Width, e.Height);

			V.hud.Center 			= new Vector2f(e.Width/2, e.Height/2);
		}

	}
}