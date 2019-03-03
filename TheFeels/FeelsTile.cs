using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;

namespace TheFeels {
    public class FeelsTile {
        protected Vector2 _pos;
        protected int _width = 16;
        protected int _height = 16;
        protected Color _fillColor;
        protected Rectangle _fillRect;
        protected GameManager.FeelsTileType _feelsType;

        public FeelsTile(Vector2 pos, GameManager.FeelsTileType ft) {
            _pos = pos;
            _fillRect = new Rectangle((int)_pos.X, (int)_pos.Y, _width, _height);
            _feelsType = ft;

            if (ft == GameManager.FeelsTileType.FeelsGood) {
                // these rgb values should probably come from GM instead of being magick #s
                _fillColor = new Color(73, 112, 198);
            } else if (ft == GameManager.FeelsTileType.FeelsBad) {
                _fillColor = new Color(185, 68, 68);
            } else if (ft == GameManager.FeelsTileType.FeelsNeutral) {
                _fillColor = new Color(100, 100, 100);
            } else {
                // FeelsNothing won't be drawn and won't have a hitbox
                _fillColor = new Color(0, 0, 0);
            }
        }

        public GameManager.FeelsTileType Type {
            get {
                return _feelsType;
            }
        }

        public int Width {
            get {
                return _width;
            }

            protected set {
                _width = value;
            }
        }

        public int Height {
            get {
                return _height;
            }

            protected set {
                _height = value;
            }
        }

        public Color FillColor {
            get {
                return _fillColor;
            }
        }

        public Rectangle Rect {
            get {
                return _fillRect;
            }
        }

        public Vector2 Pos {
            get {
                return _pos;
            }

            set {
                _pos = value;
            }
        }

        public void increaseColor() {
            var r = _fillColor.R;
            var g = _fillColor.G;
            var b = _fillColor.B;

            if (_feelsType == GameManager.FeelsTileType.FeelsGood) {
                if (r > 0) {
                    r -= 1;
                }

                if (b < 254) {
                    b += 1;
                }

                if (g > 0) {
                    g -= 1;
                }
            } else if (_feelsType == GameManager.FeelsTileType.FeelsBad) {
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

            _fillColor.R = r;
            _fillColor.G = g;
            _fillColor.B = b;
        }
    }
}
