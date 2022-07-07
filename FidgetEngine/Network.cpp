#include "Network.h"

Packet::Packet(){
	
}

Packet::Packet(sf::Packet pktin) : belly(pktin){
}

void Packet::StuffMe(double input){
	belly << input;
}

void Packet::StuffMe(bool input){
	belly << input;
}

void Packet::StuffMe(int input){
	belly << (sf::Int32)input;
}

void Packet::PopMe(double* input){
	belly >> *input;
}

void Packet::PopMe(bool* input){
	belly >> *input;
}

void Packet::PopMe(int* input){
	sf::Int32 local;
	belly >> local;
	*input = (int)local;
}

IPadd::IPadd(const char* input) : addr(input){
	
}

IPadd::IPadd(sf::IpAddress addin) : addr(addin){
}

IPadd::IPadd(const IPadd &input) : addr(input.addr){
	
}

IPadd& IPadd::operator = (const IPadd &input){
	addr = input.addr;
	return *this;
}
	

