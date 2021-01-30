#ifndef LWG_SDFRAYS2D_INCLUDED
#define LWG_SDFRAYS2D_INCLUDED

#include "./common.hlsl"
#include "./sdfCircle2D.hlsl"

// Rays SDF
real sdfRays2D(real2 uv, real rays) {
    real tau = 6.28318530718;
    uv -= .5;
    return frac(atan2(uv.y, uv.x) / tau * rays);   
}
GRAPH_DEFINE_2(sdfRays2D, real2, real, real);

#endif