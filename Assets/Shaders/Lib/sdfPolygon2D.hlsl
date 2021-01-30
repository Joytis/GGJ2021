#ifndef LWG_SDFPOLYGON2D_INCLUDED
#define LWG_SDFPOLYGON2D_INCLUDED

#include "./common.hlsl"
// Polygon SDF
real sdfPolygon2D(real2 uv, real sides) {
    uv = uv * 2. - 1.;
    real pi = 3.14159265359;
    real tau = 6.28318530718;
    real a = atan2(uv.x, uv.y) + pi;
    real r = length(uv);
    real v = tau / sides;
    return cos(floor(0.5 + a / v) * v - a) * r;
}
GRAPH_DEFINE_2(sdfPolygon2D, real2, real, real);

#endif
