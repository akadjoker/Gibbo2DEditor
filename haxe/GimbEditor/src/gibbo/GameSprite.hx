package gibbo;
import openfl.geom.Point;
import openfl.geom.Rectangle;

/**
 * ...
 * @author Luis Santos AKA DJOKER
 */
class GameSprite extends GameObject
{
	public var SourceRect:Rectangle;
	public var Image:String;
	
	public function new() 
	{
		super();
		SourceRect = new Rectangle();
	
	}
	
}