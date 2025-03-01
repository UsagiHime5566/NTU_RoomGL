Shader "Custom/ModelSwave"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        // 扭曲效果参数
        _WaveSpeed ("Wave Speed", Range(0, 10)) = 1.0
        _WaveAmplitude ("Wave Amplitude", Range(0, 1)) = 0.1
        _WaveFrequency ("Wave Frequency", Range(0, 10)) = 2.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // 添加 vertex:vert 以便我们可以修改顶点
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        
        // 扭曲参数
        float _WaveSpeed;
        float _WaveAmplitude;
        float _WaveFrequency;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        // 顶点修改函数
        void vert(inout appdata_full v) {
            // 计算基于时间和位置的波动
            float time = _Time.y * _WaveSpeed;
            
            // 创建正弦波扭曲
            float wave = sin(time + v.vertex.x * _WaveFrequency) * _WaveAmplitude;
            
            // 应用扭曲到顶点位置
            v.vertex.y += wave;
            
            // 可以添加更多轴向的扭曲
            v.vertex.z += cos(time + v.vertex.x * _WaveFrequency) * _WaveAmplitude * 0.5;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
