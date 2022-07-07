/*
Linux
g++ AbsObject.cpp Camera.cpp BaseEngine.cpp main.cpp -o run.exe -lGL -lGLEW -lsfml-graphics -lsfml-window -lsfml-system && ./run.exe

Windows
g++ Actor.cpp HandlerCollection.cpp Handler.cpp Color.cpp Collection.cpp AbsObject.cpp Camera.cpp Engine.cpp main.cpp Object.cpp CObject.cpp Transform.cpp Shape.cpp Vector.cpp -ggdb -o run.exe -IC:/glew/include -LC:/glew/lib -IC:/sfml/include -LC:/sfml/lib -IC:/glm -lsfml-window -lsfml-network -lsfml-graphics -lsfml-system -lglew32 -lopengl32 -lglu32 && run.exe

g++ *.cpp -o run.exe -IC:/glew/include -LC:/glew/lib -IC:/sfml/include -LC:/sfml/lib -IC:/glm -lsfml-window -lsfml-graphics -lsfml-system -lsfml-network -lglew32 -lopengl32 -lglu32 && run.exe

Start Window
*/
#include <iostream>
#include <SFML/Window.hpp>
#include "Engine.h"
#include "Shape.h"
#include "Transform.h"
#include "CObject.h"
#include "Handler.h"
#include "ScrObj.h"
#include <math.h>
#include "Vector.h"
#include "Physics.h"
#include "Network.h"

const double GRAV = -10;

class Squanch : public Box, public TimeHandler{
	public:
	Triangle sQuanch;
	Square sqUanch;
	FancySquare bloob;
	Squanch(Engine* game) : Box(game, Rect(Vector2(-1,-1),Vector2(1,1))),
							TimeHandler(game),
							bloob("./jtalidle.png"){
		//sQuanch.AttachTo(this,Shade(1,0.01,0.01)+Scale(0.5,0.5,1));
		//sqUanch.AttachTo(this, Shade(0.01,1,0.01)+Scale(0.5,0.5,1)+Shift(-0.6,-0.6,0));
		//bloob.AttachTo(this,Scale(0.7,0.5,1)+Shade(1,1,1));
		bloob.AttachTo(this,Shade(0.1,0.1,0.1));
	}
	void TimeEvent(double time){
		//Displace(Vector2(0.001,0.001));
	}
};


class Interactor : public Box{
	public:
	Engine *gameptr;
	Interactor(Engine* game, Rect refrect) : Box(game, refrect){
	gameptr = game;
	}
	virtual void InteractPlayer(class Player *other){}
	virtual void InteractPlatform(class Platform *other){}
};



class Lifebar : public Box{
	public:
	Square squanch;
	Lifebar(Engine* game, Rect refrect) : Box(game, refrect){
		squanch.AttachTo(this,Identity());
		SetHealth(1);
	}
	void SetHealth(double input){
		Transform trans = Identity();
		if(input>=0.51){
			trans = Shade(0.1,1,0.3);
		}
		else if(input>=0.26){
			trans = Shade(0.8,0.8,0.2);
		}
		else{
			trans = Shade(1,0.3,0.1);
		}
		squanch.UpdateT(Shift(1,0,0)+Scale(input,1,1)+Shift(-1,0,0)+trans);
	}
};

class Platform: public Box, public InteractHandler{
	public:
	Square squanch;	
	Platform(Engine* game, Rect refrect) : Box(game,refrect), InteractHandler(game){
		squanch.AttachTo(this,Shade(0.6,0.6,0.6));
	}
	void InteractEvent(class ScrObject* obj){
		((Interactor *)obj)->InteractPlatform(this);
	}
};

class Hitbox : public Interactor{
	public:
	class Player *me;
	Hitbox(Engine* game, Rect refrect, class Player *in) : Interactor(game, refrect), me(in){
	}
	void InteractPlayer(class Player *other);

};

