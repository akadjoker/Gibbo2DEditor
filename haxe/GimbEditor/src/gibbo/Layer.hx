package gibbo;


/**
 * ...
 * @author Luis Santos AKA DJOKER
 */
class Layer  
{


	public var  gameObjects:Array<GameObject>;
	public function new() 
	{
	gameObjects = new Array<GameObject>();	
	}
	
	public function addGameObject(gs:GameObject):Int
	{
	 gameObjects.push(gs);
	 return gameObjects.length - 1;
	}
	
}