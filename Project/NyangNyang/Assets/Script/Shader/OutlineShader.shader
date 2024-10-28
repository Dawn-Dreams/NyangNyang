Shader "Custom/EdgeDetectionShader_DepthNormal"
{
    Properties
    {
        _MainTex ("Scene Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1, 1, 1, 1)
        _EdgeColor ("Edge Color", Color) = (0, 0, 0, 1)
        _SobelThreshold ("Sobel Threshold", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _CameraDepthNormalsTexture;
            fixed4 _TintColor;
            fixed4 _EdgeColor;
            float _SobelThreshold;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed3 color = tex2D(_MainTex, uv).rgb;

                // ȭ�� ��ǥ�� ������� �� ��Ȯ�� ������ ���
                float2 offset = float2(0.5 / _ScreenParams.x, 0.5 / _ScreenParams.y);

                // ������Ʈ�� ȭ�� �������� ����Ͽ� ����-��� �ؽ�ó���� �ֺ� �ȼ� ���ø�
                float3 normalTL = UnpackNormal(tex2D(_CameraDepthNormalsTexture, i.screenPos.xy + offset * float2(-1, 1)));
                float3 normalT  = UnpackNormal(tex2D(_CameraDepthNormalsTexture, i.screenPos.xy + offset * float2( 0, 1)));
                float3 normalTR = UnpackNormal(tex2D(_CameraDepthNormalsTexture, i.screenPos.xy + offset * float2( 1, 1)));
                float3 normalL  = UnpackNormal(tex2D(_CameraDepthNormalsTexture, i.screenPos.xy + offset * float2(-1, 0)));
                float3 normalR  = UnpackNormal(tex2D(_CameraDepthNormalsTexture, i.screenPos.xy + offset * float2( 1, 0)));
                float3 normalBL = UnpackNormal(tex2D(_CameraDepthNormalsTexture, i.screenPos.xy + offset * float2(-1, -1)));
                float3 normalB  = UnpackNormal(tex2D(_CameraDepthNormalsTexture, i.screenPos.xy + offset * float2( 0, -1)));
                float3 normalBR = UnpackNormal(tex2D(_CameraDepthNormalsTexture, i.screenPos.xy + offset * float2( 1, -1)));

                // ���� ������ ���� �Һ� ���� ����
                float3 edgeX = -normalTL + normalTR - 2.0 * normalL + 2.0 * normalR - normalBL + normalBR;
                float3 edgeY = normalTL + 2.0 * normalT + normalTR - normalBL - 2.0 * normalB - normalBR;

                // �Һ� ���� ���� ���
                float edgeIntensity = length(edgeX) + length(edgeY);

                // ��Ƽ�ٸ������ ���� smoothstep ���
                edgeIntensity = smoothstep(_SobelThreshold - 0.05, _SobelThreshold + 0.05, edgeIntensity);

                // ���� Tint �÷� ���� �� ���� ����� ȥ��
                color = lerp(color, color * _TintColor.rgb, _TintColor.a);
                color = lerp(color, _EdgeColor.rgb, edgeIntensity);

                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}  
