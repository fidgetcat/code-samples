#pragma once

#include "Vector.h"

/*!
	\class Motion
	\brief Helper class that has simple functionality for primitive vector physics calculations
	
*/

class Motion {
	public:
	//! This class must be initialized with starting acceleration and velocity vectors. The time is used during update calculations and should come from the engine's clock.
	Motion(double initime, Vector2 acc = Vector2(0,0), Vector2 vel = Vector2(0,0));
	double lTime;
	Vector2 acceleration;
	Vector2 velocity; 
	
	//! This method updates velocity (not acceleration) AFTER returning the displacement. The time passed in is current engine time. This means that all movement happens a frame later from the last outside update, since the "old" displacement is returned.
	Vector2 Update(double rtime);
	//! This method returns a displacement based on current velocity. It updates nothing.
	Vector2 GetDisplacement(double rtime);
};