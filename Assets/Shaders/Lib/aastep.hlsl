#ifndef LWG_AASTEP_INCLUDED
#define LWG_AASTEP_INCLUDED

// Anti-aliasing step
real aastep(real threshold, real value) {
    // real pwidth = 0.1 * length(real2(ddx(value), ddy(value)));
    // return smoothstep(threshold - pwidth, threshold + pwidth, value);
    return step(threshold, value);
}

#endif