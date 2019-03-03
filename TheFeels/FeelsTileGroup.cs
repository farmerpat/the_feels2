using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;

namespace TheFeels {
    // can be represented by a matrix of zeros and ones, and will
    // have a FeelsType. fns to calucate total width /height etc.
    // if i end up creating sublcass of particular shapes (boxes Ts, etc),
    // super matrices representing levels could be a possibility.
    // descdent with squares of 1x1 2x2 etc, rects Ts, rotated Hs, etc

    // what should this inherit from? an entity, a RenderableComponent ?
    // the way i've been thinking about it is as a renderable drawing its own
    // tiles, better might be as an entity whose constructor adds tiles to itself...
    // not sure yet
    // but if this extends Renderable component and draws all the tiles, where do the hitboxes live?
    // i feel like we want this to be an entity...

    // it actually might make sense to make this be a component. FeelsTile could be changed
    // so that its just a relative position and a color (and not a Component). when we render FeelsTileGroup,
    // we can drawn squares of the appropriate color by asking each FeelsTile for its
    // position and current color. when we want to update the color, we set each one.
    // it would be nice if i could draw one "hitbox" around the whole shape, but
    // i suspect that each tile will have its own.

    // a FeelsRoom class could be a "map" of descendants of this class...
    // an a level could be an arrangement of rooms...
    public class FeelsTileGroup :  Nez.RenderableComponent, Nez.IUpdatable, Nez.ITriggerListener {
        protected FeelsTile[,] _tiles;
        protected int _rows;
        protected int _cols;
        protected int _tileWidth = 16;
        protected int _tileHeight = 16;
        protected Vector2 _entityOffset;

        public FeelsTileGroup(int rows, int cols) {
            Rows = rows;
            Cols = cols;
            _tiles = new FeelsTile[Rows, Cols];
        }

        public FeelsTileGroup(byte[,] map, GameManager.FeelsTileType ft) {
            // TODO: make sure its not jagged
            if (map.Rank != 2) {
                throw new Exception("FeelsTileGroup::FeelsTileGroup map must have rank 2 and be un-jagged");
            }

            var rows = map.GetLength(0);
            var cols = map.GetLength(1);

            _tiles = new FeelsTile[rows, cols];

            for (int i=0; i<rows; i++) {
                for (int j=0; j<cols; j++) {
                    if (map[i,j] == 0) {
                        _tiles[i, j] = new FeelsTile(new Vector2(-1, -1), GameManager.FeelsTileType.FeelsNothing);

                    } else {
                        _tiles[i, j] = new FeelsTile(new Vector2(_tileWidth * j, _tileHeight * i), ft);

                    }
                }
            }
        }

        public void SetTileAt(int r, int c, FeelsTile tile) {
            if (! (r < Rows) && (c < Cols)) {
                throw new Exception(String.Format("FeelsTileGroup::SetTileAt {0}, {1} out of bounds", r, c));
            }

            _tiles[r,c] = tile;
        }

        public override float width { get { return TotalWidth; } }
        public override float height { get { return TotalHeight; } }

        public int TotalWidth {
            get {
                return TileWidth * Cols;
            }
        }

        public int TotalHeight {
            get {
                return TileHeight * Rows;
            }
        }

        public int TileWidth {
            get {
                return _tileWidth;
            }
        }

        public int TileHeight {
            get {
                return _tileHeight;
            }
        }

        public int Rows {
            get {
                return _rows;
            }

            protected set {
                _rows = value;
            }
        }

        public int Cols {
            get {
                return _cols;
            }

            protected set {
                _cols = value;
            }
        }

        public FeelsTile[,] Tiles {
            get {
                return _tiles;
            }

            protected set {
                _tiles = value;
            }
        }

        public override void onAddedToEntity() {
            float xOffset = entity.localPosition.X;
            float yOffset = entity.localPosition.Y;
            _entityOffset = new Vector2((int)xOffset, (int)yOffset);

            var rows = _tiles.GetLength(0);
            var cols = _tiles.GetLength(1);

            for (int i = 0; i < rows; i++) {
                for (int j=0; j<cols; j++) {
                    if (Tiles[i,j].Type == GameManager.FeelsTileType.FeelsNothing) {
                        continue;
                    }

                    FeelsTile someTile = _tiles[i, j];
                    Rectangle someRect = someTile.Rect;
                    entity.addComponent<BoxCollider>(new BoxCollider(someRect));
                }
            }
        }

        void IUpdatable.update() { }
        void Nez.ITriggerListener.onTriggerEnter(Collider other, Collider self) { }
        void Nez.ITriggerListener.onTriggerExit(Collider other, Collider self) { }

        public override void render(Graphics graphics, Camera camera) {
            var rows = _tiles.GetLength(0);
            var cols = _tiles.GetLength(1);

            for (int i=0; i<rows; i++) {
                for (int j=0; j<cols; j++) {
                    if (Tiles[i,j].Type == GameManager.FeelsTileType.FeelsNothing) {
                        continue;
                    }

                    FeelsTile someTile = _tiles[i, j];
                    Rectangle someRect = someTile.Rect;
                    float fillOffsetX = someRect.X + _entityOffset.X;
                    float fillOffsetY = someRect.Y + _entityOffset.Y;

                    graphics.batcher.drawRect(new Rectangle((int)fillOffsetX, (int)fillOffsetY, (int)someRect.Width, someRect.Height), someTile.FillColor);
                }
            }
        }
    }
}
