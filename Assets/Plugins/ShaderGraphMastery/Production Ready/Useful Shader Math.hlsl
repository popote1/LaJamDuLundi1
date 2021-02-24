const float2x2 m = float2x2(0.8, -0.6, 0.6, 0.8);
float a = 0;
float b = 1;
float2 d = 0;
[loop]
for( int i=0; i<15; i++ )
{
    float2 f = frac(UV);
    float2 u = f * f * (3 - 2*f);

    float2 p = floor(UV);
    float a = T.Sample(S, p + float2(0.5,0.5)).r;
	float b = T.Sample(S, p + float2(1.5,0.5)/256).r;
	float c = T.Sample(S, p + float2(0.5,1.5)/256).r;
	float d = T.Sample(S, p + float2(1.5,1.5)/256).r;
	float3 n = float3(a + (b-a) * u.r + (c-a) * u.g + (a-b-c+d) * u.r * u.g, 6*f*(1-f)*(float2(b-a, c-a) + (a-b - c+d) * u.gr));
        
    d += n.gb;
    a += b * n.r / (1 + dot(d,d));
    b *= 0.5;
    p = mul(m, p * 2);
}
Out = a;

// Voronoi

int2 p = floor(UV);
float2  f = frac(UV);

float res = 0;
for( int j=-1; j<=1; j++ )
for( int i=-1; i<=1; i++ )
{
    int2 b = int2(i, j);
    float3 p3 = frac((p+b).rgr * float3(0.1031, 0.1030, 0.0973));
    p3 += dot(p3, p3.gbr+33.33);
    float2  r = b - f + frac((p3.rr+p3.gb)*p3.bg);
    float d = dot(r, r);
    res += 1/pow( d, 8.0);
}
Out =  pow(rcp(res), rcp(16.0));

// expVoronoi

int2 p = floor(UV);
float2  f = frac(UV);

float res = 0;
for( int j=-1; j<=1; j++ )
for( int i=-1; i<=1; i++ )
{
    int2 b = int2(i, j);
    float3 p3 = frac((p+b).rgr * float3(0.1031, 0.1030, 0.0973));
    p3 += dot(p3, p3.gbr+33.33);
    float2  r = b - f + frac((p3.rr+p3.gb)*p3.bg);
    res += exp(-32 * length(r));
}
Out = -(rcp(32.0))*log(res);
    
// Better Hash function

// Hash3
float3 p = float3( 
dot(UV,float3(127.1,311.7, 74.7)),
dot(UV,float3(269.5,183.3,246.1)),
dot(UV,float3(113.5,271.9,124.6)));
Out = frac(sin(p)*43758.5453123);

// Hash2

float2 p = float2( 
dot(UV,float2(127.1,311.7)),
dot(UV,float2(269.5,183.3)));
Out = frac(sin(p)*43758.5453123);


// Other Hashes


//  1 out, 1 in...
float p = frac(U * .1031);
p *= p + 33.33;
p *= p + p;
Out =  frac(p);

//  1 out, 2 in...
float3 p3  = frac(float3(UV.rgr) * 0.1031);
p3 += dot(p3, p3.gbr + 33.33);
Out = frac((p3.r + p3.g) * p3.b);

//  1 out, 3 in...
	float3 p3  = frac(UVW * .1031);
    p3 += dot(p3, p3.gbr + 33.33);
    Out =  frac((p3.r + p3.g) * p3.b);

//  2 out, 1 in...
	float3 p3 = frac(float3(U) * float3(.1031, .1030, .0973));
	p3 += dot(p3, p3.gbr + 33.33);
    Out =  frac((p3.rr+p3.gb)*p3.bg);

///  2 out, 2 in...
float3 p3 = frac(float3(UV.rgr) * float3(.1031, .1030, .0973));
p3 += dot(p3, p3.gbr+33.33);
Out =  frac((p3.rr+p3.gb)*p3.bg);


///  2 out, 3 in...
	float3 p3 = frac(UVW * float3(.1031, .1030, .0973));
    p3 += dot(p3, p3.gbr+33.33);
    Out =  frac((p3.rr+p3.gb)*p3.bg);

//  3 out, 1 in...
    float3 p3 = frac(float3(p) * float3(.1031, .1030, .0973));
    p3 += dot(p3, p3.gbr+33.33);
    Out =  frac((p3.rrg+p3.gbb)*p3.bgr); 

///  3 out, 2 in...
float3 p3 = frac(float3(UV.rgr) * float3(.1031, .1030, .0973));
p3 += dot(p3, p3.grb+33.33);
Out =  frac((p3.rrg+p3.gbb)*p3.bgr);

