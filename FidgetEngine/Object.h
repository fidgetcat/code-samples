#pragma once

/*!
	\class Object
	\brief Base class for almost all rendered entities in the engine
	
	Many children inherit from this base class. It provides basic functionality for attaching to CObjects and drawing. Since it in always assumed that an Object has a parent, and since object parents are collections, this makes it easy to make a long hierarchy of game objects. This is particularly useful for passing down transformation info. This also means that rendering occurs according to a parent-child hierarchy.
*/

#include "Transform.h"
#include "Vector.h"
 
class Object{
	public:
	//! Simple constructor that sets parent and node to null.
	Object();
	//! The destructor simply calls detach.
	~Object();
	//! This is the object's parent. Normally, game objects are not drawn if they don't have a parent
	class CObject *myParent;
	//! This points to the node that this object is stored in. This information is used to update the list that the object resides in. It can also be used to change the object's transform.
	struct ObjLsNode *myNode;
	
	//!Virtual function meant to be implemented by children. Some objects use Draw to pass information, delegating the actual drawing to the objects at the end of the parent-child hierarchy
	/*!
		\param gameEngine - Pointer to the game engine that will render this
		\param parentTrans - Transform being passed to the object. With long chains of object children/parents, transforms can be cumulative.
	*/
	virtual void Draw(class Engine *gameEngine, Transform parentTrans) = 0;
	//!Function to attach objects (whether CObject or otherwise) to CObjects. This will create a new node and place it in the CObject's list, which is appropriately updated.
	/*!
		\param	target - Pointer to the CObject to be attached to
		\param inputTrans - In AttachTo, the input transform can be thought of as an offset or difference (using the parent transform as reference). It is stored in the object's node and later added to the parent's transform.
	*/
	void AttachTo(class CObject *target, Transform inputTrans);
	//!Removes this object's node from its parent's collection, then sets its node and parent to null.
	void Detach();
	//!Changes the transform information stored in the object's node (if one exists). Note that this overwrites the old transform information if it is not explicitly added to the old one.
	void UpdateT(Transform input);
};
