#include "Shape.h"
#include "Engine.h"

Triangle::Triangle(){
	Vector Vec1(-1,-1,0);
	Vector Vec2(1,-1,0);
	Vector Vec3(0,1,0);
	
	GLfloat points[] ={
	Vec1.x, Vec1.y, Vec1.z,
	Vec2.x, Vec2.y, Vec2.z,
	Vec3.x,  Vec3.y, Vec3.z,};
	
	myObj.init(points, sizeof(points));
}

Triangle::~Triangle(){
	
}
	

		
void Triangle::Draw(class Engine *gameEngine, Transform parentTransform){
	Color clr(1,1,1);
	clr = parentTransform.Apply(clr);
	
	/*glm::mat4 transform(parentTransform.x.x,parentTransform.y.x,parentTransform.z.x,parentTransform.w.x,
						parentTransform.x.y,parentTransform.y.y,parentTransform.z.y,parentTransform.w.y,
						parentTransform.x.z,parentTransform.y.z,parentTransform.z.z,parentTransform.w.z,
						0,					0,					0,					1);*/
	
	glm::mat4 transform(parentTransform.x.x,parentTransform.x.y,parentTransform.x.z,0,
						parentTransform.y.x,parentTransform.y.y,parentTransform.y.z,0,
						parentTransform.z.x,parentTransform.z.y,parentTransform.z.z,0,
						parentTransform.w.x,parentTransform.w.y,parentTransform.w.z,1);
	
	gameEngine->shader.bindCamera(gameEngine->myCam.getProjection(),
								  transform,
								  clr.r, clr.g, clr.b);
	
	myObj.draw();
}

Square::Square(){
	Vector Vec1(-1,-1,0);
	Vector Vec2(1,-1,0);
	Vector Vec3(-1,1,0);
	Vector Vec4(1,1,0);
	
	GLfloat points[] ={
	Vec1.x, Vec1.y, Vec1.z,
	Vec2.x, Vec2.y, Vec2.z,
	Vec3.x, Vec3.y, Vec3.z,
	
	Vec4.x, Vec4.y, Vec4.z,
	Vec2.x, Vec2.y, Vec2.z,
	Vec3.x, Vec3.y, Vec3.z
	};
	
    myObj.init(points, sizeof(points));
}
void Square::Draw(class Engine *gameEngine, Transform parentTransform){	
	Color clr(1,1,1);
	clr = parentTransform.Apply(clr);
	
	glm::mat4 transform(parentTransform.x.x,parentTransform.x.y,parentTransform.x.z,0,
						parentTransform.y.x,parentTransform.y.y,parentTransform.y.z,0,
						parentTransform.z.x,parentTransform.z.y,parentTransform.z.z,0,
						parentTransform.w.x,parentTransform.w.y,parentTransform.w.z,1);
	
	gameEngine->shader.bindCamera(gameEngine->myCam.getProjection(),
								  transform,
								  clr.r, clr.g, clr.b);
	
	myObj.draw();
}

FancySquare::FancySquare(const char *input){
	    GLfloat points[] = {
       -1.0f, -1.0f, 0.0f,//lower left
        1.0f, -1.0f, 0.0f,//lower right
       -1.0f,  1.0f, 0.0f,//upper left
        
        1.0f, -1.0f, 0.0f,//lower right
       -1.0f,  1.0f, 0.0f,//upper left
        1.0f,  1.0f, 0.0f,//upper right
    };
    GLfloat texturePoints[] = {
        0.0f,  1.0f,//lower left
        1.0f,  1.0f,//lower right
        0.0f,  0.0f,//upper left
        
        1.0f,  1.0f,//lower right
        0.0f,  0.0f,//upper left
        1.0f,  0.0f,//upper right
    };
	
    myObj.init(points, texturePoints, sizeof(points), input);
}

void FancySquare::Draw(class Engine *gameEngine, Transform parentTransform){
	glm::vec3 mulc(parentTransform.mulCol.r,parentTransform.mulCol.g,parentTransform.mulCol.b);
	glm::vec3 addc(parentTransform.addCol.r,parentTransform.addCol.g,parentTransform.addCol.b);
	
	
	glm::mat4 transform(parentTransform.x.x,parentTransform.x.y,parentTransform.x.z,0,
						parentTransform.y.x,parentTransform.y.y,parentTransform.y.z,0,
						parentTransform.z.x,parentTransform.z.y,parentTransform.z.z,0,
						parentTransform.w.x,parentTransform.w.y,parentTransform.w.z,1);
	
	gameEngine->TXshader.bindCamera(gameEngine->myCam.getProjection(),
								  transform,
								  mulc,
								  addc);
	
	myObj.draw();
}

/*
glm::mat4 camera, float rin, float gin, float bin
std::string vs = "basicShader.vs";
std::string fs = "basicShader.fs";
shader.init(vs, fs);
shader.bind(myObj.getTransformationmyObj(),r,g,b);
shader.bindCamera(camera, myObj.getTransformationmyObj(),r,g,b);
*/