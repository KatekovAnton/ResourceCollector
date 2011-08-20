/* hello */
uniform extern float4x4 World;

uniform extern float4x4 View;

uniform extern float4x4 Projection;

uniform extern texture DiffuseTexture;

uniform extern float3 SunDirection;
uniform extern float3x3 WorldRotation;

uniform extern float4x4 Frame[28];



//����
float4x4 LightViewProj;
float3 LightDirection = float3(-1, -1, -1);
float4 AmbientColor = float4(0.15, 0.15, 0.15, 0);
float DepthBias = 0.001f;


struct Vertex
{
 
  float4 Position : POSITION;

  float3 Normal : NORMAL;

  float2 TextureCoordinate : TEXCOORD;

};



struct SkinnedVertex

{

  float4 Position : POSITION;

  float3 Normal : NORMAL;

  float2 TextureCoordinate : TEXCOORD;

  float3 BoneIndices : BLENDINDICES;

  float3 BoneWeights : BLENDWEIGHT;

};



struct PS_INPUT

{

  float4 Position : POSITION;

  float3 Normal : COLOR0;

  float2 TextureCoordinate : TEXCOORD0;
  float4 WorldPos : TEXCOORD1;
};



sampler TextureSampler = sampler_state

{

  Texture = <DiffuseTexture>;

  mipfilter = LINEAR;

};



texture ShadowMap;
sampler ShadowMapSampler = sampler_state
{
    Texture = <ShadowMap>;
};
struct CreateShadowMap_VSOut
{
    float4 Position : POSITION;
    float Depth     : TEXCOORD0;
};

CreateShadowMap_VSOut CreateShadowMap_StaticVertexShader(Vertex vertex)
{
    CreateShadowMap_VSOut Out;
    Out.Position = mul(vertex.Position, mul(World, LightViewProj)); 
    Out.Depth = Out.Position.z / Out.Position.w;    
    return Out;
}
CreateShadowMap_VSOut CreateShadowMap_SkinnedVertexShader(SkinnedVertex vertex)
{
    CreateShadowMap_VSOut Out;
    float d = 1.0 / (vertex.BoneWeights[1]+vertex.BoneWeights[0]);
    float4x4 animation = Frame[(int)vertex.BoneIndices[0]]*vertex.BoneWeights[0]*d;
    animation += Frame[(int)vertex.BoneIndices[1]]*vertex.BoneWeights[1]*d;

  
    Out.Position = mul(mul(mul(vertex.Position, animation), World), LightViewProj);
    Out.Depth = Out.Position.z / Out.Position.w;    
    return Out;
}

float4 CreateShadowMap_PixelShader(CreateShadowMap_VSOut input) : COLOR
{ 
    return float4(input.Depth, 0, 0, 0);
}












PS_INPUT StaticVertexShaderSM(Vertex vertex)

{

  PS_INPUT f = (PS_INPUT)0;


  f.Position = mul(mul(mul(vertex.Position, World), View), Projection);

  f.TextureCoordinate = vertex.TextureCoordinate;
  
  f.Normal = normalize(mul(vertex.Normal, (float3x3)World));  
 
 f.WorldPos = mul(vertex.Position, World);
 return f;

}


PS_INPUT StaticVertexShaderNoSM(Vertex vertex)

