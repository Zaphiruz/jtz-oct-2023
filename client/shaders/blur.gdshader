shader_type canvas_item;
// FROM https://www.shadertoy.com/view/Xltfzj

// GAUSSIAN BLUR SETTINGS {{{
    uniform float Directions = 16.0; // BLUR DIRECTIONS (Default 16.0 - More is better but slower)
    uniform float Quality = 3.0; // BLUR QUALITY (Default 4.0 - More is better but slower)
    uniform float Size = 8.0; // BLUR SIZE (Radius)
// GAUSSIAN BLUR SETTINGS }}}
   
void fragment() {
	vec2 Radius = Size * TEXTURE_PIXEL_SIZE;
	// Normalized pixel coordinates (from 0 to 1)
    vec2 uv = UV;
	// Pixel colour
    vec4 Color = COLOR;
	
	// Blur calculations
    for( float d=0.0; d<PI; d+=PI/Directions)
    {
		for(float i=1.0/Quality; i<=1.0; i+=1.0/Quality)
        {
			Color += texture( TEXTURE, uv+vec2(cos(d),sin(d))*Radius*i);		
        }
    }
	
	// Output to screen
    Color /= Quality * Directions - 15.0;
    COLOR =  Color;
}