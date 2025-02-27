// Upgrade NOTE: replaced 'defined LOCAL' with 'defined (LOCAL)'

Shader "Custom/LineGL"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        iMouse ("Mouse Position", Vector) = (0,0,0,0)
        iResolution ("Resolution", Vector) = (1920,1080,0,0)
        _GlowIntensity ("Glow Intensity", Range(0.0, 1.0)) = 0.6
        _GlowPower ("Glow Power", Range(1.0, 5.0)) = 3.0
        _GlowWidth ("Glow Width", Range(0.01, 0.3)) = 0.12
        _LineWidth ("Line Width", Range(0.01, 0.1)) = 0.02
        _PointWidth ("Point Width", Range(0.01, 0.2)) = 0.08
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 iMouse;
            float4 iResolution;
            float _GlowIntensity;
            float _GlowPower;
            float _GlowWidth;
            float _LineWidth;
            float _PointWidth;

            static int configuration = 0;
            static bool drawdual = true;
            
            static const float lwidth = _LineWidth;     // 使用可調整的線寬
            static const float glow_width = _GlowWidth; 
            static const float pwidth = _PointWidth;    // 使用可調整的點寬
            static const float pglow_width = _GlowWidth * 2.0;
            static const float ledge0 = 0.0, ledge1 = lwidth, ledge2 = lwidth + glow_width;
            static const float pedge0 = 0.5*(lwidth+pwidth), pedge1 = pwidth, pedge2 = pwidth + pglow_width;
            static const float scale = 2.5;
            static const float zoom = 1.0;
            
            static const float PI =  3.141592654;
            static const float eps = 1e-4;
            
            static const float3 pcolor0 = float3(1,0,0);
            static const float3 pcolor1 = float3(0,1,0);
            static const float3 pcolor2 = float3(1,1,0);
            static const float3 pcolor3 = float3(0,1,1);
            static const float3 lcolor0 = pcolor0;
            static const float3 lcolor1 = pcolor1;
            static const float3 lcolor2 = pcolor2;
            static const float3 ccolor0 = float3(1,1,1);
            static const float3 ccolor1 = float3(0,0,1);
            
            
            
            // Distance from the conic
            float dist(float3 p, float3x3 m) {
              return dot(p,mul(m,p));
            }
            
            // The gradient uses the same matrix.
            // Don't homegenize the result!
            float3 grad(float3 p, float3x3 m) {
              return mul(m,p)*2.0;
            }
            
            float conic(float3 p, float3x3 m) {
              float d = dist(p,m);
              float3 dd = grad(p,m);
              d = abs(d/(p.z*length(dd.xy))); // Normalize for Euclidean distance
              float core = 1.0-smoothstep(ledge0,ledge1,d);
              float glow = pow(1.0-smoothstep(ledge1,ledge2,d), _GlowPower);
              return core + _GlowIntensity * glow;
            }
            
            float point0(float3 p, float3 q) {
              if (abs(p.z) < eps) return 0.0;
              if (abs(q.z) < eps) return 0.0;
              p /= p.z; q /= q.z; // Normalize
              float d = distance(p,q);
              float core = 1.0-smoothstep(pedge0,pedge1,d);
              float glow = pow(1.0-smoothstep(pedge1,pedge2,d), _GlowPower);
              return core + _GlowIntensity * glow;
            }
            
            float line0(float3 p, float3 q) {
              // Just treat as a degenerate conic. Note factor of 2.
              // We could do this more efficiently of course.
              return conic(p,float3x3(0,  0,  q.x,
                                  0,  0,  q.y,
                                  q.x,q.y,2.0*q.z));
            }
            
            float newpoint(float3 p, float3 q) {
              if (drawdual) return line0(p,q);
              else return point0(p,q);
            }
            
            float newline(float3 p, float3 q) {
              if (drawdual) return point0(p,q);
              else return line0(p,q);
            }
            
            float3 join(float3 p, float3 q) {
              // Return either intersection of lines p and q
              // or line through points p and q, r = kp + jq
              return cross(p,q);
            }
            
            // Screen coords to P2 coords
            float3 map(float2 p) {
              return float3(scale*zoom*(2.0*p - iResolution.xy) / iResolution.y, 1);
            }
            
            //-------------------------------------------------
            //From https://www.shadertoy.com/view/XtXGRS#
            float2 rotate(in float2 p, in float t) {
              return p * cos(-t) + float2(p.y, -p.x) * sin(-t);
            }
            
            float3 transform(float3 p) {
              float t = _Time.y;
              //if (keypress(CHAR_SPACE)) t = 0.0;
              p.x -= sin(0.08*t);
              p.xy = rotate(p.xy,0.2*t);
              p.yz = rotate(p.yz,0.1*t);
              return p;
            }
            
            float3 cmix(float3 color0, float3 color1, float level) {
                return lerp(color0,color1,level);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
              float2 fragCoord = i.uv * iResolution.xy;
              float3 p = map(fragCoord.xy);
              float3 p0,p1,p2,p3,p4; // p4 is the movable point
              if (configuration == 0) {
                p0 = float3(1,0,0); p1 = float3(0,1,0);
                p2 = float3(0,0,1); p3 = float3(1,1,1);
                p4 = float3(0.5,-1,1);
              } else if (configuration == 1) {
                p0 = float3(0,0,1); p1 = float3(1,0,1);
                p2 = float3(0,1,1); p3 = float3(1,1,1);
                p4 = float3(0.5,-1,1);
              } else if (configuration == 2) {
                p0 = float3(0,0,1);
                p1 = float3(0,1,1);
                p2 = float3(0.866,-0.5,1);
                p3 = float3(-0.866,-0.5,1);
                p4 = float3(0,0.618,1);
              } else {
                p0 = float3(1,0,1);  p1 = float3(0,1,1);
                p2 = float3(-1,0,1); p3 = float3(0,-1,1);
                p4 = float3(0.5,-1,1);
              }
              p0 = transform(p0); p1 = transform(p1);
              p2 = transform(p2); p3 = transform(p3);
              if (iMouse.x != 0.0) {
                p4 = map(float2(iResolution.x - iMouse.x, iResolution.y - iMouse.y));
              }
            
            
              float3 p01 = join(p0,p1);
              float3 p02 = join(p0,p2);
              float3 p03 = join(p0,p3);
              float3 p12 = join(p1,p2);
              float3 p13 = join(p1,p3);
              float3 p23 = join(p2,p3);
            
              // The line through A,B intersects line L at
              // (A.L)B - (B.L)A, whose conjugate is (A.L)B + (B.L)A
              float3 s01 = dot(p4,p0)*p1 + dot(p4,p1)*p0;
              float3 t01 = dot(p4,p0)*p1 - dot(p4,p1)*p0;
              float3 s12 = dot(p4,p1)*p2 + dot(p4,p2)*p1;
              float3 t12 = dot(p4,p1)*p2 - dot(p4,p2)*p1;
              float3 s23 = dot(p4,p2)*p3 + dot(p4,p3)*p2;
              float3 t23 = dot(p4,p2)*p3 - dot(p4,p3)*p2;
              float3 s03 = dot(p4,p3)*p0 + dot(p4,p0)*p3;
              float3 t03 = dot(p4,p3)*p0 - dot(p4,p0)*p3;
            
              float3 s02 = dot(p4,p0)*p2 + dot(p4,p2)*p0;
              float3 t02 = dot(p4,p0)*p2 - dot(p4,p2)*p0;
              float3 s13 = dot(p4,p1)*p3 + dot(p4,p3)*p1;
              float3 t13 = dot(p4,p1)*p3 - dot(p4,p3)*p1;
              
              float3 color = 0;
            
              // The diagonal lines of the quadrangle
              color = cmix(color,lcolor2,newline(p,p01));
              color = cmix(color,lcolor2,newline(p,p02));
              color = cmix(color,lcolor2,newline(p,p03));
              color = cmix(color,lcolor2,newline(p,p12));
              color = cmix(color,lcolor2,newline(p,p13));
              color = cmix(color,lcolor2,newline(p,p23));
              
              // The moving line
              color = cmix(color,lcolor1,newline(p,p4));
            
              float3 l0 = join(s01,s12);
              float3 l1 = join(s23,s03);
              float3 l2 = join(s12,s23);
              float3 l3 = join(s03,s01);
              //float3 l4 = join(s02,s23);
              //float3 l5 = join(s01,s13);
              color = cmix(color,lcolor0,newline(p,l0));
              color = cmix(color,lcolor0,newline(p,l1));
              color = cmix(color,lcolor0,newline(p,l2));
              color = cmix(color,lcolor0,newline(p,l3));
              //color = cmix(color,lcolor0,newline(p,l4));
              //color = cmix(color,lcolor0,newline(p,l5));
            
              // The points of the quadrangle
              color = cmix(color,pcolor0,newpoint(p,p0));
              color = cmix(color,pcolor0,newpoint(p,p1));
              color = cmix(color,pcolor0,newpoint(p,p2));
              color = cmix(color,pcolor0,newpoint(p,p3));
            
              color = cmix(color,pcolor2,newpoint(p,s01));
              color = cmix(color,pcolor2,newpoint(p,t01));
              color = cmix(color,pcolor2,newpoint(p,s12));
              color = cmix(color,pcolor2,newpoint(p,t12));
              color = cmix(color,pcolor2,newpoint(p,s23));
              color = cmix(color,pcolor2,newpoint(p,t23));
              color = cmix(color,pcolor2,newpoint(p,s03));
              color = cmix(color,pcolor2,newpoint(p,t03));
            
              color = cmix(color,pcolor1,newpoint(p,join(l0,l1)));
              color = cmix(color,pcolor1,newpoint(p,join(l2,l3)));
            
              return float4(pow(1.0*color,float3(0.4545, 0.4545, 0.4545)),1);
            }
            
            ENDCG
        }
    }
}

