using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectDraw;

namespace Project1
{
    public class kap : MeshContainer
    {
         Texture[] dokular = null;
         

        public Texture[] getDoku() { return dokular; }
        public void setDoku(Texture[] textures) { dokular = textures; }

    
    }
}
