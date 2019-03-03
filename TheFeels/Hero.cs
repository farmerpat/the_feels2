using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Nez.Sprites;
using Nez.Textures;
//using Nez.ITriggerListener;
using Nez.Tiled;
/// shoudln't i be able to remove the Nez.xs  now?
using Nez;

namespace TheFeels {
    //class Hero : Nez.Component, Nez.ITriggerListener, Nez.IUpdateable {
    class Hero : Nez.Component, Nez.IUpdatable, Nez.ITriggerListener {
        protected float _moveSpeed = 150;
        //protected float _gravity = 1000;
        protected float _gravity = 2;
        protected float _jumpHeight = 16 * 5;
        protected float _feelsGoodHealth = 50.0f;
        protected float _feelsGoodMaxHealth = 100.0f;
        protected float _feelsBadHealth = 50.0f;
        protected float _feelsBadMaxHealth = 100.0f;

        //protected TiledMapMover _mover;
        protected Mover _mover;
        protected Nez.BoxCollider _hitBox;
        protected TiledMapMover.CollisionState _collisionState = new TiledMapMover.CollisionState();
        protected Vector2 _velocity;

        protected VirtualButton _jumpButton;
        protected VirtualAxis _xAxis;
        //public bool Enabled => true;

        //int IUpdateable.UpdateOrder => 0;

        //public event EventHandler<EventArgs> EnabledChanged;
        //public event EventHandler<EventArgs> UpdateOrderChanged;

        //Enabled = true;
        //public bool Enabled() { return true; }
        //public int UpdateOrder() { return 1; }

        public override void onAddedToEntity() {
            //var heroTexture = entity.scene.content.Load<Texture2D>("hero");
            //var heroSprite = new Sprite(heroTexture);
            //_hitBox = new BoxCollider(-8, -16, 16, 32);
            _hitBox = entity.getComponent<BoxCollider>();
            //_mover = entity.getComponent<TiledMapMover>();
            _mover = entity.getComponent<Mover>();
            //_mover = entity.addComponent(new Mover());

            setUpInput();
        }

        protected void setUpInput () {
            _jumpButton = new VirtualButton();
            _jumpButton.nodes.Add(new Nez.VirtualButton.GamePadButton(0, Buttons.A));

            _xAxis = new VirtualAxis();
            _xAxis.nodes.Add(new Nez.VirtualAxis.GamePadDpadLeftRight());
            _xAxis.nodes.Add(new Nez.VirtualAxis.GamePadLeftStickX());
        }

        public override void onRemovedFromEntity() {
            _jumpButton.deregister();
            _xAxis.deregister();
            //base.onRemovedFromEntity();
        }

        void IUpdatable.update() {
            var moveDir = new Vector2(_xAxis.value, 0);

            if (moveDir.X < 0) {
                //if (_collisionState.below) { } // then apply friction...
                _velocity.X = -_moveSpeed;
            } else if (moveDir.X > 0) {
                _velocity.X = _moveSpeed;
            } else {
                _velocity.X = 0;
            }

            if (_collisionState.below && _jumpButton.isPressed) {
                _velocity.Y = (float)-Math.Sqrt(2.0f * _jumpHeight * _gravity);
            }

            _velocity.Y += _gravity * Nez.Time.deltaTime;

            //_mover.move(_velocity * Nez.Time.deltaTime, _hitBox, _collisionState);
            CollisionResult currentCollision;
            _mover.move(_velocity, out currentCollision);

            if (currentCollision.collider != null) {
                //System.Diagnostics.Debug.WriteLine("fyf");
                if (currentCollision.collider.entity.tag == (int)GameManager.Tags.FeelsGoodTile) {
                    // (corposethumb)
                    //System.Diagnostics.Debug.WriteLine("do stuff w/ feelsgoodtile");
                    foreach (var tile in currentCollision.collider.entity.getComponents<FeelsTile>()) {
                        tile.increaseColor();
                        //System.Diagnostics.Debug.WriteLine("do stuff w/ feelsgoodtile");

                    }

                } else if (currentCollision.collider.entity.tag == (int)GameManager.Tags.FeelsBadTile) {
                    System.Diagnostics.Debug.WriteLine("do stuff w/ feelsbadtile");

                }

                if (currentCollision.normal.Y == -1) {
                    _velocity.Y = 0;
                }
            }

            if (_collisionState.below) {
                _velocity.Y = 0;
            }

            // update the health bars
            //via   _feelsGoodHealth and _feelsBadHealth... how to get at health bar components?
            // we know their names....
            var healthBarEntity = entity.scene.findEntity("health-bar");
            //var feelsGoodHealthBar = healthBarEntity.getComponent<HealthBar>();
            var feelsGoodHealthBars = healthBarEntity.getComponents<HealthBar>();

            foreach (var hb in feelsGoodHealthBars) {
                // if its feels good (should id by some sort of tag, or just make a subclass and grab that...

                // or a member that's an enum or something...
                //if (hb.FeelsType == FeelsType.FeelsGood) { }
                // then feels good
                if (hb.Position.Y == 0) {
                    hb.PercentFilled = (_feelsGoodHealth / _feelsGoodMaxHealth);
                } else {
                    hb.PercentFilled = (_feelsBadHealth / _feelsBadMaxHealth);
                }
            }
        }

        // TODO:
        // make red/blue bars and make them increase/deacrease depending on tile being stood on.

        // this code is never hit, and neither is the code in the platformer example from nez-samples...
        // i suspect the interaction between Hero and the Tiled entity is different...
        // how to get at the tiles we hit...
        // maybe i can add tile objects in the tileset object layer
        // maybe the move is to generate my own tiles...
        // TiledMapComponent doesn't implement ITriggerListener, so how to get at the tile we collided with?
        void Nez.ITriggerListener.onTriggerEnter (Collider other, Collider self) {
            //System.Diagnostics.Debug.WriteLine("triggerEnter: {0}: ", other.entity.name);
            System.Diagnostics.Debug.WriteLine("triggerEnter: {0}: ", other.entity.name);
            //Nez.Debug.log("triggerEnter: {0}: ", other.entity.name);
        }

        void Nez.ITriggerListener.onTriggerExit(Collider other, Collider self) {
           System.Diagnostics.Debug.WriteLine("triggerExit: {0}: ", other.entity.name);
        }

    }
}