///  3 out, 3 in...
	float3 p3 = frac(UVW * float3(.1031, .1030, .0973));
    p3 += dot(p3, p3.grb+33.33);
    Out =  frac((p3.rrg + p3.grr)*p3.bgr);

// 4 out, 1 in...
	float4 p4 = frac(float4(U) * float4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.abrg+33.33);
    Out =  frac((p4.rrgb+p4.gbba)*p4.bgar);

// 4 out, 2 in...
float4 p4 = frac(float4(UV.rgrg) * float4(.1031, .1030, .0973, .1099));
p4 += dot(p4, p4.abrg+33.33);
Out =  frac((p4.rrgb+p4.gbba)*p4.bgar);

// 4 out, 3 in...
	float4 p4 = frac(float4(UVW.rgbr)  * float4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.abrg+33.33);
    Out =  frac((p4.rrgb+p4.gbba)*p4.bgar);

// 4 out, 4 in...
    float4 p4 = frac(UVWX  * float4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.abrg+33.33);
    Out =  frac((p4.rrgb+p4.gbba)*p4.bgar);


//other
// e^pi (Gelfond's constant) // 2^sqrt(2) (Gelfondâ€“Schneider constant)
Out = frac(cos(dot(UV,float2(23.14069263277926, 2.665144142690225))) * 12345.6789 );


// Simple Noise

inline float unity_valueNoise (float2 uv)
{
    float2 i = floor(uv);
    float2 f = frac(uv);
    f = f * f * (3 - 2 * f);

    uv = abs(frac(UV) - 0.5);
    float2 c1 = i + float2(1, 0);
    float2 c2 = i + float2(0, 1);
    float2 c3 = i + float2(1, 1);
    
    float4 rand = 0;
    float3 p3 = frac(float3(i.rgr) * 0.1031); p3 += dot(p3, p3.gbr + 33.33); rand.r = frac((p3.r + p3.g) * p3.b);
    p3 = frac(float3(c1.rgr) * 0.1031); p3 += dot(p3, p3.gbr + 33.33); rand.g = frac((p3.r + p3.g) * p3.b);
    p3 = frac(float3(c2.rgr) * 0.1031); p3 += dot(p3, p3.gbr + 33.33); rand.b = frac((p3.r + p3.g) * p3.b);
    p3 = frac(float3(c3.rgr) * 0.1031); p3 += dot(p3, p3.gbr + 33.33); rand.a = frac((p3.r + p3.g) * p3.b);

    float2 topBottomGrid = lerp(rand.rg, rand.ba, f.r)
    float t = lerp(topBottomGrid.r, topBottomGrid.g, f.g);
    return t;
}

void Unity_SimpleNoise_float(float2 UV, float Scale, int Octaves, out float Out)
{
    float t = 0;
    float freq = 1;
    float amp = pow(0.5, Octaves);
    
    for(int i = 0; i < Octaves; i++)
    {
        freq += freq;
        amp = pow(0.5, Octaves - i);
  
        // noise function
        float2 uv = UV * Scale * amp / freq;
        float2 i = floor(uv);
        float2 f = frac(uv);
        f = f * f * (3 - 2 * f);

        uv = abs(frac(uv) - 0.5);
        float2 c1 = i + float2(1, 0);
        float2 c2 = i + float2(0, 1);
        float2 c3 = i + float2(1, 1);
        
        float4 rand = 0;
        float3 p3 = frac(float3(i.rgr) * 0.1031); 
        p3 += dot(p3, p3.gbr + 33.33); 
        rand.r = frac((p3.r + p3.g) * p3.b);
        
        p3 = frac(float3(c1.rgr) * 0.1031); 
        p3 += dot(p3, p3.gbr + 33.33); 
        rand.g = frac((p3.r + p3.g) * p3.b);
        
        p3 = frac(float3(c2.rgr) * 0.1031); 
        p3 += dot(p3, p3.gbr + 33.33); 
        rand.b = frac((p3.r + p3.g) * p3.b);
        
        p3 = frac(float3(c3.rgr) * 0.1031); 
        p3 += dot(p3, p3.gbr + 33.33); 
        rand.a = frac((p3.r + p3.g) * p3.b);
    
        float2 topBottomGrid = lerp(rand.rg, rand.ba, 1-f.r);
        t += lerp(topBottomGrid.r, topBottomGrid.g, f.g);
    }

    Out = t;
}


