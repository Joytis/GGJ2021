#ifndef LWG_SDFSTROKE2D_INCLUDED
#define LWG_SDFSTROKE2D_INCLUDED

#include "./common.hlsl"
#include "./aastep.hlsl"

// Stroke Sampler
real sdfStroke2D(real sdf, real stroke, real width) {
    real offset = width * 0.5;
    real res = aastep(stroke, sdf + offset) - aastep(stroke, sdf - offset);
    return clamp(res, 0, 1);
}
GRAPH_DEFINE_3(sdfStroke2D, real, real, real, real);

#endif