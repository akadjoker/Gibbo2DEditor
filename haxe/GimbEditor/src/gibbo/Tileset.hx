package gibbo;
import haxe.xml.Fast;
import flash.events.Event;
import flash.events.MouseEvent;

import openfl.display.BitmapData;
import openfl.display.Sprite;
import openfl.display.Tilesheet;
import openfl.geom.Rectangle;

/**
 * ...
 * @author Luis Santos AKA DJOKER
 */


 
class Tileset extends GameObject
{

	public var tilesheet:Tilesheet;
	public var TileWidth:Int;
	public var TileHeight:Int;
	public var MapWidth:Int;
	public var MapHeight:Int;
	public var horizontal:Int;
	public var vertical:Int;
	private var tileMap:Array<Int>;
	public var csv:String;
	public var tileimage:BitmapData;

	
	public function new(image:BitmapData,node:Fast) 
	{
	 super();
	 tileimage = image;
	 var map:Fast = node.node.Map;
	 TileWidth = Std.parseInt( map.att.TileWidth);
	 TileHeight = Std.parseInt( map.att.TileHeight);
	 
	 MapWidth = Std.parseInt( map.att.MapWidth);
	 MapHeight = Std.parseInt( map.att.MapHeight);
	
	 
	 horizontal = Std.int(image.width / TileWidth);
	 vertical = Std.int(image.height / TileHeight);
	
	 
	 trace(MapWidth);
	 trace(MapHeight);
	 trace(TileWidth);
	 trace(TileHeight);
	 trace(horizontal);
	 trace(vertical);
	 
     tileMap = [];
	

	 var tiles:Fast = node.node.TileData;
	 csv = tiles.innerData;
	 csvToArray(csv);
	
	}
	 
	public function getTile(x:Int, y:Int) :Int
	{ 
	  return tileMap[x * MapHeight + y];
	}
	
	


	private  function csvToArray(input:String):Void
	{
		var result:Array<Int> = new Array<Int>();
		var rows:Array<String> = StringTools.trim(input).split("\n");
		var row:String;

		for (row in rows) 
		{

			if (row == "") 
			{
				continue;
			}

			var resultRow:Array<Int> = new Array<Int>();
			var entries:Array<String> = row.split(",");
			var entry:String;

			for (entry in entries) 
			{

				if (entry != "") 
				{
		 		 var t:Int = Std.parseInt(entry);
				 tileMap.push(t);
				}
			}
		}
		
	}

	
	
  

	
}