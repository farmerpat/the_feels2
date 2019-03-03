using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;

namespace TheFeels {
    // ...this should probably be a FeelsTileComponent and FeelsTileEntity ought to exist also
    public class FeelsTile : Nez.RenderableComponent, Nez.IUpdatable, Nez.ITriggerListener {
        protected Vector2 _pos;
        protected int _width = 16;
        protected int _height = 16;
        //protected Color _borderColor;
        protected Color _fillColor;
        //protected Rectangle _borderRect;
        protected Rectangle _fillRect;
        //protected int _borderWidth;
        protected Vector2 _entityOffset;
        protected float _percentFilled = 1.0f;
        protected GameManager.FeelsTileType _feelsType;
        //protected BoxCollider _hitBox = null;
        //protected GameManager.Tags _tag;

        // nay, this isn't an enitty...must override entity and tag that...
        /*
        public GameManager.Tags FeelsTag {
            get {
                return _tag;
            }
        }
        */

        public override float width { get { return _width; } }
        public override float height {  get { return _height; } }
        //private static Color _borderColor = Color.Black;
        // feels good color
        //private static Color _fillColor = new Color();
        //private static int _borderWidth = 2;

        public FeelsTile(Vector2 pos, GameManager.FeelsTileType ft) {
            _pos = pos;
            //_fillColor = fillColor;
            //_borderRect = new Rectangle((int)_pos.X, (int)_pos.Y, _width, _height);
            _fillRect = new Rectangle((int)_pos.X, (int)_pos.Y, _width, _height);
            _feelsType = ft;

            if (ft == GameManager.FeelsTileType.Good) {
                _fillColor = new Color(73, 112, 198);
            } else {
                _fillColor = new Color(185, 68, 68);
            }
        }

        public void increaseColor() {
            var r = _fillColor.R;
            var g = _fillColor.G;
            var b = _fillColor.B;

            // bluify!
            if (_feelsType == GameManager.FeelsTileType.Good) {
                if (r > 0) {
                    r -= 1;
                }

                if (b < 254) {
                    b += 1;
                }

                if (g > 0) {
                    g -= 1;
                }
            } else {
                if (r < 255) {
                    r += 1;
                }

                if (b > 0) {
                    b -= 1;
                }

                if (g > 0) {
                    g -= 1;
                }
            }

            /*
            if (r < 0) {
                r = 0;
            } else if (r > 255) {
                r = 255;
            }

            if (g < 0) {
                g = 0;
            } else if (g > 255) {
                g = 255;
            }

            if (b < 0) {
                b = 0;
            } else if (b > 255) {
                b = 255;
            }
            */

            //System.Diagnostics.Debug.WriteLine("new r: {0}", r);
            //System.Diagnostics.Debug.WriteLine("new g: {0}", g);
            //System.Diagnostics.Debug.WriteLine("new b: {0}", b);

            //_fillColor = new Color(r, g, b);
            _fillColor.R = r;
            _fillColor.G = g;
            _fillColor.B = b;
        }

        public override void onAddedToEntity() {
            // create its hitbox given its position
            float xOffset = entity.localPosition.X;
            float yOffset = entity.localPosition.Y;
            _entityOffset = new Vector2((int)xOffset, (int)yOffset);

            // this should probably be taking place in a related entity class...
            //var hitBox = new BoxCollider(-8, -16, 16, 16);
            //_hitBox = new BoxCollider(xOffset+0, yOffset+0, 16, 16);
        }

        void IUpdatable.update() { }

        void Nez.ITriggerListener.onTriggerEnter(Collider other, Collider self) {
            System.Diagnostics.Debug.WriteLine("triggerEnter: {0}: ", other.entity.name);
            //Nez.Debug.log("triggerEnter: {0}: ", other.entity.name);
        }

        void Nez.ITriggerListener.onTriggerExit(Collider other, Collider self) {
            System.Diagnostics.Debug.WriteLine("triggerEnter: {0}: ", other.entity.name);
           //System.Diagnostics.Debug.WriteLine("triggerExit: {0}: ", other.entity.name);
        }

        public override void render(Graphics graphics, Camera camera) {
            //float borderOffsetX = _borderRect.X + _entityOffset.X;
            //float borderOffsetY = _borderRect.Y + _entityOffset.Y;
            float fillOffsetX = _fillRect.X + _entityOffset.X;
            float fillOffsetY = _fillRect.Y + _entityOffset.Y;

            //graphics.batcher.drawRect(new Rectangle((int)borderOffsetX, (int)borderOffsetY, _borderRect.Width, _borderRect.Height) , _borderColor);
            graphics.batcher.drawRect(new Rectangle((int)fillOffsetX, (int)fillOffsetY, (int)(_fillRect.Width*_percentFilled), _fillRect.Height), _fillColor);
        }

    }
}
