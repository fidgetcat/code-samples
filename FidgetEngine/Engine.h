#ifndef ENGINE_H
#define ENGINE_H

#include <iostream>
#include <GL/glew.h>
#include <SFML/Window.hpp>
#include "Camera.h"
#include "Transform.h"
#include "Object.h"
#include "HandlerCollection.h"
#include "Handler.h"
#include "Network.h"
#include "SplitShader.h"
#include "SplitShaderTX.h"
#include <SFML/Network.hpp>

/*!
	\class Engine
	\brief Game engine class. Not meant to be derived from.
	
	The game engine must first be created and then populated with objects before it is Run().
	It manages the various game entities through its Handler collections. For example, all
	objects that are or have DrawHandler are drawn in the Engine's update loop. When an object is created
	outside of the engine, it receives a pointer to the engine and populates the respective collections
	on its own.
*/


class Engine{
    public:
	Engine();
	~Engine();
	void Run();
	double GetTime();
	//! Method called by objects that want to interact with others.
	/*!
		\param obj - The object calling this method must pass itself as an argument.
	*/
	void Interact(class ScrObject* obj);
	bool interactEntry;
	//! Basic networking method to send a packet.
	/*!
		\param pktin - Pointer to the packet being sent.
		\param IPadd - IP address of recipient;
		\param portin - Port of recipient.
	*/
	void UdpSend(Packet* pktin, IPadd addin, unsigned short portin);
	//! Basic networking method to bind the engine's socket to a port.
	void UdpBind(unsigned short portin);
	
	sf::UdpSocket socket;
	sf::Clock myClock;
	Camera myCam;
	sf::Window* myWindow;
	SplitShader shader;
	SplitShaderTX TXshader;
	HandlerCollection timeHandlers;
	HandlerCollection keyHandlers;
	HandlerCollection drawHandlers;
	HandlerCollection interactHandlers;
	HandlerCollection networkHandlers;
};

#endif /* ENGINE_H */
 