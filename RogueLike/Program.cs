using RogueLike.Core;

int width = 40;
int height = 20;
int minAmountEbaka = 2;
int minAmountHoboWithShotgun = 2;
int minAmountRiches = 2;
int maxAmountEbaka = 5;
int maxAmountHoboWithShotgun = 5;
int maxAmountRiches = 5;

LevelGenerator level = new LevelGenerator(width, height, minAmountEbaka, minAmountHoboWithShotgun, minAmountRiches, maxAmountEbaka, maxAmountHoboWithShotgun, maxAmountRiches);
level.StartScreen();
level.RunGame();





