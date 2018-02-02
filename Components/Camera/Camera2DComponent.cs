using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PokeD.CPGL.Components.Input;
using PokeD.CPGL.Components.ViewportAdapters;
using PokeD.CPGL.Data;
using PokeD.CPGL.Physics.Collision;

namespace PokeD.CPGL.Components.Camera
{
    public abstract class CameraInputHandler : Component
    {
        public class MovementEventArgs : EventArgs
        {

        }

        public class ZoomEventArgs : EventArgs
        {

        }

        public class ViewEventArgs : EventArgs
        {

        }


        public event EventHandler<MovementEventArgs> OnMovement;
        public event EventHandler<ZoomEventArgs> OnView;
        public event EventHandler<ZoomEventArgs> OnZoom;


        protected CameraInputHandler(Component component) : base(component)
        {
        }


        protected virtual void RaiseMovement(MovementEventArgs e) => OnMovement?.Invoke(this, e);
        protected virtual void RaiseView(ZoomEventArgs e) => OnView?.Invoke(this, e);
        protected virtual void RaiseZoom(ZoomEventArgs e) => OnZoom?.Invoke(this, e);
    }

    public class Camera2DInputHandler : CameraInputHandler
    {
        private ICollision2D CameraCollision;
        private ICollisionManager CameraCollisionManager;

        public Camera2DInputHandler(Component component) : base(component)
        {
            KeyboardListener.KeyPressed += (this, KeyboardListener_KeyPressed);
        }

        private void KeyboardListener_KeyPressed(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.LeftShift:
                    RaiseZoom(new ZoomEventArgs()); //ZoomIn();
                    break;

                case Keys.LeftControl:
                    RaiseZoom(new ZoomEventArgs()); //ZoomOut();
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float) gameTime.ElapsedGameTime.TotalSeconds * 60F;

            var keyboardState = Keyboard.GetState();

            // -- Moving
            if (CameraCollisionManager != null)
            {
                var moveVector = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.W))
                    moveVector.Y = -1.0f * dt;

                if (keyboardState.IsKeyDown(Keys.S))
                    moveVector.Y = 1.0f * dt;

                if (keyboardState.IsKeyDown(Keys.A))
                    moveVector.X = -1.0f * dt;

                if (keyboardState.IsKeyDown(Keys.D))
                    moveVector.X = 1.0f * dt;


