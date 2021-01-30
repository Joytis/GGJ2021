#ifndef LWG_SDF_COMMON_INCLUDED
#define LWG_SDF_COMMON_INCLUDED

// NOTE(clark): I don't really have a good API to make nodes inherit precision. For now, just assume float. 
#define GRAPH_DEFINE_1(name, T1, To) \
void name##_graph_float(T1 a, out To Out) { Out = name(a); }

#define GRAPH_DEFINE_2(name, T1, T2, To) \
void name##_graph_float (T1 a, T2 b, out To Out) { Out = name(a, b); } 

#define GRAPH_DEFINE_3(name, T1, T2, T3, To) \
void name##_graph_float (T1 a, T2 b, T3 c, out To Out) { Out = name(a, b, c); } 

#define GRAPH_DEFINE_4(name, T1, T2, T3, T4, To) \
void name##_graph_float (T1 a, T2 b, T3 c, T4 d, out To Out) { Out = name(a, b, c, d); } 

#endif