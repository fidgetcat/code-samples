#pragma once
/*!
	\struct ObjLsNode
	\brief Basic node used in object lists.
	
	ObjLsNodes (Object list node) populate object collections. They are part of a simple linked list system; they also point to their respective Object and some supplied Transform information.
	
	
*/
#include "Transform.h"
#include "Object.h"

struct ObjLsNode{
	//!Straightforward constructor with init list that simply initializes the supplied arguments.
	ObjLsNode(class Object *inObj, class Transform inTrans) : myObject(inObj), myObjectTrans(inTrans){}
	//!Pointer to object associated with this node.
	Object *myObject;
	//!Pointer to the next node within this node's list.
	ObjLsNode *nextNode;
	//!Pointer to the previous node within this node's list.
	ObjLsNode *prevNode;
	//!Transform information stored when this node is created and/or updated. Usually used to offset an object from its parent transform reference.
	Transform myObjectTrans;
	
};


/*!
	\class ObjCollection
	\brief Container/linked list used to manage Objects. 
	
	Mainly used by CObjects. Manages nodes that store pointer information.
	
*/

class ObjCollection{
	public:
	//!Inits head to null.
	ObjCollection();
	//!Detaches all objects associated with nodes in this collection, therefore depopulating list as well.
	~ObjCollection();
	//!The first node in the list.
	ObjLsNode *myHead;
	//!Removes specified node from the list. This is really only called by Object's Detach().
	void Remove(ObjLsNode *target);
	//!This list adds nodes to the start of the list, not to the end, with the oldest nodes at the end of the list. The pointer returned is used by the Object associated with this node.
	/*!
		\param input - The object associated with this node
		\param inTrans - The transform information to store in the node. Usually this is some offset from the parent's transform.
	*/
	ObjLsNode* Add(Object *input, Transform inTrans);
};