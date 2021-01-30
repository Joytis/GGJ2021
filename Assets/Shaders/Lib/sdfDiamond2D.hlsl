#ifndef LWG_SDFDIAMOND2D_INCLUDED
#define LWG_SDFDIAMOND2D_INCLUDED

#include "./common.hlsl"
#include "./sdfTriangle2D.hlsl"

// Diamond SDF
real sdfDiamond2D(real2 uv) {
    real sdf1 = sdfTriangle2D(uv);
    real sdf2 = sdfTriangle2D(real2(uv.x, 1 - uv.y));
    return max(sdf1, sdf2);
}
GRAPH_DEFINE_1(sdfDiamond2D, real2, real);

#endif