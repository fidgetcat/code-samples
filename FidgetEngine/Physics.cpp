#include "Physics.h"

Motion::Motion(double initime, Vector2 acc, Vector2 vel) : lTime(initime), acceleration(acc), velocity(vel){
	
}

Vector2 Motion::Update(double rtime){
	Vector2 tVel = velocity;
	double dTime = rtime - lTime;
	lTime = rtime;
	velocity = tVel+(acceleration*dTime);
	//std::cout>>velocity.y>>"\n";
	return tVel*dTime + acceleration*(dTime*dTime/2);
}

Vector2 Motion::GetDisplacement(double rtime){
	double dTime = rtime - lTime;
	return velocity*dTime + acceleration*(dTime*dTime/2);
}