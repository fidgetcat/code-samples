#pragma once

#include "Transform.h"
#include "Vector.h"
#include "CObject.h"
#include "Handler.h"

/*!
	\class ScrObject
	\brief Short for ScreenObject, this is a useful base class with a transformation hierarchy and draw functionality
	
	This class is mainly derived from to make other useful bases. CObject provides a transformation hierarchy
	and object collection, while DrawHandler provides rendering functionality.
*/

class ScrObject : public CObject, public DrawHandler{
	public:
	ScrObject(class Engine* game);
};

/*!
	\class Box
	\brief The basic building block for the game.
	
	Combining Screen Object and Rect, this class represents an object that can be drawn on the screen,
	with its reference space being used to both draw and calculate bounding box collisions. Its overload
	of the DrawEvent method stretches its associated Objects to fit its Rect's reference space. (after any other transformations supplied to the object, which allows for "stretching" that doesn't match exactly the Rect's reference space)
*/

class Box : public ScrObject, public Rect{
	public:
	Box(class Engine* game, Rect refrect);
	void DrawEvent(class Engine* gameEngine,Transform parentTrans);
};