#include "Collection.h"
#include <stdlib.h>

ObjCollection::ObjCollection(){
	myHead = NULL;
}

ObjCollection::~ObjCollection(){
	while(myHead){
	myHead->myObject->Detach();
	}
	
}


ObjLsNode* ObjCollection::Add(class Object *input, class Transform inTrans){
	ObjLsNode *nuNode = new ObjLsNode(input, inTrans);
	nuNode->prevNode = NULL;
	nuNode->nextNode = myHead;
	
	if(myHead){
		myHead->prevNode = nuNode;
		}
		
	myHead=nuNode;
	return nuNode;
}

void ObjCollection::Remove(ObjLsNode *target){
if(target->nextNode)
target->nextNode->prevNode = target->prevNode;
if(target->prevNode)
target->prevNode->nextNode = target->nextNode;
else
myHead = target->nextNode;
delete target;
//SQUANCH OLD OBJECT
}