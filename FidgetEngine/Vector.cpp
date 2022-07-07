#include "Vector.h" 

Vector::Vector(double X, double Y, double Z) : 
x(X), y(Y), z(Z){
	//NOTHING
}

Vector::Vector(const Vector &thing) : 
x(thing.x), y(thing.y), z(thing.z){
	//nothing
}

Vector& Vector::operator = (const Vector &thing){
	x = thing.x;
	y = thing.y;
	z = thing.z;
	return *this;
}

Vector operator + (const Vector &first, const Vector &second){
	return Vector(first.x+second.x,first.y+second.y,first.z+second.z);
}

Vector operator * (double multi, const Vector &targetVec){
	return Vector(targetVec.x*multi,targetVec.y*multi,targetVec.z*multi);
}

Vector operator * (const Vector &targetVec, double multi){
	return Vector(targetVec.x*multi,targetVec.y*multi,targetVec.z*multi);
}

Vector2::Vector2(double X, double Y) : 
x(X), y(Y){
	//NOTHING
}

Vector2::Vector2(const Vector2 &thing) : 
x(thing.x), y(thing.y){
	//nothing
}

Vector2& Vector2::operator = (const Vector2 &thing){
	x = thing.x;
	y = thing.y;
	return *this;
}

Vector2 operator + (const Vector2 &first, const Vector2 &second){
	return Vector2(first.x+second.x,first.y+second.y);
}

Vector2 operator - (const Vector2 &first, const Vector2 &second){
	return Vector2(first.x-second.x,first.y-second.y);
}

Vector2 operator * (double multi, const Vector2 &targetVec){
	return Vector2(targetVec.x*multi,targetVec.y*multi);
}

Vector2 operator * (const Vector2 &targetVec, double multi){
	return Vector2(targetVec.x*multi,targetVec.y*multi);
}

Rect::Rect(Vector2 p1, Vector2 p2) : pt1(p1), pt2(p2){
	
}

double Rect::GetWidth(){
	return pt2.x-pt1.x;
}

double Rect::GetHeight(){
	return pt2.y-pt1.y;
}

bool Rect::CheckOverlap(Rect otherBox){
	return pt2.x>=otherBox.pt1.x&&pt1.x<=otherBox.pt2.x&&pt2.y>=otherBox.pt1.y&&pt1.y<=otherBox.pt2.y;
}

Vector2 Rect::GetCenter(){
	return (pt1+pt2)*0.5; 
}

void Rect::Displace(Vector2 inputvec){
	pt1 = pt1+inputvec;
	pt2 = pt2+inputvec;
}

void Rect::Place(Vector2 inputvec){
	Vector2 dist1 = pt1-GetCenter();
	Vector2 dist2 = pt2-GetCenter();
	
	Vector2 ptt1 = inputvec+dist1;
	Vector2 ptt2 = inputvec+dist2;
	pt1 = ptt1;
	pt2 = ptt2;
}

Rect::Rect(const Rect &thing) : pt1(thing.pt1), pt2(thing.pt2){
	
}

Rect Rect::operator + (const Vector2 &vecin){
	return Rect(pt1+vecin,pt2+vecin);
}

Rect& Rect::operator = (const Rect &thing){
	pt1 = thing.pt1;
	pt2 = thing.pt2;
	return *this;
}