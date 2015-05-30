package gibbo;

import openfl.display.Bitmap;
import openfl.display.BitmapData;
import openfl.display.Sprite;
import openfl.geom.Matrix;
import openfl.Lib;
import flash.Lib;
import flash.geom.Point;
import flash.geom.Rectangle;
import haxe.xml.Fast;
import openfl.Assets;
/**
 * ...
 * @author Luis Santos AKA DJOKER
 */
class GScene  
{
	public var Layers:Array<Layer>;
	public function new(filename:String) 
	{
		Layers = new Array<Layer>();
		load(filename);
	}
	public function load(filename:String)
	{
		var xml:Xml = Xml.parse (Assets.getText(filename));
		var xmlNode = xml.firstElement();
		var fast = new Fast(xmlNode);
		
		var layers:List<Fast> =  fast.nodes.resolve("Layer");
		for (layer in layers)
		{
			
			var gameLayer:Layer = new Layer();
		
	
			for (GameObjects in layer.elements)
			{
			for (GameOBJ in GameObjects.elements)
			{
				
				if (GameOBJ.att.Type == "Sprite")
				{
					
					processSprites(gameLayer, GameOBJ); 
				} else
				if (GameOBJ.att.Type == "AnimatedSprite")
				{
					
					processAnimatedSprites(gameLayer, GameOBJ);
					
				} else
				if (GameOBJ.att.Type == "Tileset")
				{
				
					processTiles(gameLayer, GameOBJ);
				} 
				
				
		    }
			}
			
		
			Layers.push(gameLayer);
		}
		
		
	
		
	}
	
	public function getLayer(layer:Int):Layer
	{
		
		return Layers[layer];
	}
	
	private function processSprites(layer:Layer, node:Fast):Void
	{
		var gSprite:GameSprite = new GameSprite();
		var Name = node.att.Name;
		
		gSprite.Enable  = (node.att.Enable == "true") ? true :false;
		gSprite.Visible =(node.att.Visible == "true")?true: false;
	    gSprite.ParentName = node.att.Parent;
	
		 var Image:Fast = node.node.Image;
		 gSprite.Image = Image.att.value;
		 
	     //var srcImage:BitmapData = Assets.getBitmapData("assets/Images/" +image);
		
		  
		  
		 
	
		 
		// var Pivot:Fast = node.node.Pivot;
		// gSprite.Pivot.x = Std.parseFloat(Pivot.att.x);
		// gSprite.Pivot.y = Std.parseFloat(Pivot.att.y);
	
	
		{
		
		 var Transform:Fast = node.node.Transform;
		 {
		 //var Position:Fast = Transform.node.Relative;
		 var Position:Fast = Transform.node.Position;
		 //gSprite.Position.x =Std.int( (Std.parseFloat(Position.att.x) + 800 / 2));
		//gSprite.Position.y =Std.int( (Std.parseFloat(Position.att.y) + 600 / 2));
	//	 gSprite.Position.x =Std.int( (Std.parseFloat(Position.att.x) ));
//		 gSprite.Position.y =Std.int( (Std.parseFloat(Position.att.y) ));
		gSprite.Position.x =Std.int( (Std.parseFloat(Position.att.x) ));
		 gSprite.Position.y =Std.int( (Std.parseFloat(Position.att.y) ));
	
		 }
		 {
		 var Scale:Fast = Transform.node.Scale;
		 gSprite.Scale.x = Std.parseFloat(Scale.att.x);
		 gSprite.Scale.y = Std.parseFloat(Scale.att.y);
		 }
		 {
		 var Rotate:Fast = Transform.node.Rotate;
		 var rotation:Int =  Std.parseInt(Rotate.att.angleDeg) ;
		gSprite.Angle = rotation;
		 }
		} 
		 
		 
		
		 
		 var Color:Fast = node.node.Color;
		 var r:Int = Std.parseInt(Color.att.r);
		 var g:Int = Std.parseInt(Color.att.g);
		 var b:Int = Std.parseInt(Color.att.b);
		 var a:Int = Std.parseInt(Color.att.a);
		 
		
		
		 
		layer.addGameObject(gSprite);
		
	 
		
		
	
	}
	private function processAnimatedSprites(layer:Layer, node:Fast):Void
	{

/*
		var enable:Int=Std.parseInt(node.att.Enable);
		var visible:Int=Std.parseInt(node.att.Visible);
	    var parent:String = node.att.Parent;
	
		
		var gSprite:AnimatedSprite = new AnimatedSprite();
		gSprite.Name = node.att.Name;
		gSprite.Enable = (enable == 1)? true:false;
		gSprite.visible = (visible == 1)? true:false;
		
		
		
		
	
		layer.addGameObject(gSprite);
		
	 */
		
		
	
	}
	
	private function processTiles(layer:Layer, node:Fast):Void
	{
	
		 var Image:Fast = node.node.Image;
		 var image:String = Image.att.value;
	     var srcImage:BitmapData = Assets.getBitmapData("assets/Images/" +image);
	 
	
		
		
		var gTiles:Tileset = new Tileset(srcImage,node);
		gTiles.Name = node.att.Name;
		
		gTiles.Enable = (node.att.Enable == "true") ? true :false;
		gTiles.Visible =(node.att.Visible == "true")?true: false;
	    var parent:String = node.att.Parent;
	
		
	
	
	
		{
		
		 var Transform:Fast = node.node.Transform;
		 {
		 var Position:Fast = Transform.node.Position;
		 gTiles.Position.x = Std.parseFloat(Position.att.x);
		 gTiles.Position.y = Std.parseFloat(Position.att.y);
		 }
		 {
		 var Scale:Fast = Transform.node.Scale;
		 gTiles.Scale.x = Std.parseFloat(Scale.att.x);
		 gTiles.Scale.y = Std.parseFloat(Scale.att.y);
		 }
		 {
		 var Rotate:Fast = Transform.node.Rotate;
		 gTiles.Angle = Std.parseFloat(Rotate.att.angleDeg);
		 }
		} 
		 

	
		 
		 var Color:Fast = node.node.Color;
		 var r:Int = Std.parseInt(Color.att.r);
		 var g:Int = Std.parseInt(Color.att.g);
		 var b:Int = Std.parseInt(Color.att.b);
		 var a:Int = Std.parseInt(Color.att.a);
		 
		
		 
		 
		layer.addGameObject(gTiles);
	}
	
	
	
}