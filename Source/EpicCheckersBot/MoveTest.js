MoveUnit = function(toRow, toCol)
{
	var boardGame = g_sceneManager.scene.boardGame;
	var gameData = boardGame.gameData;
	
	var tileSize = boardGame.tileSprite.size
	
	var d = toRow * tileSize.width / 2 - toCol * tileSize.width / 2;
	var f = toRow * tileSize.height / 2 + toCol * tileSize.height / 2;
	
	var unitArray = boardGame.unit2;
	var unitIndex = 0;
	
	unitArray[unitIndex].setTargetPosition(boardGame.position.x - d, boardGame.position.y / 2 + f);
	
};