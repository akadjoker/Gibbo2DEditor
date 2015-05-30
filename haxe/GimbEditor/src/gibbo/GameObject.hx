package gibbo;


import openfl.geom.Point;

/**
 * ...
 * @author Luis Santos AKA DJOKER
 */
class GameObject  
{
	public var Name:String;
	public var ParentName:String;
	public var Enable:Bool;
	public var Visible:Bool;
	public var Position:Point;
	public var Scale:Point;
	public var Pivot:Point;
	public var Angle:Float;
	public var ObjectType:Int;

	public function new() 
	{
	
		Pivot = new Point(0, 0);
		Position = new Point(0, 0);
		Enable = true;
		Visible = true;
		Scale = new Point(1, 1);
		Angle = 0;
		Name = "GameObject";
		ObjectType = 0;
	}
	
}