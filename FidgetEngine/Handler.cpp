#include "Handler.h"
#include "HandlerCollection.h"
#include "Engine.h"

Handler::Handler(HandlerCollection* input){
	myCollection = NULL;
	myNode = NULL;
	Hold(input);
}

Handler::~Handler(){
	Letgo();
}

void Handler::Letgo(){
	if(myCollection){
		myCollection->Remove(myNode);
		myNode=NULL;
		myCollection=NULL;
	}
}

void Handler::Hold(HandlerCollection *target){
	Letgo();
	myCollection = target;
	myNode = myCollection->Add(this);
}

TimeHandler::TimeHandler(Engine* game) : Handler(&game->timeHandlers){
	
}

NetworkHandler::NetworkHandler(Engine* game) : Handler(&game->networkHandlers){
	
}

DrawHandler::DrawHandler(Engine* game) : Handler(&game->drawHandlers){
	
}

InteractHandler::InteractHandler(Engine* game) : Handler(&game->interactHandlers){
	
}

KeyHandler::KeyHandler(Engine* game) : Handler(&game->keyHandlers){
	
}