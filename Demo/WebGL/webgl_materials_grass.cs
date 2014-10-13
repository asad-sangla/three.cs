﻿using System;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Demo.Misc
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    using Examples;

    using ThreeCs.Cameras;
    using ThreeCs.Extras.Geometries;
    using ThreeCs.Materials;
    using ThreeCs.Math;
    using ThreeCs.Objects;
    using ThreeCs.Renderers;
    using ThreeCs.Scenes;
    using ThreeCs.Textures;

    [Example("webgl_materials_grass", ExampleCategory.WebGL, "materials", 0.6f)]
    class webgl_materials_grass : Example
    {
        private PerspectiveCamera camera;

        private Scene scene;

        private Mesh mesh;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Bitmap generateTexture() 
        {
            var canvas = new Bitmap(512, 512);

            using (var context = Graphics.FromImage(canvas))
            {
                using (Brush brsh = new SolidBrush(ColorTranslator.FromHtml("#ff00ffff")))
                {
                    context.FillRectangle(brsh, 0,0,canvas.Width, canvas.Height);

					/*
                    for ( var i = 0; i < 200; i ++ ) {

                        //context.fillStyle = 'hsl(0,0%,' + ( Math.random() * 50 + 50 ) + '%)';
                        //context.beginPath();
                        //context.arc( random.NextDouble() * canvas.Width, random.NextDouble() * canvas.Height, random.NextDouble() + 0.15, 0, Math.PI * 2, true );
                        //context.fill();
                    }
					*/

	                for (var i = 0; i < 10000; i++)
	                {
						context.FillRectangle(new SolidBrush(Color.FromArgb(255,random.Next(256),random.Next(256),random.Next(256))),
										      random.Next(canvas.Width), random.Next(canvas.Height), random.Next(10), random.Next(10));
	                }
                }
                //context.globalAlpha = 0.075;
                //context.globalCompositeOperation = 'lighter';
            }

			// to BART:
			// for testing purposes, to see what is drawn in the texture, we can save the image to disk
			//canvas.Save(@"d:\test.png");

			return canvas;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public override void Load(Control control)
        {
            base.Load(control);
            
            camera = new PerspectiveCamera(60, control.Width / (float)control.Height, 1, 1000);
			camera.Position = new Vector3( 0, 75, 100 );

			scene = new Scene();

			var geometry = new PlaneGeometry( 100, 100 );

			var texture = new Texture( generateTexture() );
			texture.needsUpdate = true;

			for ( var i = 0; i < 15; i ++ ) {

				var material = new MeshBasicMaterial() {
					color = new Color().setHSL( 0.3f, 0.75f, ( i / 15.0f ) * 0.4f + 0.1f ),
					map = texture,
					depthTest = false,
					depthWrite = false,
					transparent = true
				} ;

				mesh = new Mesh( geometry, material );

				mesh.Position.Y = i * 0.25f;
				mesh.Rotation.X = - (float)Math.PI / 2;

				scene.Add( mesh );

			}

			scene.Children.Reverse();

            renderer.SortObjects = false;
            renderer.SetClearColor((Color)colorConvertor.ConvertFromString("#003300"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSize"></param>
        public override void Resize(Size clientSize)
        {
            Debug.Assert(null != this.camera);
            Debug.Assert(null != this.renderer);

            this.camera.Aspect = clientSize.Width / (float)clientSize.Height;
            this.camera.UpdateProjectionMatrix();

            this.renderer.size = clientSize;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Render()
        {
            Debug.Assert(null != this.mesh);
            Debug.Assert(null != this.renderer);

            var time = stopWatch.ElapsedMilliseconds / 6000.0f;

            camera.Position.X = 80 * (float)System.Math.Cos(time);
            camera.Position.Z = 80 * (float)System.Math.Sin(time);

            camera.LookAt(scene.Position);

            for (var i = 0; i < scene.Children.Count; i++)
            {
                var mesh = scene.Children[i];

                mesh.Position.X = (float)System.Math.Sin(time * 4) * i * i * 0.005f;
                mesh.Position.Z = (float)System.Math.Cos(time * 6) * i * i * 0.005f;

            }

            renderer.Render(scene, camera);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Unload()
        {
            this.scene.Dispose();
            this.camera.Dispose();

            base.Unload();
        }
    }
}