                if (moveVector != Vector2.Zero)
                {
                    //moveVector = PositionConverter?.Position2DConverterFunc(moveVector) ?? moveVector;
                    RaiseMovement(new MovementEventArgs()); //Move(moveVector);

                    if (CameraCollisionManager.Collides(CameraCollision))
                        RaiseMovement(new MovementEventArgs());  //Move(-moveVector);
                    //else
                    //    PositionChanged?.Position2DChangedAction?.Invoke(Position);
                }
            }
        }

        /// <summary>Shuts down the component.</summary>
        protected override void Dispose(bool disposing)
        {
            KeyboardListener.KeyPressed -= KeyboardListener_KeyPressed;

            base.Dispose(disposing);
        }
    }

    public class Camera2DComponent : Component, IPosition2D
    {
        private const float ZoomStep = 0.125f;

        private float _maximumZoom = float.MaxValue;
        private float _minimumZoom;
        private float _zoom;

        private ViewportAdapter ViewportAdapter { get; }

        public IPosition2DChanged PositionChanged { get; private set; }
        public IPosition2DConverter PositionConverter { get; private set; }

        public PlayerMovementState MovementState { get; private set; }
        public ICollisionManager CollisionManager { get; private set; }
        public ICollision2D Collision { get; private set; }
        public ICameraBorders CameraBorders { get; private set; }


        public Camera2DComponent(PortableGame game) : this(new DefaultViewportAdapter(game)) { }
        public Camera2DComponent(ViewportAdapter viewportAdapter) : base(viewportAdapter)
        {
            ViewportAdapter = viewportAdapter;
            ViewportAdapter.OnResize += (sender, args) => Origin = new Vector2(viewportAdapter.VirtualWidth * 0.5f, viewportAdapter.VirtualHeight * 0.5f);

            Rotation = 0;
            Zoom = 1;
            Origin = new Vector2(viewportAdapter.VirtualWidth * 0.5f, viewportAdapter.VirtualHeight * 0.5f);
            Position = Vector2.Zero;

            KeyboardListener.KeyPressed += (this, KeyboardListener_KeyPressed);
        }

        private void KeyboardListener_KeyPressed(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.LeftShift:
                    ZoomIn();
                    break;

                case Keys.LeftControl:
                    ZoomOut();
                    break;
            }
        }

        public Vector2 Origin { get; set; }

        public float Zoom
        {
            get => _zoom;
            set
            {
                if ((value < MinimumZoom) || (value > MaximumZoom))
                    throw new ArgumentException("Zoom must be between MinimumZoom and MaximumZoom");

                _zoom = value;
            }
        }

        public float MinimumZoom
        {
            get => _minimumZoom;
            set
            {
                if (value < 0)
                    throw new ArgumentException("MinimumZoom must be greater than zero");

                if (Zoom < value)
                    Zoom = MinimumZoom;

                _minimumZoom = value;
            }
        }

        public float MaximumZoom
        {
            get => _maximumZoom;
            set
            {
                if (value < 0)
                    throw new ArgumentException("MaximumZoom must be greater than zero");

                if (Zoom > value)
                    Zoom = value;

                _maximumZoom = value;
            }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                var frustum = GetBoundingFrustum();
                var corners = frustum.GetCorners();
                var topLeft = corners[0];
                var bottomRight = corners[2];
                var width = bottomRight.X - topLeft.X;
                var height = bottomRight.Y - topLeft.Y;
                return new Rectangle((int) topLeft.X, (int) topLeft.Y, (int) width, (int) height);
            }
        }

        public Rectangle CameraBordersRectangle
        {
            get
            {
                var boundingBox = CameraBorders.ObjectBoundingBox;
                var topLeft = boundingBox.Min;
                var bottomRight = boundingBox.Max;
                var width = bottomRight.X - topLeft.X;
                var height = bottomRight.Y - topLeft.Y;
                return new Rectangle((int) topLeft.X + 10, (int) topLeft.Y + 10, (int) width - 20, (int) height - 20);
            }
        }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }

        public void Move(Vector2 direction) => Position += Vector2.Transform(direction, Matrix.CreateRotationZ(-Rotation));

        public void Rotate(float deltaRadians) => Rotation += deltaRadians;

        public void ZoomIn() => ClampZoom(Zoom + ZoomStep);
        public void ZoomOut() => ClampZoom(Zoom - ZoomStep);
        public void ZoomIn(float deltaZoom) => ClampZoom(Zoom + deltaZoom);
        public void ZoomOut(float deltaZoom) => ClampZoom(Zoom - deltaZoom);

        private void ClampZoom(float value)
        {
            if (value < MinimumZoom)
                Zoom = MinimumZoom;
            else
                Zoom = value > MaximumZoom ? MaximumZoom : value;
        }

        public void LookAt(Vector2 position) => Position =
            position - new Vector2(ViewportAdapter.VirtualWidth / 2f, ViewportAdapter.VirtualHeight / 2f);

        public Vector2 WorldToScreen(float x, float y) => WorldToScreen(new Vector2(x, y));

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            var viewport = ViewportAdapter.Viewport;
            return Vector2.Transform(worldPosition + new Vector2(viewport.X, viewport.Y), GetViewMatrix());
        }

        public Vector2 ScreenToWorld(float x, float y) => ScreenToWorld(new Vector2(x, y));

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            var viewport = ViewportAdapter.Viewport;
            return Vector2.Transform(screenPosition - new Vector2(viewport.X, viewport.Y),
                Matrix.Invert(GetViewMatrix()));
        }

        private Matrix GetVirtualViewMatrix(Vector2 parallaxFactor) =>
            Matrix.CreateTranslation(new Vector3(-Position * parallaxFactor, 0.0f)) *
            //Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1) *
            Matrix.CreateTranslation(new Vector3(Origin, 0.0f));

        private Matrix GetVirtualViewMatrix() => GetVirtualViewMatrix(Vector2.One);

        public Matrix GetViewMatrix(Vector2 parallaxFactor) => GetVirtualViewMatrix(parallaxFactor) *
                                                               ViewportAdapter.GetScaleMatrix();

        public Matrix GetViewMatrix() => GetViewMatrix(Vector2.One);
        public Matrix GetInverseViewMatrix() => Matrix.Invert(GetViewMatrix());

        private Matrix GetProjectionMatrix(Matrix viewMatrix)
        {
            var projection = Matrix.CreateOrthographicOffCenter(0, ViewportAdapter.VirtualWidth,
                ViewportAdapter.VirtualHeight, 0, -1, 0);
            Matrix.Multiply(ref viewMatrix, ref projection, out projection);
            return projection;
        }

        public BoundingFrustum GetBoundingFrustum()
        {
            var viewMatrix = GetVirtualViewMatrix();
            var projectionMatrix = GetProjectionMatrix(viewMatrix);
            return new BoundingFrustum(projectionMatrix);
        }

        public ContainmentType Contains(Point point) => Contains(point.ToVector2());

        public ContainmentType Contains(Vector2 vector2) => GetBoundingFrustum()
            .Contains(new Vector3(vector2.X, vector2.Y, 0));

        public ContainmentType Contains(Rectangle rectangle)
        {
            var max = new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 0.5f);
            var min = new Vector3(rectangle.X, rectangle.Y, 0.5f);
            var boundingBox = new BoundingBox(min, max);
            return GetBoundingFrustum().Contains(boundingBox);
        }

        public void MoveTo(Vector2 pos, float rot)
        {
            Position = pos;
            Rotation = rot;
        }

        public void SetPositionConverter(IPosition2DConverter positionConverter) => PositionConverter =
            positionConverter;

        public void SetPositionChangedAction(IPosition2DChanged positionChanged) => PositionChanged = positionChanged;

        public void SetCollisionManager(ICollisionManager collisionManager) => CollisionManager = collisionManager;
        public void SetPlayerCollision(ICollision2D collision) => Collision = collision;

        public void SetCameraBorders(ICameraBorders cameraBorders) => CameraBorders = cameraBorders;

        public override void Update(GameTime gameTime)
        {
            var dt = (float) gameTime.ElapsedGameTime.TotalSeconds * 60F;

            var keyboardState = Keyboard.GetState();

            // -- Moving
            if (CollisionManager != null)
            {
                MovementState = PlayerMovementState.None;
                var moveVector = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.W))
                {
                    MovementState = PlayerMovementState.Down;
                    moveVector.Y = -1.0f * dt;
                }

                if (keyboardState.IsKeyDown(Keys.S))
                {
                    MovementState = PlayerMovementState.Up;
                    moveVector.Y = 1.0f * dt;
                }

                if (keyboardState.IsKeyDown(Keys.A))
                {
                    MovementState = PlayerMovementState.Right;
                    moveVector.X = -1.0f * dt;
                }

                if (keyboardState.IsKeyDown(Keys.D))
                {
                    MovementState = PlayerMovementState.Left;
                    moveVector.X = 1.0f * dt;
                }

                if (moveVector != Vector2.Zero)
                {
                    moveVector = PositionConverter?.Position2DConverterFunc(moveVector) ?? moveVector;
                    Move(moveVector);
                    PositionChanged?.Position2DChangedAction?.Invoke(Position);

                    if (CollisionManager.Collides(Collision))
                    {
                        Move(-moveVector);
                        PositionChanged?.Position2DChangedAction?.Invoke(Position);
                    }

                    // -- Border Shit
                    if (Zoom < 1.0f)
                        while (!CameraBordersRectangle.Contains(BoundingRectangle))
                            ZoomIn();
                    // -- Border Shit
                }

                /*
                // -- Moving
                if (CollisionManager != null)
                {
                    MovementState = PlayerMovementState.None;
                    var moveVector = Vector2.Zero;

                    if (keyboardState.IsKeyDown(Keys.W))
                    {
                        MovementState = PlayerMovementState.Down;
                        moveVector.Y = -1.0f;
                    }

                    if (keyboardState.IsKeyDown(Keys.S))
                    {
                        MovementState = PlayerMovementState.Up;
                        moveVector.Y = 1.0f;
                    }

                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        MovementState = PlayerMovementState.Right;
                        moveVector.X = -1.0f;
                    }

                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        MovementState = PlayerMovementState.Left;
                        moveVector.X = 1.0f;
                    }

                    if (moveVector != Vector2.Zero)
                    {
                        moveVector = moveVector * 4 * (float)dt;
                        //moveVector.Normalize();
                        //moveVector *= dt * CameraSpeed;
                        //moveVector = new Vector2(
                        //    (float) Math.Round((double) moveVector.X, 3, MidpointRounding.AwayFromZero),
                        //    (float) Math.Round((double) moveVector.Y, 3, MidpointRounding.));

                        //Move(moveVector);
                        //MoveTo(Position + moveVector, Rotation);
                        Position += moveVector;

                        // -- Border Shit
                        if (Zoom < 1.0f)
                            while (!CameraBordersRectangle.Contains(BoundingRectangle))
                                ZoomIn();
                        // -- Border Shit

                        if (CollisionManager.Collides(Collision))
                            Move(-moveVector);
                        else
                            PositionChanged?.Position2DChangedAction?.Invoke(PositionConverter?.Position2DConverterFunc(Position) ?? Position);
                    }
                }
                */

            }
        }

        /// <summary>Shuts down the component.</summary>
        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    KeyboardListener.KeyPressed -= KeyboardListener_KeyPressed;
                }
            }

            base.Dispose(disposing);
        }
    }
}