{

  PS_INPUT f = (PS_INPUT)0;


  f.Position = mul(mul(mul(vertex.Position, World), View), Projection);

  f.TextureCoordinate = vertex.TextureCoordinate;
  
  f.Normal = normalize(mul(vertex.Normal, (float3x3)World));  
 
 //f.WorldPos = mul(vertex.Position, World);
 return f;

}
PS_INPUT SkinnedVertexShaderSM(SkinnedVertex vertex)
{
    PS_INPUT f = (PS_INPUT)0;



    float d = 1.0 / (vertex.BoneWeights[1]+vertex.BoneWeights[0]);
    float4x4 animation = Frame[(int)vertex.BoneIndices[0]]*vertex.BoneWeights[0];
    animation += Frame[(int)vertex.BoneIndices[1]]*vertex.BoneWeights[1];
    // animation += Frame[(int)vertex.BoneIndices[2]]*vertex.BoneWeights[2];



    f.Position = mul(mul(mul(mul(vertex.Position, animation), World), View), Projection);
    f.Normal = normalize(mul(mul(vertex.Normal,(float3x3)animation), (float3x3)World));
    f.TextureCoordinate = vertex.TextureCoordinate;

	f.WorldPos = mul(mul(vertex.Position, animation), World);
    return f;
}

PS_INPUT SkinnedVertexShaderNoSM(SkinnedVertex vertex)
{
    PS_INPUT f = (PS_INPUT)0;



    float d = 1.0 / (vertex.BoneWeights[1]+vertex.BoneWeights[0]);
    float4x4 animation = Frame[(int)vertex.BoneIndices[0]]*vertex.BoneWeights[0];
    animation += Frame[(int)vertex.BoneIndices[1]]*vertex.BoneWeights[1];
    // animation += Frame[(int)vertex.BoneIndices[2]]*vertex.BoneWeights[2];



    f.Position = mul(mul(mul(mul(vertex.Position, animation), World), View), Projection);
    f.Normal = normalize(mul(mul(vertex.Normal,(float3x3)animation), (float3x3)World));
    f.TextureCoordinate = vertex.TextureCoordinate;

    return f;
}




//===============================PIXEL SHADER======================================

float4 SolidTextureNoSM(PS_INPUT f) : COLOR0
{
float shadowintens = 0.4f;
	float minclamp = 0.1;
	


	float shadowcoeff =0;
	float lambertfactor = clamp( dot( -f.Normal,normalize(LightDirection)),minclamp,1.0f)+shadowintens - minclamp;

lambertfactor*=1.1;
    
    float3 color = tex2D(TextureSampler, f.TextureCoordinate).rgb* lambertfactor;
    return float4(color,1);

}

float4 SolidTextureSM(PS_INPUT f) : COLOR0
{
	float4 lightingPosition = mul(f.WorldPos, LightViewProj);
	float2 ShadowTexCoord = 0.5 * lightingPosition.xy / lightingPosition.w + float2( 0.5, 0.5 );
	ShadowTexCoord.y = 1.0f - ShadowTexCoord.y;
	float shadowdepth = tex2D(ShadowMapSampler, ShadowTexCoord).r;    
	float ourdepth = (lightingPosition.z / lightingPosition.w) - DepthBias;
	


	float shadowintens = 0.4f;
	float minclamp = 0.1;
	


	float shadowcoeff =0;
	float lambertfactor = clamp( dot( -f.Normal,normalize(LightDirection)),minclamp,1.0f)+shadowintens - minclamp;

	if (shadowdepth < ourdepth)
	{
		shadowcoeff = 0.15;
		if(lambertfactor !=shadowintens)
		{
			lambertfactor = shadowintens;
		}
	}
lambertfactor*=1.1;
	float3 color = tex2D(TextureSampler, f.TextureCoordinate).rgb* lambertfactor;
	return float4(color,1);
}



