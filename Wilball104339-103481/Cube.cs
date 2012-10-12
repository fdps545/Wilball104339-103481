﻿//using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System.Windows.Forms;

namespace Wilball104339103481
{
    class Cube : GameWindow
    {/**
        private Matrix4 cameraMatrix;
        private float speed, mouseX, forwardZ, sideX, rotation, crement, crement2, crement3, look, jump, up, gravity, rot;
        private bool zoomed, direction;
        private int n, awesomesauce;
        Random rand = new Random();   


        #region Cube information

        float[] cubeColors = {

            
			1.0f, 0.0f, 0.0f, 1.0f,
			0.0f, 1.0f, 0.0f, 1.0f,
			0.0f, 0.0f, 1.0f, 1.0f,
			0.0f, 1.0f, 1.0f, 1.0f,
			1.0f, 0.0f, 0.0f, 1.0f,
			0.0f, 1.0f, 0.0f, 1.0f,
			0.0f, 0.0f, 1.0f, 1.0f,
			0.0f, 1.0f, 1.0f, 1.0f,
		};

        byte[] triangles =
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

        /*float[] cube = {
			-0.5f,  0.5f,  0.5f, // vertex[0]
			 0.5f,  0.5f,  0.5f, // vertex[1]
			 0.5f, -0.5f,  0.5f, // vertex[2]
			-0.5f, -0.5f,  0.5f, // vertex[3]
			-0.5f,  0.5f, -0.5f, // vertex[4]
			 0.5f,  0.5f, -0.5f, // vertex[5]
			 0.5f, -0.5f, -0.5f, // vertex[6]
			-0.5f, -0.5f, -0.5f, // vertex[7]
		};*/
        float y_off_set = 1f;
        float[] cube = {
			-0.5f,  0.5f,  0.5f, // vertex[0]
			 0.5f,  0.5f,  0.5f, // vertex[1]
			 0.5f, -0.5f,  0.5f, // vertex[2]
			-0.5f, -0.5f,  0.5f, // vertex[3]
			-0.5f,  0.5f, -0.5f, // vertex[4]
			 0.5f,  0.5f, -0.5f, // vertex[5]
			 0.5f, -0.5f, -0.5f, // vertex[6]
			-0.5f, -0.5f, -0.5f, // vertex[7]
		};

        #endregion

        Matrix4 matrixProjection, matrixModelview;
        float cameraRotation = 0f;

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            //this is added
            GL.EnableClientState(EnableCap.VertexArray);
            GL.EnableClientState(EnableCap.ColorArray);
            
            //cameraMatrix = Matrix4.CreateRotationY(10);
            cameraMatrix = Matrix4.CreateTranslation(-75f, 0f, -75f);
            Cursor.Hide();
            Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Right / 2, Screen.PrimaryScreen.Bounds.Bottom / 2);

            zoomed = false;
            rotation = 1f;
            crement = 0.03f;
            n = 0;
            awesomesauce = 1;
            crement3 = 0.1f;
            gravity = 0;
        }
        

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            matrixProjection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1f, 100f);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref matrixProjection);
            //added
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadMatrix(ref cameraMatrix);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            #region Camera

            cameraRotation = (cameraRotation < 360f) ? (cameraRotation + 10f * (float)e.Time) : 0f;
            //cameraRotation = (cameraRotation < -1f) ? (cameraRotation + 1f * (float)e.Time) : 1f;
            Matrix4.CreateRotationX(cameraRotation, out matrixModelview);
            //Matrix4.CreateTranslation(0, 0, cameraRotation, out matrixModelview);
            matrixModelview *= Matrix4.LookAt(0f, 0f, -5f, 0f, 0f, 0f, 0f, 1f, 0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref matrixModelview);

            #endregion

            #region Draw cube

            GL.VertexPointer(3, VertexPointerType.Float, 0, cube);
            GL.ColorPointer(4, ColorPointerType.Float, 0, cubeColors);
            GL.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedByte, triangles);

            #endregion

            SwapBuffers();
        }

        [STAThread]
       /* private static void Main(string[] args)
        {
            using (Cube p = new Cube())
            {
                p.Run(60d);
            }
        }*/
    }
}
 