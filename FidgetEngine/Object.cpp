#include "Object.h"
#include "CObject.h"
#include <stdlib.h>
Object::Object(){
	myParent = NULL;
	myNode = NULL;
}

void Object::AttachTo(CObject *target, Transform inputTrans){
	
		Detach();
	
	myParent = target;
	myNode = myParent->myStuff.Add(this,inputTrans);
}

void Object::UpdateT(Transform input){
	if(myNode)
	myNode->myObjectTrans = input;
}

void Object::Detach(){
	if(myParent){
	myParent->myStuff.Remove(myNode);
	myNode = NULL;
	myParent = NULL;
	}
}

Object::~Object(){
	Detach();
}