#include "HandlerCollection.h"

HandlerCollection::HandlerCollection(){
	myHead = NULL;
}

HandlerCollection::~HandlerCollection(){
	while(myHead){
	myHead->myHandler->Letgo();
	}
	
}

HandLsNode* HandlerCollection::GetFirst(){
	if(myHead)
	currPtr = myHead->nextNode;
	return myHead;
}

HandLsNode* HandlerCollection::GetNext(){
	HandLsNode* temPtr = currPtr;
	if(currPtr)
	currPtr = currPtr->nextNode;
	return temPtr;
}

HandLsNode* HandlerCollection::Add(Handler *inHandler){
	HandLsNode *nuNode = new HandLsNode(inHandler);
	nuNode->prevNode = NULL;
	nuNode->nextNode = myHead;
	
	if(myHead){
		myHead->prevNode = nuNode;
		}
		
	myHead=nuNode;
	return nuNode;
}

void HandlerCollection::Remove(HandLsNode *target){
	if(target==currPtr){
		GetNext();
	}
	
	
if(target->nextNode)
target->nextNode->prevNode = target->prevNode;
if(target->prevNode)
target->prevNode->nextNode = target->nextNode;
else
myHead = target->nextNode;
delete target;
//SQUANCH OLD HANDLER
}