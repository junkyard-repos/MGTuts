// Our texture sampler

float xSize;
float ySize;
float xDraw;
float yDraw;

float4 filterColor;


texture Texture;
sampler TextureSampler = sampler_state
{
	Texture = <Texture>;
};

// This data comes from the sprite batch vertex shader
struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCordinate : TEXCOORD0;
};

// Our pixel shader
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
	float4 texColor = tex2D(TextureSampler, input.TextureCordinate);


	float4 color = float4(texColor.r, texColor.g, texColor.b, texColor.a);



	return color * filterColor;
}

// Compile our shader
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}