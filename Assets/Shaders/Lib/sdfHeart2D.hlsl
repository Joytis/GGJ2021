#ifndef LWG_SDFHEART2D_INCLUDED
#define LWG_SDFHEART2D_INCLUDED

#include "./common.hlsl"

// Polygon SDF
real sdfHeart2D(real2 uv) {
    uv -= real2(.5,.8);
    real r = length(uv)*5.;
    uv = normalize(uv);
    return r - 
         ((uv.y*pow(abs(uv.x),0.67))/ 
         (uv.y+1.5)-(2.)*uv.y+1.26);
}
GRAPH_DEFINE_1(sdfHeart2D, real2, real);

#endif
