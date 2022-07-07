#pragma once

#include <SFML/System.hpp>
#include <SFML/Network.hpp>
/*!
	\class Packet
	\brief Wrapper for sfml Packet class that provides primitive Packet generation and manipulation.
	
	StuffMe methods place data of the appropriate type into the packet. They can be placed into outside variables
	using pointers together with PopMe methods. Variables must be popped out in the same order they were stuffed in.
	
*/

class Packet{
	public:
	Packet();
	//! Copy constructor.
	Packet(sf::Packet pktin);
	sf::Packet belly;
	
	void StuffMe(double input);
	void StuffMe(int input);
	void StuffMe(bool input);
	
	void PopMe(double* input);
	void PopMe(int* input);
	void PopMe(bool* input);
};

/*!
	\class IPadd
	\brief Wrapper for sfml IP address class
	
*/

class IPadd{
	public:
	IPadd(){}
	IPadd(const char* input);
	IPadd(const IPadd &input);
	IPadd(sf::IpAddress addin);
	sf::IpAddress addr;
	IPadd& operator = (const IPadd &input);
};

