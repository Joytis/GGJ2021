#ifndef LWG_SDFRECT2D_INCLUDED
#define LWG_SDFRECT2D_INCLUDED

#include "./common.hlsl"
// Rect SDF
real sdfRect2D(real2 uv, real2 size) {
    uv = uv * 2 - 1;
    return max(abs(uv.x/size.x),
              abs(uv.y/size.y));
}
GRAPH_DEFINE_2(sdfRect2D, real2, real2, real);

#endif