import { Obstacle } from '/js/wasteful-game/obstacle.js';
import { Info } from '/js/wasteful-game/info.js';
import { Direction } from '/js/wasteful-game/direction.js';
import { Zombie } from '/js/wasteful-game/zombie.js';
import { Player } from '/js/wasteful-game/player.js';
import { Taco } from '/js/wasteful-game/taco.js';
import { Grid } from '/js/wasteful-game/grid.js';

const wastefulBrown = '#dfd29e';
const wastefulGray = '#cccccc';
const hangryRed = '#ff0000';
const size = 42;
const wastefulInfoWidth = 126;


export class Wasteful {
  constructor(canvas) {
    this._canvas = canvas;
    this._context = canvas.getContext('2d');
    this._isRunning = false;
  }

  startGame() {

    var config = {
      type: Phaser.AUTO,
      width: this._canvas.width,
      height: this._canvas.height,
      scene: {
        preload: this._preload,
        create: this._create,
        update: this._update,
        render: this._render
      }
    };

    var game = new Phaser.Game(config);

    this._isRunning = true;
    window.requestAnimationFrame(() => this._updateFrame());
  }

  _preload() {
    this.load.setBaseURL('/images/ZedChatter/');

    this.load.image('taco', 'Taco-0.png');

    this.load.image('barrel', 'Barrel-0.png');
    this.load.image('barrel-fire', 'BarrelFires-0.png');
    this.load.image('tires', 'Tires-0.png');
    this.load.image('tires-fire', 'TiresFire-1.png');

    this.load.image('player', 'Hat-YellowShirt-Player-Idle-0.png');
    this.load.image('zombie', 'Zombie-0.png');

    this.load.image('rocky-ground-0', 'RockyGroundTile-0.png');
    this.load.image('rocky-ground-1', 'RockyGroundTile-1.png');
    this.load.image('rocky-ground-2', 'RockyGroundTile-2.png');
  }

  _create() {
    this.add.image(42, 42, 'sky');

    this._grid = new Grid(this._canvas, this._context);
    this._info = new Info(this._canvas, this._context);
    this._player = new Player(this._grid);
    this._zombie = new Zombie(this._grid);
    this._items = [];
    this._items.push(new Taco(this._grid));
    this._createObstacles();
  }

  movePlayer(direction) {
    this._player.move(new Direction(direction));
    this._zombie.moveToward(this._player);

    if (this._player.health <= 0) {
      this._isRunning = false;
    }
  }

  _updateFrame() {
    if (this._isRunning) {
      this._clearCanvas();
      this._drawBackground();
      this._grid.draw();
      this._info.draw(this._player);
      window.requestAnimationFrame(() => this._updateFrame());
    } else {
      this._drawGameOver();
    }
  }

  _clearCanvas() {
    this._context.clearRect(0, 0, this._canvas.width, this._canvas.height);
  }

  _drawGameOver() {
    this._context.fillStyle = hangryRed;
    this._context.font = "128px Arial";
    this._context.fillText('Game Over', 20, this._canvas.height - 10);
  }

  _drawBackground() {
    this._context.fillStyle = wastefulBrown;
    this._context.fillRect(0, 0, this._canvas.width - wastefulInfoWidth, this._canvas.height);

    this._context.fillStyle = wastefulGray;
    this._context.fillRect(this._canvas.width - wastefulInfoWidth, 0, wastefulInfoWidth, this._canvas.height);
  }

  _createObstacles() {
    for (let i = 0; i < 10; i++) {
      this._items.push(new Obstacle(this._grid));
    }
  }
}
