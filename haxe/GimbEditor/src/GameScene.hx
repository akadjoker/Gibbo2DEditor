package;

import com.haxepunk.graphics.PreRotation;
import com.haxepunk.graphics.Text;
import com.haxepunk.graphics.Tilemap;
import com.haxepunk.HXP;
import com.haxepunk.Scene;
import com.haxepunk.graphics.Image;
import com.haxepunk.utils.Draw;
import com.haxepunk.utils.Input;
import com.haxepunk.utils.Key;
import gibbo.GameSprite;
import gibbo.GScene;
import gibbo.Layer;
import gibbo.Tileset;

/**
 * ...
 * @author Luis Santos AKA DJOKER
 */


class GameScene extends Scene
{
	
	public var scene:GScene;
    public var tilemap:Tilemap;
	public var label:Text;
 
    public function new()
    {
        super();
    }

    public override function begin()
    {
		//HXP.camera.x =- HXP.width / 2;
	//	HXP.camera.y =- HXP.height / 2;
		
		
      scene = new GScene("assets/scene.xml");
	  var layer:Layer = scene.getLayer(0);
	  
	  for (obj in layer.gameObjects)
	  {
		  if (Std.is(obj, GameSprite))
		  {
			  var sprite:GameSprite = cast(obj, GameSprite);
			  
			 
				  var graphic = new Image("assets/Images/" + sprite.Image);
				  graphic.x = sprite.Position.x + HXP.width/2;
				  graphic.y = sprite.Position.y + HXP.height/2;
				  
				
				  
				  graphic.centerOrigin();
				  if (sprite.Angle != 0)
				  {
				  graphic.centerOO();
				  graphic.angle = sprite.Angle;
				 
				  }
				  addGraphic(graphic, 1, 0, 0);
		  } else
		  if (Std.is(obj, Tileset))
		  {
			 var map:Tileset = cast(obj, Tileset);
			  
			
			 
			  tilemap = new Tilemap(map.tileimage, map.MapWidth*map.TileWidth, map.MapHeight*map.TileHeight, map.TileWidth, map.TileHeight);
			//  tilemap.loadFromString(map.csv);
			 
			 for (x in 0...map.MapWidth)
			 {
				 for (y in 0...map.MapHeight)
				 {
					 
					var id:Int =  map.getTile(x, y);
				
					if (id != 0)
					{
						tilemap.setTile(x, y, id - 1);
					}
				
				 }
			 }
			 
			 var MapSizeX = (map.MapWidth  * map.TileWidth);
			 var MapSizeY = (map.MapHeight * map.TileHeight);
			 
			 tilemap.x = ((HXP.width - MapSizeX) /2);
			 tilemap.y = ((HXP.height - MapSizeY) / 2);
			 
			 addGraphic(tilemap,1,0,0);
		  }
	  }
	  
	  label = new Text("position", 100, 100, 100, 20);
	  addGraphic(label);
	  
    }
    public override function update()
    {
	
       if (Input.check(Key.A))
	   {
		   tilemap.x -= 1;
	   }
      if (Input.check(Key.D))
	   {
		   tilemap.x += 1;
	   }
	    if (Input.check(Key.W))
	   {
		   tilemap.y -= 1;
	   }
      if (Input.check(Key.S))
	   {
		   tilemap.y += 1;
	   }
	   label.text = tilemap.x + "," + tilemap.y;
	   
	   
    }
	 override public function render():Void
	{
		super.render();
		/*
		HXP.sprite.graphics.clear();
		HXP.sprite.graphics.lineStyle(5, 0xFFFFFF);
		HXP.sprite.graphics.moveTo(0, 0);
		HXP.sprite.graphics.lineTo(500, 500);
		HXP.buffer.draw(HXP.sprite);

		HXP.sprite.graphics.clear();
		HXP.sprite.graphics.lineStyle(1, 0xFF0000);
		HXP.sprite.graphics.drawRect(0, 0, 100, 100);
		HXP.buffer.draw(HXP.sprite);
	*/
		Draw.line(Std.int(HXP.width / 2), 0, Std.int(HXP.width / 2), Std.int(HXP.height));
		Draw.line(0, Std.int(HXP.height/2), Std.int(HXP.width ), Std.int(HXP.height/2));
	   
      
	}
}
