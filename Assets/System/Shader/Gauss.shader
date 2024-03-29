﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Gauss"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		CGINCLUDE

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

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			return o;
		}

		sampler2D _MainTex;
		float2 _PixelSize;

		fixed4 fragHorizon(v2f i) : SV_Target
		{
			float4 col = float4(0, 0, 0, 0);
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(-3, 0)) * 0.053;
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(-2, 0)) * 0.123;
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(-1, 0)) * 0.203;
			col += tex2D(_MainTex, i.uv) * 0.240;
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(1, 0)) * 0.203;
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(2, 0)) * 0.123;
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(3, 0)) * 0.053;
			return col;
		}
		fixed4 fragVertical(v2f i) : SV_Target
		{
			float4 col = float4(0, 0, 0, 0);
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(0, -3)) * 0.053;
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(0, -2)) * 0.123;
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(0, -1)) * 0.203;
			col += tex2D(_MainTex, i.uv) * 0.240;
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(0, 1)) * 0.203;
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(0, 2)) * 0.123;
			col += tex2D(_MainTex, i.uv + _PixelSize * float2(0, 3)) * 0.053;
			return col;
		}
		ENDCG

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragHorizon

			#include "UnityCG.cginc"

			ENDCG
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragVertical

			#include "UnityCG.cginc"

			ENDCG
		}
	}
}