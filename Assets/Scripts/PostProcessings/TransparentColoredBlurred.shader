Shader "Unlit/TransparentColoredBlurred"
{
	Properties
	{
	  _MainTex("Base (RGB), Alpha (A)", 2D) = "white" {}
	  _Distance("Distance", Float) = 0.015
	}

		SubShader
	  {
		LOD 100

		Tags
		{
		  "Queue" = "Transparent"
		  "IgnoreProjector" = "True"
		  "RenderType" = "Transparent"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Offset -1, -1
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
		  CGPROGRAM
		  #pragma vertex vertexProgram
		  #pragma fragment fragmentProgram

		  #include "UnityCG.cginc"

		  struct appdata_t
		  {
			float4 vertex : POSITION;
			float2 textureCoordinate : TEXCOORD0;
			fixed4 color : COLOR;
		  };

		  struct vertexToFragment
		  {
			float4 vertex : SV_POSITION;
			half2 textureCoordinate : TEXCOORD0;
			fixed4 color : COLOR;
		  };

		  sampler2D _MainTex;
		  float4 _MainTex_ST;
		  float _Distance;

		  vertexToFragment vertexProgram(appdata_t vertexData)
		  {
			vertexToFragment output;
			output.vertex = UnityObjectToClipPos(vertexData.vertex);
			output.textureCoordinate = TRANSFORM_TEX(vertexData.textureCoordinate, _MainTex);
			output.color = vertexData.color;
			return output;
		  }

		  fixed4 fragmentProgram(vertexToFragment input) : COLOR
		  {
			float distance = _Distance;
			fixed4 computedColor = tex2D(_MainTex, input.textureCoordinate) * input.color;
			computedColor += tex2D(_MainTex, half2(input.textureCoordinate.x + distance , input.textureCoordinate.y + distance)) * input.color;
			computedColor += tex2D(_MainTex, half2(input.textureCoordinate.x + distance , input.textureCoordinate.y)) * input.color;
			computedColor += tex2D(_MainTex, half2(input.textureCoordinate.x , input.textureCoordinate.y + distance)) * input.color;
			computedColor += tex2D(_MainTex, half2(input.textureCoordinate.x - distance , input.textureCoordinate.y - distance)) * input.color;
			computedColor += tex2D(_MainTex, half2(input.textureCoordinate.x + distance , input.textureCoordinate.y - distance)) * input.color;
			computedColor += tex2D(_MainTex, half2(input.textureCoordinate.x - distance , input.textureCoordinate.y + distance)) * input.color;
			computedColor += tex2D(_MainTex, half2(input.textureCoordinate.x - distance , input.textureCoordinate.y)) * input.color;
			computedColor += tex2D(_MainTex, half2(input.textureCoordinate.x , input.textureCoordinate.y - distance)) * input.color;
			computedColor = computedColor / 9;

			return computedColor;
		  }
		  ENDCG
		}
	  }
}