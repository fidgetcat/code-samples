#pragma once

#include <stdlib.h>
#include "Handler.h"

struct HandLsNode{
	HandLsNode(Handler *inHandler) : myHandler(inHandler){}
	Handler *myHandler;
	HandLsNode *nextNode;
	HandLsNode *prevNode;
};


class HandlerCollection{
	public:
	HandlerCollection();
	~HandlerCollection();
	
	HandLsNode *myHead;
	HandLsNode *currPtr;
	
	HandLsNode* Add(Handler *inHandler);
	void Remove(HandLsNode *target);
	HandLsNode* GetFirst();
	HandLsNode* GetNext();
};
