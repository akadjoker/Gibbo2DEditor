package;

import openfl.display.Sprite;
import openfl.Lib;


import com.haxepunk.Entity;
import com.haxepunk.HXP;
import com.haxepunk.Engine;
import com.haxepunk.graphics.Image;

/**
 * ...
 * @author Luis Santos AKA DJOKER
 */


class Main extends Engine
{

	override public function init()
	{
		
#if debug
		HXP.console.enable();
#end
		HXP.scene = new GameScene();
	}

	public static function main() { new Main(); }

}
