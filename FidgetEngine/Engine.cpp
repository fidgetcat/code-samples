#include "Engine.h"
#include "Transform.h"
#include "ScrObj.h"
#include <string>

Engine::Engine() : myCam(70,800.0f/600.0f,0.01f,100.0f,0,0,3){
	socket.bind(sf::Socket::AnyPort);
	socket.setBlocking(false);
	interactEntry = false;
	sf::ContextSettings settings;
    settings.depthBits = 24;
    settings.stencilBits = 8;
    settings.antialiasingLevel = 4;
    settings.majorVersion = 3;
    settings.minorVersion = 0;
	
	myWindow = new sf::Window(sf::VideoMode(800, 600), "SFML FPS", sf::Style::Default, settings);
    myWindow->setPosition(sf::Vector2i(0,0));

    GLenum status = glewInit();
    if(status != GLEW_OK)
        std::cout << "ERROR" << std::endl;
	
	myCam.updateCamera();
	//glEnable(GL_DEPTH_TEST);
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	
	std::string vs = "basicShader.vs";
    std::string fs = "basicShader.fs";
	shader.init(vs, fs);
	vs = "basicShaderTX.vs";
    fs = "basicShaderTX.fs";
	TXshader.init(vs,fs);
}

Engine::~Engine(){
	//destructor
}

void Engine::UdpBind(unsigned short portin){
	socket.unbind();
	socket.bind(portin);
}

void Engine::UdpSend(Packet* pktin, IPadd addin, unsigned short portin){
	socket.send(pktin->belly,addin.addr,portin);
}

void Engine::Interact(ScrObject* obj){
	if(interactEntry){
		std::cout<<"Interact() reentry error!"<<std::endl;
		exit(0);
	}
	interactEntry = true;
	HandLsNode *ptr = NULL;
	
	for(ptr = interactHandlers.GetFirst(); ptr; ptr = interactHandlers.GetNext()){
		((InteractHandler *)ptr->myHandler)->InteractEvent(obj);
	}
	interactEntry = false;
}

double Engine::GetTime(){
	return myClock.getElapsedTime().asSeconds();
}

void Engine::Run(){

	//init();
	
    //makes sure the program runs at 60 FPS
    const float TIME_PER_FRAME = 1.0f/60*1000;
	sf::Clock FPSclock;
	HandLsNode *ptr = NULL;
	
    while (myWindow->isOpen()){
        sf::Event event;
        while (myWindow->pollEvent(event)){
            if (event.type == sf::Event::Closed){
                myWindow->close();
            }else if (event.type == sf::Event::Resized){
                glViewport(0, 0, event.size.width, event.size.height);
            }//end else if
			else if (event.type == sf::Event::KeyPressed){
					for(ptr = keyHandlers.GetFirst(); ptr; ptr = keyHandlers.GetNext()){
					((KeyHandler *)ptr->myHandler)->KeyDnEvent((KeyHandler::Key::Keycode)event.key.code, event.key.alt, event.key.control, event.key.shift, event.key.system);
					}
			} // end keyDNIf
			else if (event.type == sf::Event::KeyReleased){
					for(ptr = keyHandlers.GetFirst(); ptr; ptr = keyHandlers.GetNext()){
					((KeyHandler *)ptr->myHandler)->KeyUpEvent((KeyHandler::Key::Keycode)event.key.code, event.key.alt, event.key.control, event.key.shift, event.key.system);
					}
			} //end keyUPif
        }//end while
		//update();
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		
		
		Packet pekg;
		IPadd dad;
		unsigned short dort;
		
		while(socket.receive(pekg.belly, dad.addr, dort)==sf::Socket::Done){
			for(ptr = networkHandlers.GetFirst(); ptr; ptr = networkHandlers.GetNext()){
			((NetworkHandler *)ptr->myHandler)->UdpEvent(&pekg, dad, dort);
			}
		}
		

		
		for(ptr = timeHandlers.GetFirst(); ptr; ptr = timeHandlers.GetNext()){
			((TimeHandler *)ptr->myHandler)->TimeEvent(GetTime());
		}
		
		for(ptr = drawHandlers.GetFirst(); ptr; ptr = drawHandlers.GetNext()){
			((DrawHandler *)ptr->myHandler)->DrawEvent(this, Identity());
		}	
		
        //render(camera);
        myWindow->display();

        //regulates the frames to 60
        sf::sleep(sf::milliseconds(TIME_PER_FRAME-
            FPSclock.getElapsedTime().asMilliseconds()));
        FPSclock.restart();
    }//end while
}