/// gradient noise with derivatives

    float2 i = floor(p);
    float2 f = frac(p);
    float2 u = f*f*f*(f*(f*6-15)+10);
    float2 du = 30*f*f*(f*(f-2)+1);
    
    float2 c1 = i + float2(1,0);
    float2 c2 = i + float2(0,1);
    float2 c3 = i + 1;
    
    float3 v1 = 0;
    float3 v2 = 0;
    float3 v3 = 0;
    float3 v4 = 0;
    
    ///  2 out, 2 in...
    float3 p3 = frac(float3(i.rgr) * float3(.1031, .1030, .0973)); p3 += dot(p3, p3.gbr+33.33); v1.y = frac((p3.rr+p3.gb)*p3.bg);// ga
    p3 = frac(float3(c1.rgr) * float3(.1031, .1030, .0973)); p3 += dot(p3, p3.gbr+33.33); v2.y = frac((p3.rr+p3.gb)*p3.bg);// gb
    p3 = frac(float3(c2.rgr) * float3(.1031, .1030, .0973)); p3 += dot(p3, p3.gbr+33.33); v3.y = frac((p3.rr+p3.gb)*p3.bg);// gc
    p3 = frac(float3(c3.rgr) * float3(.1031, .1030, .0973)); p3 += dot(p3, p3.gbr+33.33); v4.y = frac((p3.rr+p3.gb)*p3.bg);// gd
    
    float v1.x = dot( v1.y, f); // va
    float v2.x = dot( v2.y, f - float2(1,0) ); // vb
    float v3.x = dot( v3.y, f - float2(0,1) ); // vc
    float v4.x = dot( v4.y, f - 1 ); //vd 

    return float3(
        v1 + dot(u*(v1-v2), 1) + u.r * u.g * v1-v2-v3 + v4 + 
        du * (u.gr*(v1.x - v2.x - v3.x + v4.x) + float2(v1.x, v2.x) + v1.x);
    )
    
// Rotate About Axis Optimized

    float s = sin(Rotation);
    float c = cos(Rotation);
    float nc = 1.0 - c;

    Axis = normalize(Axis);
    float3x3 rot_mat = 
    {   nc * Axis.x * Axis.x + c,            nc * Axis.x * Axis.y - Axis.z * s,   nc * Axis.z * Axis.x + Axis.y * s,
        nc * Axis.x * Axis.y + Axis.z * s,   nc * Axis.y * Axis.y + c,            nc * Axis.y * Axis.z - Axis.x * s,
        nc * Axis.z * Axis.x - Axis.y * s,   nc * Axis.y * Axis.z + Axis.x * s,   nc * Axis.z * Axis.z + c
    };
    Out = mul(rot_mat,  In);
   
   
   
   
float c = cos(Rotation);
float3 axis = normalize(Axis);  
float3 Axiss = axis * sin(Rotation);
float3 Ncaxis = (1.0 - c) * axis;

float3x3 rot_mat = {   
    NcAxis.xxz * axis.xyx + float3(c, Axiss.z, -Axiss.y), 
    NcAxis.xyy * axis.yyz + float3(-Axiss.z, c, Axiss.x), 
    NcAxis.zyz * axis.xzz + float3(Axiss.y, -Axiss.x, c)
};
Out = mul(rot_mat,  In);


nc * Axis.x * Axis.x + c,            nc * Axis.x * Axis.y - Axis.z * s,     nc * Axis.z * Axis.x + Axis.y * s,
nc * Axis.x * Axis.y + Axis.z * s,   nc * Axis.y * Axis.y + c,              nc * Axis.y * Axis.z - Axis.x * s,
nc * Axis.z * Axis.x - Axis.y * s,   nc * Axis.y * Axis.z + Axis.x * s,     nc * Axis.z * Axis.z + c



/// Bicubic Filtering

float4 filter(sampler2D texture, vec2 UV, vec2 texscale)
{
    float2 fxy = frac(UV);
    float2 uv = UV - fxy;
    float2 x2 = fxy * fxy;
    float2 x3 = x2 * fxy;
    float4 a = float4(-1,3,-3,1);
    float4 b = float4(3,-6,3,0);
    float4 c = float4(-3,0,3,0);
    float4 d = float4(1,4,1,0);

    float4 xcubic = (a * x3.r + b * x2.r + c * fxy.r + d) / 6;
    float4 ycubic = (a * x3.g + b * x2.g + c * fxy.g + d) / 6;

    float4 c1 = uv.rrgg + float2(-0.5, 1.5).rgrg;
    float4 s = float4(xcubic.rb + xcubic.ga, ycubic.rb + ycubic.ga);
    float4 offset = c1 + float4(xcubic.ga, ycubic.ga) /s;

    float4 s0 = T.Sample(Sampler, offset.rb * texscale);
    float4 s1 = T.Sample(Sampler, offset.gb * texscale);
    float4 s2 = T.Sample(Sampler, offset.ra * texscale);
    float4 s3 = T.Sample(Sampler, offset.ga * texscale);

    float2 sxy = s.rg / (s.rg +s.ba);

    Out = lerp(lerp(s3, s2, sxy.r),lerp(s1, s0, sxy.r), sxy.g);
}
