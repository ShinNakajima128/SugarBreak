Shader "Custom/ScreenDoorTransparent"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BayerTex("BayerTex", 2D) = "black" {}
        _BlockSize("BlockSize", int) = 4
        _Radius("Radius", Range(0.001, 100)) = 10
    }
        SubShader
        {
            Tags {
                // 透明である必要はない
                        "RenderType" = "Opaque"
                    }

                    Blend SrcAlpha OneMinusSrcAlpha

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
                            float4 wpos : TEXCOORD1;
                            float4 spos : TEXCOORD2;
                        };

                        sampler2D _MainTex;
                        sampler2D _BayerTex;
                        float4 _MainTex_ST;

                        float _BlockSize;
                        float _Radius;

                        v2f vert(appdata v)
                        {
                            v2f o;
                            // MVP行列をかける 
                                    o.vertex = UnityObjectToClipPos(v.vertex);
                                    // UV座標
                                            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                                            // ワールド座標
                                                    o.wpos = mul(unity_ObjectToWorld, v.vertex);
                                                    // スクリーン座標
                                                            o.spos = ComputeScreenPos(o.vertex);
                                                            return o;
                                                        }

                                                        fixed4 frag(v2f i) : SV_Target
                                                        {
                                                            fixed4 col = tex2D(_MainTex, i.uv);

                                                        // カメラからの距離
                                                                float dist = distance(i.wpos, _WorldSpaceCameraPos);
                                                                // 領域内で0~1の距離にClamp
                                                                        float clamp_distance = saturate(dist / _Radius);
                                                                        // BlockSizeピクセル分でBayerMatrixの割り当て
                                                                                float2 uv_BayerTex = (i.spos.xy / i.spos.w) * (_ScreenParams.xy / _BlockSize);
                                                                                // BayerMatrixから閾値をとってくる
                                                                                        float threshold = tex2D(_BayerTex, uv_BayerTex).r;
                                                                                        // 閾値未満は描画しない
                                                                                        clip(clamp_distance - threshold);

                                                                                        return col;
                                                                                            }
                                                                                            ENDCG
                                                                                        }
        }
}