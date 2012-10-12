using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Windows.Forms;



/**
SOURCES:
 * Cube - http://www.opentk.com/node/2873 
 * Enzo and Patsen's Positioning Grid - The nested for-loop
**/
namespace Wilball104339_103481
{
    class MainProgram : GameWindow
    {
	//input random here
        private Matrix4 cameraMatrix;
        private float speed, mouseX, mouseY, forwardZ, sideX, rotation, crement, crement2, crement3, look, jump, up, gravity, rot;
        private bool zoomed, direction;
        private int n, awesomesauce;
        Random randomFactor = new Random();
		
		Matrix4 matrixProjection, matrixModelview, renderMatrix;
        float cameraRotation;
        float rainDown;

		 public MainProgram() : base(Screen.PrimaryScreen.Bounds.Right, Screen.PrimaryScreen.Bounds.Bottom)
        {
            VSync = VSyncMode.On;
		}
	
		#region Cubes

        float[] cubeColors = {
			1, 0, 0,
            Color.Orange.R, Color.Orange.G, Color.Orange.B,
            Color.Yellow.R, Color.Yellow.G, Color.Yellow.B,
            0, 1, 1,
            Color.Gray.R, Color.Gray.G, Color.Gray.B,
            Color.Indigo.R, Color.Indigo.G, Color.Indigo.B,
            1, 0, 0,
            Color.Black.R, Color.Black.G, Color.Black.B,
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
		
		protected override void OnLoad(EventArgs e)
		{
            base.OnLoad(e);
			GL.ClearColor(0,0,0,0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.EnableClientState(EnableCap.VertexArray);
            GL.EnableClientState(EnableCap.ColorArray);

            cameraMatrix = Matrix4.CreateTranslation(-100f, 20f, -100f);
            Cursor.Hide();
            Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Right / 2, Screen.PrimaryScreen.Bounds.Bottom / 2);

            zoomed = false;
            rotation = 1f;
            crement = 0.08f;
            n = 0;
            crement3 = 0.1f;
            gravity = 0;
		}
		
		 protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            matrixProjection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1f, 100f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref matrixProjection);
        }
		
		protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadMatrix(ref cameraMatrix);

            /*#region Camera

            cameraRotation = (cameraRotation < 360f) ? (cameraRotation + 10f * (float)e.Time) : 0f;
            Matrix4.CreateRotationX(cameraRotation, out matrixModelview);
            matrixModelview *= Matrix4.LookAt(0f, 0f, -5f, 0f, 0f, 0f, 0f, 1f, 0f); //eye, target, up
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref matrixModelview);
            
            #endregion */
            //matrixModelview *= Matrix4.LookAt(0f, 0f, -50f, 0f, 0f, 50f, 0f, 50f, 0f);
            //GL.LoadMatrix(ref matrixModelview);

            for (int x = 0; x <= 100; x++)
            {
                for (int z = 0; z <= 100; z++)
                {
                    GL.PushMatrix();
                    GL.Translate((float)x * 2f ,0f, (float) z * 2f );
                    if (x % 4 == 0)
                    {   
                        if ( z % 4 == 0)
                            drawCubes();
                    }
                    //GL.End();
                    GL.PopMatrix();
                    
                }
            }
			    //drawCubes(0,0);
            
            
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (Keyboard[Key.Escape])
                Exit();
            //actual movement of objects when camera is moved
            if (Keyboard[Key.Down])
                lowerDownBoxes();
            
            if (Keyboard[Key.Up])
                liftUpBoxes();

            if (Keyboard[Key.Left])
                slideLeftBoxes();//object movements
            if (Keyboard[Key.Right])
                slideRightBoxes();
            //added
            speed = 15f * (float)e.Time;
            look = 150f;
            forwardZ = 0f;
            sideX = 0f;
            up = 0;

