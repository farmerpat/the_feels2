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
    public class FeelsTileGroup {
        protected GameManager.FeelsTileType[,] _tiles;
        protected int _rows;
        protected int _cols;

        public FeelsTileGroup(int rows, int cols) {
            Rows = rows;
            Cols = cols;
            _tiles = new GameManager.FeelsTileType[Rows, Cols];
        }

        public void SetTileAt(int r, int c, GameManager.FeelsTileType type) {
            if (! (r < Rows) && (c < Cols)) {
                throw new Exception(String.Format("FeelsTileGroup::SetTileAt {0}, {1} out of bounds", r, c));
            }

            _tiles[r,c] = type;
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

        public GameManager.FeelsTileType[,] Tiles {
            get {
                return _tiles;
            }

            protected set {
                _tiles = value;
            }
        }
    }
}
