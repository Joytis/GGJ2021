#ifndef LWG_SDFCROSS2D_INCLUDED
#define LWG_SDFCROSS2D_INCLUDED

#include "./common.hlsl"
#include "./sdfRect2D.hlsl"

// Cross SDF
real sdfCross2D(real2 uv, real s) {
    real2 size = real2(0.25, s);
    real sdf1 = sdfRect2D(uv.xy, size.xy);
    real sdf2 = sdfRect2D(uv.xy, size.yx);
    return min(sdf1, sdf2);
}
GRAPH_DEFINE_2(sdfCross2D, real2, real, real);


#endif