            #region controls
            /*#region zoom
            if (zoomed)
            {
                speed /= 10;
                look /= 2;
                if (OpenTK.Input.Mouse.GetState().IsButtonDown(MouseButton.Right))
                {
                    cameraMatrix *= Matrix4.CreateTranslation(0f, 0f, -3f);
                    zoomed = false;
                }
            }
            else
            {
                if (OpenTK.Input.Mouse.GetState().IsButtonDown(MouseButton.Left) && !zoomed)
                {
                    cameraMatrix *= Matrix4.CreateTranslation(0f, 0f, 3f);
                    zoomed = true;
                }
            }
            #endregion*/
            #region WASD
            //camera view movement
            if (Keyboard[Key.ShiftLeft] || Keyboard[Key.ShiftRight])
            {
                speed *= 1.5f;
            }
            //if (Keyboard[Key.Space])
            //{
            //    up -= 0.2f;
            //    //up = (up > 1f) ? (up - 0.1f) : 0f;	
            //}
            if (Keyboard[Key.W])
            {
                forwardZ = speed;
            }
            else if (Keyboard[Key.S])
            {
                forwardZ = -speed;
            }

            if (Keyboard[Key.A])
            {
                sideX = speed;
            }

            else if (Keyboard[Key.D])
            {
                sideX = -speed;
            }


            #endregion
            #endregion
            //if (up < 0)
            //{
            //    gravity = 0.1f;
            //}
            //else
            //{
            //    gravity = 0;
            //}
            cameraMatrix *= Matrix4.CreateTranslation(sideX, up, forwardZ);

           
            if (Keyboard[Key.ShiftRight])
            {
                cameraMatrix *= Matrix4.CreateRotationY(mouseX);
                mouseX = Mouse.XDelta / look;
            }

            if (Keyboard[Key.ShiftLeft])
            {
                cameraMatrix *= Matrix4.CreateRotationX(mouseY);
                mouseY = Mouse.YDelta / look;
            }

            //cameraMatrix *= Matrix4.CreateRotationY(mouseX);
            //cameraMatrix *= Matrix4.CreateRotationX(mouseY);
            //mouseX = Mouse.YDelta / look;
            //mouseY = Mouse.YDelta / look;
            if (Cursor.Position.X >= Screen.PrimaryScreen.Bounds.Right - 1)
            {
                /*cameraMatrix *= Matrix4.CreateRotationY(mouseX);
                mouseX = Mouse.XDelta / look;*/
                Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Right - 1, Cursor.Position.Y);
            }
            else if (Cursor.Position.X <= Screen.PrimaryScreen.Bounds.Left)
            {

                Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Left, Cursor.Position.Y);
            }

            if ((Cursor.Position.Y >= Screen.PrimaryScreen.Bounds.Top - 1))// || Keyboard[Key.ShiftRight]))
            {
                Cursor.Position = new Point(Cursor.Position.X, Screen.PrimaryScreen.Bounds.Top - 1);
            }
            else if ((Cursor.Position.Y <= Screen.PrimaryScreen.Bounds.Bottom))// || Keyboard[Key.ShiftRight]))
            {
                Cursor.Position = new Point(Cursor.Position.X, Screen.PrimaryScreen.Bounds.Bottom);
            }

            if (Keyboard[Key.Escape])
                Exit();

        }

        public void lowerDownBoxes()
        {
            const float factor = 0.5f;
            for (int i = 0; i < cube.Length; i++)
            {
                if ((i + 1) % 3 == 2)
                    cube[i] -= factor;
            }
            drawCubes();
        }

        public void slideLeftBoxes()
        {
            const float factor = 0.5f;
            for (int i = 0; i < cube.Length; i++)
            {
                if ((i + 1) % 3 == 1)
                    cube[i] -= factor;
            }
            drawCubes();
        }

        public void slideRightBoxes() 
        {
            const float factor = 0.5f;
            for (int i = 0; i < cube.Length; i++)
            {
                if ((i + 1) % 3 == 1)
                    cube[i] += factor;
            }
            drawCubes();
        }

        public void liftUpBoxes()
        {
            const float factor = 0.5f;
            for (int i = 0; i < cube.Length; i++)
            {
                if ((i + 1) % 3 == 2)
                    cube[i] += factor;
            }
            drawCubes();   
        }

        public void drawCubes()
		{
			#region DrawCube

            GL.VertexPointer(3, VertexPointerType.Float, 0, cube);
            GL.ColorPointer(4, ColorPointerType.Float, 0, cubeColors);
            GL.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedByte, cubeTriangles);

            #endregion
		}
		
		[STAThread]
        private static void Main(string[] args)
        {
            using (MainProgram p = new MainProgram())
            {
                p.Run(30.0);
            }
        }

        void generateExplosion()
        {
            //GL.DrawArrays(BeginMode.Points, 0, 4);
        }
		
	}
}

    