float4 SolidTextureSMSmooth(PS_INPUT f) : COLOR0
{
	float4 lightingPosition = mul(f.WorldPos, LightViewProj);
	float2 ShadowTexCoord = 0.5 * lightingPosition.xy / lightingPosition.w + float2( 0.5, 0.5 );
	ShadowTexCoord.y = 1.0f - ShadowTexCoord.y;
    
	float ourdepth = (lightingPosition.z / lightingPosition.w) - DepthBias;
	


	float shadowintens = 0.4f;
	float minclamp = 0.1;
	


	float shadowcoeff =0;
	float lambertfactor = clamp(dot(-f.Normal,normalize(LightDirection)), minclamp, 1.0f)+shadowintens - minclamp;


	if(tex2D(ShadowMapSampler, ShadowTexCoord).r < ourdepth)
		shadowcoeff = 0.35;
	if(tex2D(ShadowMapSampler, ShadowTexCoord + float2( -1.0/2048.0, -1.0/2048.0)).x < ourdepth)
		shadowcoeff += 0.0625;
	if(tex2D(ShadowMapSampler, ShadowTexCoord + float2( -1.0/2048.0, 0)).x < ourdepth)	
		shadowcoeff += 0.125;
	if(tex2D(ShadowMapSampler, ShadowTexCoord + float2( -1.0/2048.0, 1.0/2048.0)).x < ourdepth)
		shadowcoeff += 0.0625;
	if(tex2D(ShadowMapSampler, ShadowTexCoord + float2( 0, -1.0/2048.0)).x < ourdepth)
		shadowcoeff += 0.125;
	if(tex2D(ShadowMapSampler, ShadowTexCoord + float2( 0, 1.0/2048.0)).x < ourdepth)
		shadowcoeff += 0.125;
	if(tex2D(ShadowMapSampler, ShadowTexCoord + float2( 1.0/2048.0, -1.0/2048.0)).x < ourdepth)
		shadowcoeff += 0.0625;
	if(tex2D(ShadowMapSampler, ShadowTexCoord + float2( 1.0/2048.0, 0)).x < ourdepth)
		shadowcoeff += 0.125;
	if(tex2D(ShadowMapSampler, ShadowTexCoord + float2( 1, 1.0/2048.0)).x < ourdepth)
		shadowcoeff += 0.0625;
	

	shadowcoeff = 0.95 - shadowcoeff * shadowintens;


	if(shadowcoeff!=1.0 && lambertfactor > shadowcoeff)
		lambertfactor = shadowcoeff;


	lambertfactor*=1.1;

	float3 color = tex2D(TextureSampler, f.TextureCoordinate).rgb* lambertfactor;
	return float4(color,1);
}
//=================================================================================



float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);    
}















//������ ��� ���������
technique AnimRenderNoSM
{
  pass P0
  {
    VertexShader = compile vs_2_0 SkinnedVertexShaderNoSM();
    PixelShader = compile ps_2_0 SolidTextureNoSM();
  }
}

technique NotAnimRenderNoSM
{
  pass P0
  {
    VertexShader = compile vs_2_0 StaticVertexShaderSM();
    PixelShader = compile ps_2_0 SolidTextureNoSM();
  }
}


//������ �� ���������� ����������
technique AnimRenderSMSmooth
{
  pass P0
  {
    VertexShader = compile vs_2_0 SkinnedVertexShaderSM();
    PixelShader = compile ps_2_0 SolidTextureSMSmooth();
  }
}

technique NotAnimRenderSMSmooth
{
  pass P0
  {
    VertexShader = compile vs_2_0 StaticVertexShaderSM();
    PixelShader = compile ps_2_0 SolidTextureSMSmooth();
  }
}
//������ � ������������ ����������
technique AnimRenderSM
{
  pass P0
  {
    VertexShader = compile vs_2_0 SkinnedVertexShaderSM();
    PixelShader = compile ps_2_0 SolidTextureSM();
  }
}

technique NotAnimRenderSM
{
  pass P0
  {
    VertexShader = compile vs_2_0 StaticVertexShaderSM();
    PixelShader = compile ps_2_0 SolidTextureSM();
  }
}

////�������� ���������
technique CreateStaticShadowMap
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 CreateShadowMap_StaticVertexShader();
        PixelShader = compile ps_2_0 CreateShadowMap_PixelShader();
    }
}
technique CreateAnimShadowMap
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 CreateShadowMap_SkinnedVertexShader();
        PixelShader = compile ps_2_0 CreateShadowMap_PixelShader();
    }
}
//��������� ������� �� ����� � ���� �� ��������