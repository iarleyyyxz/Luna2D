#version 460 core

out vec4 FragColor;
in vec2 TexCoord;
e
uniform sampler2D uTexture;

void main()
{
    FragColor = texture(uTexture, TexCoord);
}
