#version 120

attribute vec3 position;
attribute vec2 texCoord;

varying vec2 texCoord0;
varying vec3 color0;
varying vec3 color1;

uniform mat4 transform;
uniform vec3 mulcolor;
uniform vec3 addcolor;

void main() {
	gl_Position = transform * vec4(position, 1.0);
	texCoord0 = texCoord;
	color0 = mulcolor;
	color1 = addcolor;
}

