#ifndef SPLIT_SHADER_TX_H
#define SPLIT_SHADER_TX_H

#include <iostream>
#include <fstream>
#include <string>
#include <GL/glew.h>
#include <glm/glm.hpp>
#define GLM_FORCE_RADIANS
#include <glm/gtx/transform.hpp>
#include <stdio.h>
#include <SFML/Graphics.hpp>

class SplitShaderTX{
    public:
        SplitShaderTX();
        SplitShaderTX(std::string shaderV,std::string shaderF);
        void init(std::string shaderV,std::string shaderF);
        ~SplitShaderTX();

        void bind(glm::mat4 tranModel,glm::vec3 mulcolor,glm::vec3 addcolor);
        void bindCamera(glm::mat4 camera,glm::mat4 tranModel,glm::vec3 mulcolor,glm::vec3 addcolor);
    private:
        GLuint uniformTransform;
        GLuint uniformMulColor;
        GLuint uniformAddColor;
 
        GLuint shader;
        GLuint vertexShader;
        GLuint fragmentShader;

        std::string getFileContent(std::string fileName);
};

#endif /* SPLIT_SHADER_TX_H */
