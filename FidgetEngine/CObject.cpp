#include "CObject.h"
#include "Engine.h"

void CObject::Draw(class Engine *gameEngine, Transform parentTrans){
	for(ObjLsNode *ptr = myStuff.myHead; ptr; ptr = ptr->nextNode){
		ptr->myObject->Draw(gameEngine, ptr->myObjectTrans+parentTrans);
	}
}