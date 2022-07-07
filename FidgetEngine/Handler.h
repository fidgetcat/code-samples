#pragma once
#include <stdlib.h>
#include <SFML/Window.hpp>
#include "Transform.h"
/*!
	\class Handler
	\brief Base class for the game's event handlers
	
	This engine processes events instead of having predefined code in an update loop. Objects that trigger or listen for events will normally inherit from either TimeHandler or Keyhandler, and then implement the abstract functions. KeyHandler has an enum interface that works with SFML, while the Engine has a clock that works for TimeHandler.
	Objects that want to be interacted with must inherit from or have an InteractHandler. To be drawn by the engine, objects must be DrawHandlers. 
*/
class Handler{
	public:
	Handler(class HandlerCollection* input);
	~Handler();


	
	class HandlerCollection *myCollection;
	struct HandLsNode *myNode;
	
	void Hold(class HandlerCollection *target);
	void Letgo();

};

class TimeHandler : public Handler{
	public:
	TimeHandler(class Engine* game);
	virtual void TimeEvent(double currTime)=0;
};

class DrawHandler : public Handler{
	public:
	DrawHandler(class Engine* game);
	virtual void DrawEvent(class Engine *gameEngine,Transform parentTrans)=0;
};

class NetworkHandler : public Handler{
	public:
	NetworkHandler(class Engine* game);
	virtual void UdpEvent(class Packet* pktin, class IPadd addin, unsigned short portin)=0;
};

class InteractHandler : public Handler{
	public:
	InteractHandler(class Engine* game);
	virtual void InteractEvent(class ScrObject* obj)=0;
};

class KeyHandler : public Handler{
	public:
		class Key{
		public:
			enum Keycode
			{
        Unknown = -1, ///< Unhandled key
        A = 0,        ///< The A key
        B,            ///< The B key
        C,            ///< The C key
        D,            ///< The D key
        E,            ///< The E key
        F,            ///< The F key
        G,            ///< The G key
        H,            ///< The H key
        I,            ///< The I key
        J,            ///< The J key
        K,            ///< The K key
        L,            ///< The L key
        M,            ///< The M key
        N,            ///< The N key
        O,            ///< The O key
        P,            ///< The P key
        Q,            ///< The Q key
        R,            ///< The R key
        S,            ///< The S key
        T,            ///< The T key
        U,            ///< The U key
        V,            ///< The V key
        W,            ///< The W key
        X,            ///< The X key
        Y,            ///< The Y key
        Z,            ///< The Z key
        Num0,         ///< The 0 key
        Num1,         ///< The 1 key
        Num2,         ///< The 2 key
        Num3,         ///< The 3 key
        Num4,         ///< The 4 key
        Num5,         ///< The 5 key
        Num6,         ///< The 6 key
        Num7,         ///< The 7 key
        Num8,         ///< The 8 key
        Num9,         ///< The 9 key
        Escape,       ///< The Escape key
        LControl,     ///< The left Control key
        LShift,       ///< The left Shift key
        LAlt,         ///< The left Alt key
        LSystem,      ///< The left OS specific key: window (Windows and Linux), apple (MacOS X), ...
        RControl,     ///< The right Control key
        RShift,       ///< The right Shift key
        RAlt,         ///< The right Alt key
        RSystem,      ///< The right OS specific key: window (Windows and Linux), apple (MacOS X), ...
        Menu,         ///< The Menu key
        LBracket,     ///< The [ key
        RBracket,     ///< The ] key
        SemiColon,    ///< The ; key
        Comma,        ///< The , key
        Period,       ///< The . key
        Quote,        ///< The ' key
        Slash,        ///< The / key
        BackSlash,    ///< The \ key
        Tilde,        ///< The ~ key
        Equal,        ///< The = key
        Dash,         ///< The - key
        Space,        ///< The Space key
        Return,       ///< The Return key
        BackSpace,    ///< The Backspace key
        Tab,          ///< The Tabulation key
        PageUp,       ///< The Page up key
        PageDown,     ///< The Page down key
        End,          ///< The End key
        Home,         ///< The Home key
        Insert,       ///< The Insert key
        Delete,       ///< The Delete key
        Add,          ///< The + key
        Subtract,     ///< The - key
        Multiply,     ///< The * key
        Divide,       ///< The / key
        Left,         ///< Left arrow
        Right,        ///< Right arrow
        Up,           ///< Up arrow
        Down,         ///< Down arrow
        Numpad0,      ///< The numpad 0 key
        Numpad1,      ///< The numpad 1 key
        Numpad2,      ///< The numpad 2 key
        Numpad3,      ///< The numpad 3 key
        Numpad4,      ///< The numpad 4 key
        Numpad5,      ///< The numpad 5 key
        Numpad6,      ///< The numpad 6 key
        Numpad7,      ///< The numpad 7 key
        Numpad8,      ///< The numpad 8 key
        Numpad9,      ///< The numpad 9 key
        F1,           ///< The F1 key
        F2,           ///< The F2 key
        F3,           ///< The F3 key
        F4,           ///< The F4 key
        F5,           ///< The F5 key
        F6,           ///< The F6 key
        F7,           ///< The F7 key
        F8,           ///< The F8 key
        F9,           ///< The F9 key
        F10,          ///< The F10 key
        F11,          ///< The F11 key
        F12,          ///< The F12 key
        F13,          ///< The F13 key
        F14,          ///< The F14 key
        F15,          ///< The F15 key
        Pause,        ///< The Pause key

        KeyCount      ///< Keep last -- the total number of keyboard keys
    };
			};
	
	KeyHandler(class Engine* game);
	virtual void KeyUpEvent(Key::Keycode keyIn,bool alt, bool control, bool shift, bool system){}
	virtual void KeyDnEvent(Key::Keycode keyIn,bool alt, bool control, bool shift, bool system)=0;
	

	
};