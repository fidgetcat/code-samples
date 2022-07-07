#pragma once
/*!
	\class CObject
	\brief COLLECTION OBJECT Basic building block for your game objects. Tiles inherit from this, and most if not all game objects should inherit from this.
	
	 A CObject's (Collection Object) job is to store other objects/CObjects and pass down transformation info down the object chain. They can be used to group different kinds of objects, for example, and easily transform the group as a whole while conforming to parent reference transforms.
*/

#include "Object.h"
#include "Collection.h"

class CObject : public Object{
	public:
	//!Empty constructor.
	CObject(){};
	//!Empty destructor.
	~CObject(){};
	//!The collection of objects associated with this CObject.
	ObjCollection myStuff;
	//!Calls draw on all the objects in the collection, while passing transform information down. The game programmer will rarely (never) explicitly call a specific CObject's Draw since this is mainly for passing transform information down.
	/*!
		\param gameEngine - pointer to engine that will draw this collection
		\param parentTrans - transform information being applied to objects in this collection
	*/
	virtual void Draw(class Engine *gameEngine, Transform parentTrans);
};