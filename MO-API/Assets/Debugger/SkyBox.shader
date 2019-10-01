Shader "Custom/SkyBox"
{
    Properties
    {
        _Color1("Color1",Color) = (1,1,1,1)
        _Color2("Color2",Color) = (1,0,0,1)
    }
    SubShader
    {
        Tags 
        {
            "RenderType" = "Opaque" 
            "Queue" = "BackGround"
            "PreviewType" = "Skybox"
        }
        
        Pass
        {
            ZWrite Off
            Cull Off
        
            CGPROGRAM
            
            #include "UnityCG.cginc"
            
            #pragma vertex vert
            #pragma fragment frag
            
            fixed3 _Color1;
            fixed3 _Color2;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                return o;
            }
            
            fixed4 frag(v2f o) : SV_TARGET
            {
                return fixed4(lerp(_Color2,_Color1,o.uv.y * 0.5 + 0.5),1.0);
            }
            
            ENDCG
        }
    }
}
