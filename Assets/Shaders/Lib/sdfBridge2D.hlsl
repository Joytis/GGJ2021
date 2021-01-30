#ifndef LWG_sdfBridge2D_INCLUDED
#define LWG_sdfBridge2D_INCLUDED

#include "./common.hlsl"
#include "./aastep.hlsl"
#include "./sdfStroke2D.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"


// Bridge Sampler
real3 sdfBridge2D(real3 color, real sdf, real stroke, real width) {
    color *= 1 - sdfStroke2D(sdf, stroke, width * 2);
    return color + sdfStroke2D(sdf, stroke, width);
    
}

// Stoke Sample Graph
GRAPH_DEFINE_4(sdfBridge2D, real3, real, real, real, real3);

#endif