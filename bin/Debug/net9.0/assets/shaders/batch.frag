#version 330 core

in vec2 vUV;
in vec4 vColor;
flat in int vTexIndex;

out vec4 FragColor;

uniform sampler2D uTextures[16];

void main()
{
    int tid = clamp(vTexIndex, 0, 15);
    vec4 texColor = texture(uTextures[tid], vUV);
    FragColor = texColor * vColor;
}
