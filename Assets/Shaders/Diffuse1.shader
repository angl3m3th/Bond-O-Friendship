﻿Shader "Player View Stencil/Diffuse1" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "Queue"="Geometry"}
		//
		Pass {
			Stencil {
				Ref 1
				Comp equal
				Pass keep
				Fail keep
				ZFail keep
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 _Color;

			struct appdata {
				float4 vertex : POSITION;
			};
			struct v2f {
				float4 pos : SV_POSITION;
			};
			v2f vert(appdata v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}
			half4 frag(v2f i) : COLOR {
				return _Color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}