class Player : public Interactor, public InteractHandler,
			   public KeyHandler, public TimeHandler, public Motion, public NetworkHandler{
	public:
	FancySquare idleSpr;
	FancySquare atkSpr;
	Lifebar guts;
	int playerID;
	bool inAir;
	bool facingRight;
	bool inAtk;
	double atkTimer;
	double atkCD;
	double invulTimer;
	double invulCD;
	
	double atkWidth;
	double atkHeight;
	double yAdjust;
	
	double health;
	
	Player(Engine* game, Rect refrect, const char *input, const char *input2, int ID) : Interactor(game,refrect), 
	InteractHandler(game), idleSpr(input), playerID(ID),
	Motion(game->GetTime(),Vector2(0,GRAV),Vector2(0,0)),
	TimeHandler(game),
	KeyHandler(game), atkSpr(input2),
	guts(game,ID==1 ? Rect(Vector2(-1.7,1),Vector2(-0.4,1.3)):Rect(Vector2(0.4,1),Vector2(1.7,1.3))),
	NetworkHandler(game){
		if(playerID==1){
			atkWidth = 1.6;
			atkHeight = 1.2;
			yAdjust = 0.12;
		}
		else{
			atkWidth = 1.7;
			atkHeight = 1.1;
			yAdjust = 0.11;
		}
		atkCD = 0.3;
		inAtk = false;
		inAir = true;
		facingRight = true;
		if(playerID==2)
			facingRight=false;
		health = 1;
		SpriteProc();
	}
	
	void SpriteProc(){
		Transform trans = Identity();
		if(facingRight)
			trans = Scale(1,1,1);
		else
			trans = Scale(-1,1,1);
		if(inAtk){
			idleSpr.Detach();
			atkSpr.AttachTo(this,Shift(0,yAdjust,0)+trans+Scale(atkWidth,atkHeight,1));
		}
		else{
			atkSpr.Detach();
			idleSpr.AttachTo(this,trans);
		}
	}
	
	void TakeDamage(){
		health-=0.25;
		if(health<=0)
			health = 0;
		guts.SetHealth(health);
	}
	
	void InteractEvent(class ScrObject* obj){
		((Interactor *)obj)->InteractPlayer(this);
	}
	void InteractPlayer(class Player *other){
		if(other == this)
			return;
		
		
	}
	void InteractPlatform(class Platform *other){
		if(CheckOverlap(*other)){
			inAir = false;
			velocity.y = 0;
			Displace(Vector2(0,other->pt2.y-pt1.y));
			acceleration.y = 0;
	}
	}
	
	void Jump(){
		if(!inAir){
		inAir = true;
		velocity.y = 4.1;
		acceleration.y = GRAV;
	}
	}
	
	void Attack(){
		Vector2 size(0.55, 0.18);
		Vector2 displace(0.26, 0.1);
		if(!facingRight)
			displace.x = -displace.x;
		Hitbox box = Hitbox(gameptr,Rect(GetCenter()+displace+size*-0.5,GetCenter()+displace+size*0.5),this);
		gameptr->Interact(&box);
		//jtal: 0.7, 0.55
	}
	
	void KeyDnEvent(Key::Keycode keyIn,bool alt, bool control, bool shift, bool system){
		if(playerID==1){
		if(keyIn==Key::Up){
			Jump();
		}
		
		if(keyIn==Key::Down){
		}
		
		if(keyIn==Key::Left){
			facingRight = false;
			velocity.x = -2;
			SpriteProc();
		}
		
		if(keyIn==Key::Right){
			facingRight = true;
			velocity.x = 2;
			SpriteProc();
		}
		
		if(keyIn==Key::Space){
			if(inAtk){
				//
			}
			else{
				atkTimer = gameptr->GetTime();
				inAtk = true;
				SpriteProc();
				Attack();
				
			}
		}
		}
		else if(playerID==2){
			if(keyIn==Key::W){
				Jump();
			}
			if(keyIn==Key::A){
				facingRight = false;
				velocity.x = -2;
				SpriteProc();
			}
			if(keyIn==Key::S){
				
			}
			if(keyIn==Key::D){
				facingRight = true;
				velocity.x = 2;
				SpriteProc();
			}
			if(keyIn==Key::R){
				if(inAtk){
				//
				}
			else{
				atkTimer = gameptr->GetTime();
				inAtk = true;
				SpriteProc();
				Attack();
				}
				
			}
		}
	}
	
	void TimeEvent(double currTime){
		if(inAtk&&currTime-atkTimer>=atkCD){
			inAtk = false;
			SpriteProc();
		}
		Displace(Update(currTime));
		gameptr->Interact(this);
	}
	
	void KeyUpEvent(Key::Keycode keyIn,bool alt, bool control, bool shift, bool system){
		if(playerID==1){
			if(keyIn==Key::Left){
			velocity.x = 0;
		}
		if(keyIn==Key::Right){
			velocity.x = 0;
		}
		}
		else if(playerID==2){
		if(keyIn==Key::A){
			velocity.x = 0;
		}
		if(keyIn==Key::D){
			velocity.x = 0;
		}
		}
	}
	
	void UdpEvent(class Packet* pktin, class IPadd addin, unsigned short portin){
		
	}
	
  };

	void Hitbox::InteractPlayer(class Player *other){
		if(CheckOverlap(*other)){
			if(me!=other)
			other->TakeDamage();
		}
	}
	
int main(){
	Engine game;
	//Squanch squanchy(&game);
	game.UdpBind(52000);
	Platform platty(&game,Rect(Vector2(-3,-1.3),Vector2(3,-1)));
	Player jtal(&game,Rect(Vector2(0,0),Vector2(0.7,0.55)),"./jtalidle.png","./jtalstHK.png",1);
	jtal.Place(Vector2(-1.2,0.5));
	Player jeda(&game,Rect(Vector2(-0.8,0),Vector2(-0.16,0.77)),"./jedahidle.png","./jedahstHP.png",2);
	jeda.Place(Vector2(1.2,0.5));
	game.Run();
	

	return 0;
}
//end func