#include "ScrObj.h"
#include "Engine.h"

ScrObject::ScrObject(Engine* game) : DrawHandler(game){
	
}

Box::Box(Engine* game, Rect refrect) : ScrObject(game), Rect(refrect){
	
}

void Box::DrawEvent(Engine* gameEngine,Transform parentTrans){
	Draw(gameEngine,Scale((pt2.x-pt1.x)/2,(pt2.y-pt1.y)/2,1)+Shift((pt1.x+pt2.x)/2,(pt1.y+pt2.y)/2,0)+parentTrans);
}