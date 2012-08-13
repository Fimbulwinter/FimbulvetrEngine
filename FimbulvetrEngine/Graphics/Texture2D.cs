﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace FimbulvetrEngine.Graphics
{
    public class Texture2D : GraphicResource
    {
        public int Texture { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool Loaded { get; private set; }

        private TextureWrapMode _postLoadS;
        private TextureWrapMode _postLoadT;

        public Texture2D()
        {
            Texture = -1;
            _postLoadS = TextureWrapMode.ClampToEdge;
            _postLoadT = TextureWrapMode.ClampToEdge;
        }

        public Texture2D(int width, int height)
            : this(GL.GenTexture(), width, height)
        {
            SetDefaultParamterers();
        }

        public Texture2D(int texture, int width, int height)
        {
            Texture = texture;
            Width = width;
            Height = height;
            Loaded = false;
        }

        protected override void GCUnmanagedFinalize()
        {
            if (Texture != 0)
                GL.DeleteTexture(Texture);
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void SetData(PixelFormat format, PixelInternalFormat iFormat, PixelType type, IntPtr data)
        {
            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, iFormat, Width, Height, 0, format, type, data);
        }

        public void SetData<T>(PixelFormat format, PixelInternalFormat iFormat, PixelType type, T[] data) where T : struct
        {
            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, iFormat, Width, Height, 0, format, type, data);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture);
        }

        public void SetWrapMode(TextureWrapMode s, TextureWrapMode t)
        {
            if (Texture != -1)
            {
                Bind();
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)s);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)t);
            }
            else
            {
                _postLoadS = s;
                _postLoadT = t;
            }
        }

        public void SetTexture(int texture)
        {
            Texture = texture;
            SetDefaultParamterers();
        }

        private void SetDefaultParamterers()
        {
            Bind();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)_postLoadS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)_postLoadT);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
        }

        public void SetLoaded()
        {
            Loaded = true;
        }
    }
}
