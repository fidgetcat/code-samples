#pragma once

class Color{
	public:
	Color(double R, double G, double B);
	Color(const Color &thing);
	Color& operator = (const Color &thing);
	double r;
	double g;
	double b;
};

Color operator + (const Color &first, const Color &second);

Color operator * (const Color &first, const Color &second);