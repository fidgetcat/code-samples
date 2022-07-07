#ifndef SPLIT_MODEL_H
#define SPLIT_MODEL_H

#include <iostream>
#include <GL/glew.h>
#include <glm/glm.hpp>
#define GLM_FORCE_RADIANS
#include <glm/gtx/transform.hpp>
#include <stdio.h>
#include <SFML/Graphics.hpp>

class SplitModel{
    public:
        SplitModel();
        SplitModel(GLfloat points[],int size);
        void init(GLfloat points[],int size);
        ~SplitModel();

        glm::vec3* getTranslationPtr();
        glm::vec3* getRotationPtr();
        glm::vec3* getScalePtr();

        glm::mat4 getTransformationModel();
        void draw();
    private:
        glm::vec3 translation, rotation, scale;
        const static glm::vec3 X_ROT_MATRIX, Y_ROT_MATRIX, Z_ROT_MATRIX;

        GLuint vertexArrayID;
        GLuint positionBuffer;
        int arraySize;
};

#endif /* SPLIT_MODEL_H */
