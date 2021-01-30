#ifndef LWG_SDFFILL2D_INCLUDED
#define LWG_SDFFILL2D_INCLUDED

#include "./common.hlsl"
#include "./aastep.hlsl"

// Fill Sample
real sdfFill2D(real sdf, real size) {
    return 1 - aastep(size, sdf);
}
GRAPH_DEFINE_2(sdfFill2D, real, real, real);

#endif