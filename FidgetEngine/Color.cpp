#include "Color.h"

Color::Color(double R, double G, double B) : 
r(R), g(G), b(B){
	//NOTHING
}

Color::Color(const Color &thing) : 
r(thing.r), g(thing.g), b(thing.b){
	//nothing
}

Color& Color::operator = (const Color &thing){
	r = thing.r;
	g = thing.g;
	b = thing.b;
	return *this;
}

Color operator + (const Color &first, const Color &second){
	return Color(first.r+second.r,first.g+second.g,first.b+second.b);
}

Color operator * (const Color &first, const Color &second){
	return Color(first.r*second.r,first.g*second.g,first.b*second.b);
}