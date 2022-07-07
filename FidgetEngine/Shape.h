#pragma once

#include "Object.h"
#include "Vector.h"
#include "SplitModel.h"
#include "SplitModelTX.h"
class Triangle : public Object {
	public:
	Triangle();
	~Triangle();
	virtual void Draw(class Engine *gameEngine, Transform parentTrans);
	SplitModel myObj;
	};

/*!
	\class Square
	\brief Low-level Object prefab that draws a simple colored square.
*/

class Square : public Object{
	public:
	Square();
	virtual void Draw(class Engine *gameEngine, Transform parentTrans);
	SplitModel myObj;
};

/*!
	\class FancySquare
	\brief Low-level Object prefab that draws a simple textured square.
*/

class FancySquare : public Object{
	public:
	FancySquare(const char *input);
	virtual void Draw(class Engine *gameEngine, Transform parentTrans);
	SplitModelTX myObj;
};
