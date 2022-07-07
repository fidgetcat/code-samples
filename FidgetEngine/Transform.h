#pragma once
/*!
	\class Transform
	\brief Class that deals with matrix/vector transformations.
	
	Transform and its children perform matrix and vertex calculations. Most often, game programmers will make use of the various children to quickly create and pass various kinds of transformations.
*/
#include "Vector.h"
#include <math.h>
#include "Color.h"


class Transform{
	public:
	Transform(Vector X, Vector Y, Vector Z, Vector W, Color mCol, Color aCol);
	Transform(const Transform &thing);
	Transform& operator = (const Transform &thing);
	Vector Apply(const Vector &input);
	Color Apply(const Color &input);
	Vector x;
	Vector y;
	Vector z;
	Vector w;
	Color mulCol;
	Color addCol;
};
Transform operator + (const Transform &first, const Transform &second);

class Identity : public Transform {
public:
Identity () : Transform (Vector (1, 0, 0), Vector (0, 1, 0), Vector (0, 0, 1), Vector (0, 0, 0), Color(1,1,1), Color(0,0,0)) {}
};

class Shift : public Transform {
	public:
	Shift(Vector input) : Transform (Vector (1, 0, 0), Vector (0, 1, 0), Vector (0, 0, 1), input, Color(1,1,1), Color(0,0,0)) {}
	Shift(double x, double y, double z) : Transform (Vector (1, 0, 0), Vector (0, 1, 0), Vector (0, 0, 1), Vector(x,y,z), Color(1,1,1), Color(0,0,0)) {}
};

class Scale : public Transform {
	public:
	Scale(Vector input) : Transform (Vector (input.x, 0, 0), Vector (0, input.y, 0), Vector (0, 0, input.z), Vector(0,0,0), Color(1,1,1), Color(0,0,0)) {}
	Scale(double x, double y, double z) : Transform (Vector (x, 0, 0), Vector (0, y, 0), Vector (0, 0, z), Vector(0,0,0), Color(1,1,1), Color(0,0,0)) {}
	
	Scale(double input) : Transform (Vector (input, 0, 0), Vector (0, input, 0), Vector (0, 0, input), Vector(0,0,0), Color(1,1,1), Color(0,0,0)) {}
};

class Rotate : public Transform{
	public:
	Rotate (double angle) : Transform (Vector (cos(angle*M_PI/180),sin(angle*M_PI/180),0), Vector(-sin(angle*M_PI/180),cos(angle*M_PI/180),0), Vector (0, 0, 1), Vector (0, 0, 0), Color(1,1,1), Color(0,0,0)){}
};

class Shade: public Transform{
	public:
	Shade(Color input) : Transform (Vector (1, 0, 0), Vector (0, 1, 0), Vector (0, 0, 1), Vector(0,0,0), input, Color(0,0,0)) {}
	
	Shade(double R, double G, double B) : Transform (Vector (1, 0, 0), Vector (0, 1, 0), Vector (0, 0, 1), Vector(0,0,0), Color(R,G,B), Color(0,0,0)) {}
	
	Shade(double input) : Transform (Vector (1, 0, 0), Vector (0, 1, 0), Vector (0, 0, 1), Vector(0,0,0), Color(input,input,input), Color(0,0,0)) {}
};

class Blend: public Transform{
	public:
	Blend(Color input) : Transform (Vector (1, 0, 0), Vector (0, 1, 0), Vector (0, 0, 1), Vector(0,0,0), Color(1,1,1), input) {}
	
	Blend(double R, double G, double B) : Transform (Vector (1, 0, 0), Vector (0, 1, 0), Vector (0, 0, 1), Vector(0,0,0), Color(1,1,1), Color(R,G,B)) {}
};