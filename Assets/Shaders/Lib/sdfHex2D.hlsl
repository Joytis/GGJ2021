#ifndef LWG_SDFHEX2D_INCLUDED
#define LWG_SDFHEX2D_INCLUDED

#include "./common.hlsl"
// Polygon SDF
real sdfHex2D(real2 uv) {
    uv = abs(uv * 2 - 1);
    return max(abs(uv.y), uv.x * 0.866025 + uv.y * 0.5);
}
GRAPH_DEFINE_1(sdfHex2D, real2, real);

#endif
