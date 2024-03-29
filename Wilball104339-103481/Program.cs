﻿using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;

/**
SOURCES:
**/
namespace Wilball104339_103481
{
    class Program : GameWindow
    {
	//input random here
		
		Matrix4 matrixProjection, matrixModelview, cameraMatrix, renderMatrix;

		 public Game()
            : base(Screen.PrimaryScreen.Bounds.Right, Screen.PrimaryScreen.Bounds.Bottom)
        {
			
		}
	
		#region Cubes

        float[] cubeColors = {
			1, 0, 0,
            Color.Orange.R, Color.Orange.G, Color.Orange.B,
            Color.Yellow.R, Color.Yellow.G, Color.Yellow.B,
            0, 1, 1,
            1, 0, 0,
            Color.Indigo.R, Color.Indigo.G, Color.Indigo.B,
            1, 0, 0,
            Color.Orange.R, Color.Orange.G, Color.Orange.B,
		};

        byte[] cubeTriangles =
		{
			1, 0, 2, // front
			3, 2, 0,
			6, 4, 5, // back
			4, 6, 7,
			4, 7, 0, // left
			7, 3, 0,
			1, 2, 5, //right
			2, 6, 5,
			0, 1, 5, // top
			0, 5, 4,
			2, 3, 6, // bottom
			3, 7, 6,
		};

        float[] cube = {
			-0.5f,  1f,  0.5f, // vertex[0]
			 0.5f,  1f,  0.5f, // vertex[1]
			 0.5f, -1f,  0.5f, // vertex[2]
			-0.5f, -1f,  0.5f, // vertex[3]
			-0.5f,  1f, -0.5f, // vertex[4]
			 0.5f,  1f, -0.5f, // vertex[5]
			 0.5f, -1f, -0.5f, // vertex[6]
			-0.5f, -1f, -0.5f, // vertex[7]
		};
        #endregion
		
		protected override void OnLoad(EventArgs e)
		{
			GL.ClearColor(Color.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.EnableClientState(EnableCap.VertexArray);
            GL.EnableClientState(EnableCap.ColorArray);
			
		}
		
		 protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            matrixProjection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1f, 100f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref matrixProjection);
        }
		
		protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            #region Camera

            cameraRotation = (cameraRotation < 360f) ? (cameraRotation + 10f * (float)e.Time) : 0f;
            Matrix4.CreateRotationX(cameraRotation, out matrixModelview);
            matrixModelview *= Matrix4.LookAt(0f, 0f, -5f, 0f, 0f, 0f, 0f, 1f, 0f); //eye, target, up
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref matrixModelview);

            #endregion

			drawCubes();

            SwapBuffers();
        }
		
		public void drawCubes()
		{
			#region Draw cube

            GL.VertexPointer(3, VertexPointerType.Float, 0, cube);
            GL.ColorPointer(4, ColorPointerType.Float, 0, cubeColors);
            GL.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedByte, triangles);

            #endregion
		}
		
		[STAThread]
        private static void Main(string[] args)
        {
            using (Program p = new Program())
            {
                p.Run(60d);
            }
        }
		
	}
}
