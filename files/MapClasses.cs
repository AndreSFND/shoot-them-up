using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Main{

	public abstract class Geral{
		public float vida_				= 100;

		public string name 				= "";
		public int frame				= 0;	
		public int direcao				= 0;
		public int forca 				= 0;
		public int vidas 				= 0;
		public int speed				= 0;
		public float x					= 0;	
		public float y					= 0;	
		public float width				= 0;	
		public float height				= 0;
		
		public abstract void Reset();
		public abstract void Destroy();
		public abstract void Draw();
		public abstract void Interagir(Geral p);
	}

	public abstract class Playable : Geral{
		public float movimentar		= 0;
		public bool menu			= false;
		public int 	menuType 		= 0;

		public Playable(){
			
		}

		public abstract void OnDie();
		public override void Reset(){}
		public override void Destroy(){}
		public override void Draw(){}
		public override void Interagir(Geral p){}

	}

	public class NonPlayable : Geral{
		public override void Reset(){}
		public override void Destroy(){}
		public override void Draw(){}
		public override void Interagir(Geral p){}
	}
	
	public class Player : Playable{

		public Player(string name, float x, float y, float width, float height){
			this.name 		= name;
			this.x 			= x-16;
			this.y 			= y;
			this.height 	= height;
			this.width 		= width;
			this.direcao 	= 0;
			this.movimentar = 0;
			this.speed 		= 2;
			this.forca 		= 1;
			this.vidas 		= 1;
			this.menu 		= false;
			this.frame 		= -1;
		}

		public override void Draw(){
			if(vidas <= 0)
				OnDie();
			else{

				foreach(Geral o in V.objetos.ToList()) if(o != this && o.vidas > 0 && !(o.GetType().Name == "Player"))
					F.ColideBloco(this, o, true);

				if(PlayState.state == 2)
					F.MovePersonagem(this);

				Interacoes.Movimentar(this);
				
				int sprite = this.name == "player1" ? 0 : (this.name == "player2" ? 1 : 2);
				
				V.img[0].Texture 		= V.IMG_CAT[0][sprite];

				V.img[0].Position 		= new Vector2f(x, y);
				V.img[0].TextureRect	= new IntRect(0, 0, (int)width, (int)height);

				V.window.Draw(V.img[0]);

				if((F.Key("return") && this.name == "player2") || (F.Key("space") && this.name == "player1")){

					if(!F.TeclaDesativada("bullet_"+this.name) && PlayState.state == 2){

						if(this.vidas == 1 || this.vidas == 4)
							V.bullets.Add(new Bullet(this.x+11, this.y-4, this.forca, 0, 0, sprite));

						else if(this.vidas == 2 || this.vidas == 5){
							V.bullets.Add(new Bullet(this.x+22, this.y-4, this.forca, 0, 0, sprite));
							V.bullets.Add(new Bullet(this.x, this.y-4, this.forca, 0, 0, sprite));
						}

						if(this.vidas == 3 || this.vidas == 6){
							V.bullets.Add(new Bullet(this.x+22, this.y-4, this.forca, 1, 0, sprite));
							V.bullets.Add(new Bullet(this.x+11, this.y-4, this.forca, 0, 0, sprite));
							V.bullets.Add(new Bullet(this.x, this.y-4, this.forca, 2, 0, sprite));
						}
		
						F.DesativarTecla("bullet_"+this.name, this.vidas <= 3 ? 500 : 250); 

					}
				}

				if(this.name != "player1" && this.name != "player2" && PlayState.state == 2){

					if(this.y+this.height < 128)
						V.objetos.RemoveAt(V.objetos.IndexOf(this));

					if(!F.TeclaDesativada("bullet_"+this.name) && PlayState.state == 2){

						V.bullets.Add(new Bullet(this.x+22, this.y-4, this.forca, 0, 0, sprite));
						V.bullets.Add(new Bullet(this.x, this.y-4, this.forca, 0, 0, sprite));

						F.DesativarTecla("bullet_"+this.name, 1000); 

					}

					this.y -= 2;

				}

			}
		}

		public override void Interagir(Geral p){

			this.vidas -= p.forca;
			V.bullets.RemoveAt(V.bullets.IndexOf((Bullet)p));

		}

		public override void OnDie(){
			Geral p1 = PlayState.player1, p2 = PlayState.player2;

			if((V.multiplayer && p1.vidas <= 0 && p2.vidas <= 0) || (!V.multiplayer && p1.vidas <= 0))
				PlayState.state = 3;
		}
	}

	public class Enemy : Playable{
		public int 	id 				= 0;
		public int 	sprite 			= 0;
		public int 	protocolo		= 0;
		public int 	protocolo_state	= 0;
		public int 	score			= 0;
		public bool	bonus			= false;
			
		public Enemy(int id, float x, int sprite, int protocolo, bool bonus, int score, int vidas, int forca, int speed, float width, float height){
			this.id 		= id;
			this.x 			= x-(width/2);
			this.y 			= -50;
			this.sprite 	= sprite;
			this.bonus 		= bonus;
			this.score 		= score;
			this.protocolo 	= protocolo;
			this.vidas 		= vidas;
			this.forca 		= forca;
			this.speed 		= speed;
			this.width 		= width;
			this.height 	= height;

			PlayState.enemies_alive++;
			V.objetos.Add(this);
		}

		public override void OnDie(){
			PlayState.score 			+= this.score;
			PlayState.especial_count 	+= this.score/10;
			PlayState.enemies_killed 	+= 1;

			if(this.bonus)
				new Bonus(this.x, this.y);

			this.Destroy();
		}

		public override void Destroy(){
			PlayState.enemies_alive--;
			V.objetos.RemoveAt(V.objetos.IndexOf(this));
		}

		public override void Draw(){
			if(this.x+this.width < 0 || this.x > Screen.width || this.y > Screen.height+128)
				this.Destroy();

			if(vidas <= 0)
				OnDie();
			else{

				if(PlayState.state == 2){
					switch(protocolo){

						case 0:

							switch(this.protocolo_state){

								case 0:

									this.y += this.speed;
									if(this.y == 228){ this.protocolo_state++; F.DesativarTecla("protocolo_state_"+this.id, 3000);}

								break;

								case 1:

									if(!F.TeclaDesativada("protocolo_state_"+this.id))
										this.y += this.speed;

								break;

							}

						break;

						case 1:

							if(this.y < 228  || this.x+this.width/2 == Screen.width*0.75f)this.y += this.speed;
							if(this.y == 228 && this.x+this.width/2 <  Screen.width*0.75f)this.x += this.speed;

						break;

						case 2:

							if(this.y < 228  || this.x+this.width/2 == Screen.width*0.25f)this.y += this.speed;
							if(this.y == 228 && this.x+this.width/2 >  Screen.width*0.25f)this.x -= this.speed;

						break;

						case 3:

							this.x += (this.y < 128 ? 0 : this.speed);
							this.y += (this.y < 528 ? this.speed : 0);

						break;

						case 4:

							this.x -= (this.y < 128 ? 0 : this.speed);
							this.y += (this.y < 528 ? this.speed : 0);

						break;

					}

					if(!F.TeclaDesativada("bullet_e"+this.id) && PlayState.state == 2){

						if(this.vidas == 1 || this.vidas == 4)
							V.bullets.Add(new Bullet(this.x+11, this.y+this.height+4, this.forca, 0, 1, this.sprite));

						else if(this.vidas == 2 || this.vidas == 5){
							V.bullets.Add(new Bullet(this.x+22, this.y+this.height+4, this.forca, 0, 1, this.sprite));
							V.bullets.Add(new Bullet(this.x, this.y+this.height+4, this.forca, 0, 1, this.sprite));
						}

						if(this.vidas == 3 || this.vidas == 6){
							V.bullets.Add(new Bullet(this.x+22, this.y+this.height+4, this.forca, 1, 1, this.sprite));
							V.bullets.Add(new Bullet(this.x+11, this.y+this.height+4, this.forca, 0, 1, this.sprite));
							V.bullets.Add(new Bullet(this.x, this.y+this.height+4, this.forca, 2, 1, this.sprite));
						}

						F.DesativarTecla("bullet_e"+this.id, 1000); 

					}
				
					Interacoes.Movimentar(this);
				}
				
				V.img[0].Texture 		= V.IMG_CAT[3][sprite];
				V.img[0].TextureRect	= new IntRect(0, 0, (int)width, (int)height);
				V.img[0].Position 		= new Vector2f(x, y);
				V.window.Draw(V.img[0]);

			}
		}

		public override void Interagir(Geral p){

			this.vidas -= p.forca;
			V.bullets.RemoveAt(V.bullets.IndexOf((Bullet)p));
			
		}
	}

	public class Bullet : NonPlayable{
		public int tipo 		= 0;
		public int sprite 		= 0;
		public int protocolo 	= 0;

		public Bullet(float x, float y, int forca, int protocolo, int tipo, int sprite){
			this.x 			= x;
			this.y 			= y;
			this.forca 		= forca;
			this.protocolo 	= protocolo;
			this.sprite 	= sprite;
			this.tipo 		= tipo;
		}

		public override void Draw(){
			if(this.x+this.width < 0 || this.x > Screen.width || this.y+this.height < 128 || this.y > Screen.height+128)
				V.bullets.RemoveAt(V.bullets.IndexOf(this));

			foreach(Geral o in V.objetos.ToList()) if(o is Playable)
				if(!(this.tipo == 0 && o.GetType().Name == "Player") && !(this.tipo == 1 && o.GetType().Name == "Enemy"))
					if(o.GetType().Name != "Player" || (o.GetType().Name == "Player" && (o.name == "player1" || o.name == "player2")))
						F.ColideBloco(o, this, false);

			if(PlayState.state == 2){
				this.y += (this.tipo == 1 ? 5 : -5);
				this.x += (this.protocolo == 0 ? 0 : (this.protocolo == 1 ? 1 : -1));
			}

			V.img[0].Texture = V.IMG_CAT[this.tipo+5][this.sprite];

			V.img[0].Position 		= new Vector2f(x, y);
			V.img[0].TextureRect	= new IntRect(0, 0, (int)10, (int)10);

			V.window.Draw(V.img[0]);
		}
		public override void Interagir(Geral p){

			V.bullets.RemoveAt(V.bullets.IndexOf((Bullet)p));
			V.bullets.RemoveAt(V.bullets.IndexOf(this));

		}
	}

	public class Bonus : NonPlayable{
		public int tipo = 0;

		public Bonus(float x, float y){
			this.x 		= x;
			this.y 		= y;

			this.width 	= 26;
			this.height = 26;

			this.tipo 	= (int)V.random.Next(0, V.multiplayer ? 3 : 2);

			V.bonus.Add(this);
		}

		public override void Draw(){
			if(this.x+this.width < 0 || this.x > Screen.width || this.y+this.height < 128 || this.y > Screen.height+128)
				V.bonus.RemoveAt(V.bonus.IndexOf(this));

			foreach(Geral o in V.objetos.ToList()) if(o is Playable && (o.GetType().Name == "Player" && (o.name == "player1" || o.name == "player2")))
				F.ColideBloco(o, this, false);

			if(PlayState.state == 2)
				this.y += 1;

			V.img[0].Texture = V.IMG_CAT[4][this.tipo];

			V.img[0].Position 		= new Vector2f(x, y);
			V.img[0].TextureRect	= new IntRect(0, 0, (int)26, (int)26);

			V.window.Draw(V.img[0]);
		}
		public override void Interagir(Geral p){

			Geral p1 = PlayState.player1, p2 = PlayState.player2;

			if(this.tipo == 0 && p.vidas < 6)
				p.vidas++;
			else if(this.tipo == 1 && p.speed < 4)
				p.speed++;

			if(V.multiplayer){
				if(this.tipo == 2 && p == p1 && p2.vidas <= 0)
					p2.vidas = 1;
				else if(this.tipo == 2 && p == p2 && p1.vidas <= 0)
					p1.vidas = 1;
			}

			V.bonus.RemoveAt(V.bonus.IndexOf(this));

		}
	}
	
	public class Limite : NonPlayable{
		public Limite(float x, float y, float width, float height){
			this.x 		= x;
			this.y 		= y;
			this.width 	= width;
			this.height = height;
		}

		public override void Draw(){
			foreach(Geral o in V.objetos.ToList()) 
				if(((o is Playable && o.GetType().Name != "Enemy") || (o.GetType().Name == "Player")) && PlayState.state == 2)
					if(o.GetType().Name != "Player" || (o.GetType().Name == "Player" && (o.name == "player1" || o.name == "player2")) )
						F.ColideBloco(o, this, false);
		}
		public override void Interagir(Geral p){

		}
	}
	
	public class Background{
		public int sprite		= 0;
		public float x			= 0;
		public float y			= 0;
		public float y_ 		= 0;
		public float width		= 0;
		public float height		= 0;
	
		public Background(int t, float cX, float cY){
			this.sprite		= t;
			this.width 		= (float)V.IMG_CAT[2][sprite].Size.X;
			this.height 	= (float)V.IMG_CAT[2][sprite].Size.Y;
			this.x			= (float)cX*width;
			this.y			= (float)cY*height;
			this.y_			= (float)cY*height;
		}

		public void Draw(){
			V.img[0].Position 		= new Vector2f(x, y);
			V.img[0].Texture 		= V.IMG_CAT[2][sprite];
			V.img[0].TextureRect	= new IntRect(0, 0, (int)width, (int)height);
			V.window.Draw(V.img[0]);

			this.y += 4;
			if(this.y-this.y_ >= 256)
				this.y = this.y_;
		}
	}
	
	public class TeclaDesativada{
		public string keyCode;
		public int ms;
		public DateTime inicio 	= DateTime.Now;

		public TeclaDesativada(string k, int mls){
			keyCode = k;
			ms 		= mls;
		}
	}

	public class Controle{
		public int id;
		public int keycode;
		public string nome;

		public Controle(int id, string nome, int keycode){
			this.id 		= id;
			this.nome 		= nome;
			this.keycode 	= keycode;
		}
	}

}