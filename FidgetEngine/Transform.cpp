#include "Transform.h"


Transform::Transform(Vector X, Vector Y, Vector Z, Vector W, Color mCol, Color aCol) : 
x(X), y(Y), z(Z), w(W), mulCol(mCol), addCol(aCol){
	//NOTHING
}

Transform::Transform(const Transform &thing) : 
x(thing.x), y(thing.y), z(thing.z), w(thing.w), addCol(thing.addCol), mulCol(thing.mulCol){
	//NOTHING
	}

Vector Transform::Apply(const Vector &input){
	return x*input.x + y*input.y + z*input.z + w;
}

Color Transform::Apply(const Color &input){
	return input*mulCol+addCol;
}

Transform& Transform::operator =(const Transform &thing){
	x = thing.x;
	y = thing.y;
	z = thing.z;
	w = thing.w;
	addCol = thing.addCol;
	mulCol = thing.mulCol;
	return *this;
}

Transform operator + (const Transform &first, const Transform &second){
	return Transform (
first.x.x*second.x + first.x.y*second.y + first.x.z*second.z,
first.y.x*second.x + first.y.y*second.y + first.y.z*second.z,
first.z.x*second.x + first.z.y*second.y + first.z.z*second.z,
first.w.x*second.x + first.w.y*second.y + first.w.z*second.z + second.w,
first.mulCol*second.mulCol,
first.addCol*second.mulCol+second.addCol
);
}