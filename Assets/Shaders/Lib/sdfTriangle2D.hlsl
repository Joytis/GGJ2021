#ifndef LWG_SDFTRIANGLE2D_INCLUDED
#define LWG_SDFTRIANGLE2D_INCLUDED
#include "./common.hlsl"

// Triangle SDF
real sdfTriangle2D(real2 uv) {
    uv = (uv * 2 - 1) * 2 ;
    return max(abs(uv.x) * 0.866025 + uv.y * 0.5, -uv.y * 0.5);
}
GRAPH_DEFINE_1(sdfTriangle2D, real2, real);

#endif