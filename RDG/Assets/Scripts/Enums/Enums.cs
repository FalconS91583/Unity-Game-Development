﻿public enum Orientation
{
    north,
    east,
    south,
    west,
    none
}
public enum GameState
{
    gameStarted,
    playingLevel,
    engagingEnemies,
    bossStage,
    engagingBoss,
    levelCompleted,
    gameWon,
    gameLost,
    dungeonOverviewMap, 
    restartGame
}

public enum AimDirection
{
    Up,
    UpRight,
    UpLeft,
    Right,
    Left,
    Down
}