#ifndef LWG_SDFCIRCLE2D_INCLUDED
#define LWG_SDFCIRCLE2D_INCLUDED

#include "./common.hlsl"

// Circle SDF
real sdfCircle2D(real2 uv) {
    return length(uv - 0.5) * 2;
}
GRAPH_DEFINE_1(sdfCircle2D, real2, real);


#endif