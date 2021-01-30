#ifndef LWG_SDFVESICA2D_INCLUDED
#define LWG_SDFVESICA2D_INCLUDED

#include "./common.hlsl"
#include "./sdfCircle2D.hlsl"

// Vesica SDF
real sdfVesica2D(real2 uv, real separation) {
    real2 offset = real2(separation * 0.5, 0);
    real sdf1 = sdfCircle2D(uv - offset);
    real sdf2 = sdfCircle2D(uv + offset);
    return max(sdf1, sdf2);
}
GRAPH_DEFINE_2(sdfVesica2D, real2, real, real);


#endif