#version 120

uniform sampler2D diffuse;
varying vec2 texCoord0;
varying vec3 color0;
varying vec3 color1;

void main()
{
	gl_FragColor = texture2D(diffuse, texCoord0) * vec4(color0, 1.0) + vec4(color1, 0.0);
}
