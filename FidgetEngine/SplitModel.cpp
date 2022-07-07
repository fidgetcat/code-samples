#include "SplitModel.h"

const glm::vec3 SplitModel::X_ROT_MATRIX = glm::vec3(1.0, 0.0, 0.0);
const glm::vec3 SplitModel::Y_ROT_MATRIX = glm::vec3(0.0, 1.0, 0.0);
const glm::vec3 SplitModel::Z_ROT_MATRIX = glm::vec3(0.0, 0.0, 1.0);

SplitModel::SplitModel(){
}

SplitModel::SplitModel(GLfloat points[],int size){
    init(points,size);
}

void SplitModel::init(GLfloat points[],int size){
    glGenVertexArrays(1, &vertexArrayID);
    glBindVertexArray(vertexArrayID);
    
    glGenBuffers(1, &positionBuffer);
    glBindBuffer(GL_ARRAY_BUFFER, positionBuffer);
    glBufferData(GL_ARRAY_BUFFER, size, points, GL_STATIC_DRAW);

    glEnableVertexAttribArray(0);
    glVertexAttribPointer(0,3,GL_FLOAT,GL_FALSE,0,0);
    glBindVertexArray(0);

    arraySize = size/(sizeof(GLfloat)*3);

    translation.x = translation.y = translation.z = 0;
    rotation.x = rotation.y = rotation.z = 0;
    scale.x = scale.y = scale.z = 1;
}

SplitModel::~SplitModel(){
	glDeleteBuffers(1, &positionBuffer); //FIX
    glDeleteVertexArrays(1, &vertexArrayID);
}

glm::vec3* SplitModel::getTranslationPtr(){ return &translation; }
glm::vec3* SplitModel::getRotationPtr(){ return &rotation; }
glm::vec3* SplitModel::getScalePtr(){ return &scale; }

glm::mat4 SplitModel::getTransformationModel(){ 
    return glm::translate(translation) * /* translation */
        (glm::rotate(rotation.x, X_ROT_MATRIX) * /* rotation */
            glm::rotate(rotation.y, Y_ROT_MATRIX) *
            glm::rotate(rotation.z, Z_ROT_MATRIX)) *
        glm::scale(scale); /* scale */
}


void SplitModel::draw(){
    glBindVertexArray(vertexArrayID);
    glDrawArrays(GL_TRIANGLES,0,arraySize);
    glBindVertexArray(0);
}