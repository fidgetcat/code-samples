#pragma once

class Vector{
	public:
	Vector(double X, double Y, double Z);
	Vector(const Vector &thing);
	Vector& operator = (const Vector &thing);
	double x;
	double y;
	double z;
};
Vector operator + (const Vector &first, const Vector &second);

Vector operator * (double multi, const Vector &targetVec);

Vector operator * (const Vector &targetVec, double multi);

class Vector2{
	public:
	Vector2(double X, double Y);
	Vector2(const Vector2 &thing);
	Vector2& operator = (const Vector2 &thing);
	double x;
	double y;
};

/*!
	\class Rect
	\brief Class used for making boxes used in collisions and Screen Objects
	
	This class is mainly used as a reference space for Screen Objects. It defines a box with many useful methods
	good for checking collisions and moving the box around.
*/

class Rect{
	public:
	//! Constructor that accepts two points.
	/*!
		\param p1 - The first point must always be the lower left point of the rect.
		\param p2 - The second point must always be the top right point of the rect.
	*/
	Rect(Vector2 p1, Vector2 p2);
	//! Copy constructor.
	Rect(const Rect &thing);
	Vector2 pt1;
	Vector2 pt2;
	Rect operator + (const Vector2 &vecin);
	Rect& operator = (const Rect &thing);
	Vector2 GetCenter();
	//! Checks if an overlap occurs with the argument.
	bool CheckOverlap(Rect otherBox);
	double GetHeight();
	double GetWidth();
	//! Displaces the rect by the supplied vector.
	void Displace(Vector2 inputvec);
	//! Places the rect (centered) at the supplied point.
	void Place(Vector2 inputvec);
};

Vector2 operator + (const Vector2 &first, const Vector2 &second);

Vector2 operator - (const Vector2 &first, const Vector2 &second);

Vector2 operator * (double multi, const Vector2 &targetVec);

Vector2 operator * (const Vector2 &targetVec, double multi);