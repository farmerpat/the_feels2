using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
//using Nez.Scene;
using Nez.Tiled;

namespace TheFeels {
    public class Game1 : Nez.Core {
        public Game1() : base(1024, 768, false, true, "The Feels") { }

        protected override void Initialize() {
            base.Initialize();

            Scene myScene = Scene.createWithDefaultRenderer(Color.Gainsboro);

            //Scene.setDesignResolution(1024, 768, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            myScene.setDesignResolution(1024, 768, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);

            // next i tried "embedding" the tileset in the tilemap
            // while importing it as a tileset in Tiled...
            //TiledMap tiledMap = myScene.content.Load<TiledMap>("the_feels_tilemap_demo_take2");
            //TiledMap tiledMap = scene.content.Load<TiledMap>("tile_map");

            //var objectLayer = tiledMap.getObjectGroup("objects");
            //var spawnObject = objectLayer.objectWithName("Spawn");

            //var tiledEntity = myScene.createEntity("tiled-map-entity");
            // collision layer 'main'...not sure how hit boxes for tile map are being created as of yet...
            //var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap, "main"));

            var heroTexture = myScene.content.Load<Texture2D>("hero");
            var heroSprite = new Sprite(heroTexture);

            var spawnObject = new Vector2(100, 300);
            var heroEntity = myScene.createEntity("hero", spawnObject);
            //var heroEntity = scene.createEntity("hero", new Vector2(200,200));

            heroEntity.addComponent(heroSprite);

            // make texture and add it.
            //x,y,w,h (these are incorrect)
            // TODO: move this to Hero's definition. can get at the entity e.g. FeelsTileGroup onAddedToEntity
            var hitBox = new BoxCollider(-8, -8, 16, 32);
            //var hitBox = new BoxCollider(16, 32);
            heroEntity.addComponent(hitBox);
            heroEntity.addComponent(new Mover());
            heroEntity.addComponent(new Hero());
            //var tiledMapMover = new TiledMapMover(tiledMapComponent.collisionLayer);
            //heroEntity.addComponent(tiledMapMover);

            var healthBarsPos = new Vector2(20, 20);
            int healthBarWidth = 200;
            int healthBarHeight = 30;
            var healthBarEntity = myScene.createEntity("health-bar", healthBarsPos);

            var feeslGoodHealthBarComponent = new FeelsGoodHealthBar(
                new Vector2(0,0),
                healthBarWidth,
                healthBarHeight
            );

            var feelsBadHealthBarComponent = new FeelsBadHealthBar(
                new Vector2(0, 31),
                healthBarWidth,
                healthBarHeight
            );

            healthBarEntity.addComponent(feelsBadHealthBarComponent);
            healthBarEntity.addComponent(feeslGoodHealthBarComponent);

            var testTileGroupEnt = myScene.createEntity("test-tile-group", new Vector2(spawnObject.X, spawnObject.Y + 20));

            byte [,]map = {
                { 1, 1, 1, 1, 1, 1 },
                { 0, 0, 1, 1, 0, 0 },
                { 0, 0, 1, 1, 0, 0 },
                { 0, 0, 1, 1, 0, 0 },
                { 0, 0, 1, 1, 0, 0 },
                { 1, 1, 1, 1, 1, 1 },
            };

            FeelsTileGroup feelsTileGroupComponent = new FeelsTileGroup(map, GameManager.FeelsTileType.FeelsBad);
            testTileGroupEnt.tag = (int)GameManager.FeelsTileType.FeelsBad;
            testTileGroupEnt.addComponent(feelsTileGroupComponent);

            //myScene.addPostProcessor(new VignettePostProcessor(1));
            Core.debugRenderEnabled = true;
            Core.scene = myScene;
        }
    }
}
