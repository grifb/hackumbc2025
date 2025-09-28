shader_type spatial;
render_mode unshaded, blend_mix, depth_draw_opaque, cull_front, diffuse_toon, specular_toon;

uniform vec3 base_color: source_color;
uniform vec3 top_color: source_color;
uniform float blend_amount:hint_range(0.0, 1.0, 0.1)=0.5;
uniform sampler2D emission_gradient;
uniform sampler2D color_gradient;
uniform sampler2D fresnel_gradient;

float fresnel (float amount, vec3 normal, vec3 view)
{
	return pow((1.0 - clamp(dot(normalize(normal), normalize(view)), 0.0, 1.0)), amount);
}

void vertex() {

}

void fragment() {
	float basic_fresnel = 0.0;
	vec4 graident_emission  = texture(emission_gradient, vec2(UV.x, UV.y));
	ALBEDO = graident_emission.rgb;// mix( top_color,base_color, UV.y*blend_amount) + (basic_fresnel * texture(fresnel_gradient, vec2(basic_fresnel, 0.0)).rgb * 1.0);
	
	//EMISSION = graident_emission.rgb;
}

void light() {
	float dotproduct = clamp(dot(NORMAL, LIGHT), -0.9, 0.9);
	float sample = clamp((dotproduct + 1.0) * ATTENUATION / 2.1, 0.05, 0.95);
	vec4 shaded = texture(color_gradient, vec2(sample, 0.0));
	DIFFUSE_LIGHT += clamp((shaded.rgb * LIGHT_COLOR), 0.0, 1.0);
	//DIFFUSE_LIGHT += (shaded.rgb * LIGHT_COLOR);
}