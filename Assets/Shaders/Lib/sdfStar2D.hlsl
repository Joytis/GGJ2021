#ifndef LWG_SDFSTAR2D_INCLUDED
#define LWG_SDFSTAR2D_INCLUDED

#include "./common.hlsl"
#include "./sdfRect2D.hlsl"

// Star SDF
real sdfStar2D(real2 uv, real points, real size) {
    real tau = 6.28318530718;
    uv = uv * 4 - 2;
    real a = atan2(uv.y, uv.x)/tau;
    real seg = a * points;
    a = ((floor(seg) + 0.5)/points + 
        lerp(size, -size, step(.5, frac(seg)))) 
        * tau;
    return abs(dot(real2(cos(a),sin(a)), uv));
}
GRAPH_DEFINE_3(sdfStar2D, real2, real, real, real);

#endif