using System;
//using Nez.AI.Pathfinding;
using Microsoft.Xna.Framework;
//using System.Collections.Generic;
//using Nez.Tiled;
using Nez;

namespace TheFeels {
    public class FeelsGoodHealthBar : HealthBar {
        private static Color _goodBorderColor = Color.Black;
        private static Color _goodFillColor = new Color(73, 112, 198);
        private static int _goodBorderWidth = 2;

        //public HealthBar (Vector2 pos, int width, int height, Color borderColor, Color fillColor, int borderWidth = 2) {
        public FeelsGoodHealthBar(Vector2 pos, int width, int height) :
            base(pos, width, height, _goodBorderColor, _goodFillColor, _goodBorderWidth, GameManager.FeelsType.Good) {

        }
    }

    public class FeelsBadHealthBar : HealthBar {
        private static Color _badBorderColor = Color.Black;
        private static Color _badFillColor = new Color(185, 68, 68);
        private static int _badBorderWidth = 2;

        public FeelsBadHealthBar(Vector2 pos, int width, int height) :
            base(pos, width, height, _badBorderColor, _badFillColor, _badBorderWidth, GameManager.FeelsType.Bad) {

        }
    }

    public class HealthBar : RenderableComponent { //, IUpdatable {
        protected Vector2 _pos;
        protected int _width;
        protected int _height;
        protected Color _borderColor;
        protected Color _fillColor;
        protected Rectangle _borderRect;
        protected Rectangle _fillRect;
        protected int _borderWidth;
        protected Vector2 _entityOffset;
        protected float _percentFilled = 1.0f;
        protected GameManager.FeelsType _feelsType;

        public override float width { get { return _width; } }
        public override float height {  get { return _height; } }

        public Vector2 Position {
            get {
                return _pos;
            }

            protected set {
                _pos = value;
            }
        }

        public float PercentFilled {
            get {
                return _percentFilled;
            }

            set {
                _percentFilled = value;
            }
        }

        public HealthBar (Vector2 pos, int width, int height, Color borderColor, Color fillColor, int borderWidth, GameManager.FeelsType ft) {
            _pos = pos;
            _width = width;
            _height = height;
            _borderColor = borderColor;
            _fillColor = fillColor;
            _borderWidth = borderWidth;
            _borderRect = new Rectangle((int)_pos.X, (int)_pos.Y, _width, _height);
            _fillRect = new Rectangle((int)_pos.X+_borderWidth, (int)_pos.Y+_borderWidth, _width - _borderWidth*2, _height - _borderWidth*2);
            _feelsType = ft;

        }

        public override void onAddedToEntity() {
            //base.onAddedToEntity();
            float xOffset = entity.localPosition.X;
            float yOffset = entity.localPosition.Y;
            _entityOffset = new Vector2((int)xOffset, (int)yOffset);
        }

        // TODO: fill the rest with the background color (e.g. add bg color as a param)
        public override void render(Graphics graphics, Camera camera) {
            float borderOffsetX = _borderRect.X + _entityOffset.X;
            float borderOffsetY = _borderRect.Y + _entityOffset.Y;
            float fillOffsetX = _fillRect.X + _entityOffset.X;
            float fillOffsetY = _fillRect.Y + _entityOffset.Y;

            graphics.batcher.drawRect(new Rectangle((int)borderOffsetX, (int)borderOffsetY, _borderRect.Width, _borderRect.Height) , _borderColor);
            graphics.batcher.drawRect(new Rectangle((int)fillOffsetX, (int)fillOffsetY, (int)(_fillRect.Width*_percentFilled), _fillRect.Height), _fillColor);
        }

        //public void update() { }
    }
}
