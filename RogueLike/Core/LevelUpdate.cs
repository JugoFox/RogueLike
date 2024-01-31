
using RogueLike.GameObjects;
using System.Numerics;

namespace RogueLike.Core
{
    public class LevelUpdate
    {        
        ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
        Drawing draw = new Drawing();
        private bool isRunningGame = true;
        private bool isWinFloor = false;
        private int _width, _height;
        private List<Unit> _units;
        private Patsan _patsan;
        private List<PescerkaWay> _pescerkaMap;
        private List<Riches> _riches;
        private List<string> _logi;
        private int _score = 0;
        private List<Bullet> _bullets = new List<Bullet> { };

        enum WalkingDirection
        {
            Up = 1,
            Left = 2,
            Down = 3,
            Right = 4,
            None = 0
        }

        public LevelUpdate(int width, int height, List<PescerkaWay> pescerkaMap, List<Unit> units, Patsan patsan, List<Riches> riches, List<string> logi, int score, List<Bullet> bullets)
        {
            _width = width;
            _height = height;
            _pescerkaMap = pescerkaMap;
            _units = units;
            _patsan = patsan;
            _riches = riches;
            _logi = logi;
            _score = score;
            _bullets = bullets;
        }

        public void Update()
        {
            int countEbaka = 0;
            int countHoboWithShotgun = 0;
            foreach (var enamies in _units)
                if (enamies.Symbol == 'Т')
                    countEbaka++;
                else if (enamies.Symbol == 'Д')
                    countHoboWithShotgun++;

             PatsanController();

            if (countEbaka > 0)
                EbakaController();
            if (countHoboWithShotgun > 0)
                HoboWithShotgunController();
            if (_bullets.Count > 0)
                BulletController(_bullets, _units, _pescerkaMap);
            if (_riches.Count > 0)
                RichesController();

            foreach (var pescherkaMap in _pescerkaMap)
                if (pescherkaMap.TypeWay == 2 && pescherkaMap.Position.X == _patsan.Position.X && pescherkaMap.Position.Y == _patsan.Position.Y)
                {
                    _score += 572;
                    _logi.Add("Переход на следующий этаю, дополнительные очки 572");
                    if (countEbaka <= 0)
                    {
                        _score += 115;
                        _logi.Add("Все Т-позеры были наказаны, дополнотельные очки + 115");
                    }
                    if (countHoboWithShotgun <= 0)
                    {
                        _score += 207;
                        _logi.Add("Уничтожена вся популяция далеков, дополнотельные очки + 207");
                    }
                    if (countEbaka <= 0)
                    {
                        _score += 535;
                        _logi.Add("Подобраны все ништяки, дополнотельные очки + 535");
                    }
                    Console.Clear();
                    isWinFloor = true;
                }

            if (_patsan.Health <= 0)
            {
                Console.Clear();
                Console.WriteLine("Похоже это не твое. Ты помер");
                Console.WriteLine("Вы отошли в мир иной. Итоговый счет :" + _score);
                isRunningGame = false;
            }

            if (keyInfo.Key == ConsoleKey.Escape)
                isRunningGame = false;

            if (isRunningGame)
            {
                _units.RemoveAll(_enemies => _enemies.Health <= 0);
                _bullets.RemoveAll(_bullets => _bullets.Health <= 0);
                
                draw.PrintPescerka(_pescerkaMap, _width, _height);
                draw.PrintRiches(_riches, _width, _height);
                draw.PrintBullet(_bullets, _width, _height);
                draw.PrintUnit(_units, _width, _height);

                draw.DrawLog(_width, _patsan, _logi, _score);
            }
        }

        private void PatsanController()
        {
            if (keyInfo.Key == ConsoleKey.W && IsRoadAhead((int)_patsan.Position.X, (int)_patsan.Position.Y - 1) && !IsUnitAhead((int)_patsan.Position.X, (int)_patsan.Position.Y - 1))
                _patsan.Move(0, -1);
            else if (keyInfo.Key == ConsoleKey.A && IsRoadAhead((int)_patsan.Position.X - 1, (int)_patsan.Position.Y) && !IsUnitAhead((int)_patsan.Position.X - 1, (int)_patsan.Position.Y))
                _patsan.Move(-1, 0);
            else if (keyInfo.Key == ConsoleKey.S && IsRoadAhead((int)_patsan.Position.X, (int)_patsan.Position.Y + 1) && !IsUnitAhead((int)_patsan.Position.X, (int)_patsan.Position.Y + 1))
                _patsan.Move(0, 1);
            else if (keyInfo.Key == ConsoleKey.D && IsRoadAhead((int)_patsan.Position.X + 1, (int)_patsan.Position.Y) && !IsUnitAhead((int)_patsan.Position.X + 1, (int)_patsan.Position.Y))
                _patsan.Move(1, 0);

            else if (keyInfo.Key == ConsoleKey.UpArrow && IsRoadAhead((int)_patsan.Position.X, (int)_patsan.Position.Y - 1))
                PatsanAttack((int)_patsan.Position.X, (int)_patsan.Position.Y - 1);
            else if (keyInfo.Key == ConsoleKey.LeftArrow && IsRoadAhead((int)_patsan.Position.X - 1, (int)_patsan.Position.Y))
                PatsanAttack((int)_patsan.Position.X - 1, (int)_patsan.Position.Y);
            else if (keyInfo.Key == ConsoleKey.DownArrow && IsRoadAhead((int)_patsan.Position.X, (int)_patsan.Position.Y + 1))
                PatsanAttack((int)_patsan.Position.X, (int)_patsan.Position.Y + 1);
            else if (keyInfo.Key == ConsoleKey.RightArrow && IsRoadAhead((int)_patsan.Position.X + 1, (int)_patsan.Position.Y))
                PatsanAttack((int)_patsan.Position.X + 1, (int)_patsan.Position.Y);
        }

        private void PatsanAttack(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write('*');
            Thread.Sleep(100);
            Console.SetCursorPosition(x, y);
            Console.Write(' ');

            foreach (var enamies in _units)
            {
                if (enamies.Position.X == x && enamies.Position.Y == y)
                {
                    enamies.TakeDamage(_patsan.Damage);
                    if (enamies.Symbol == 'Т')
                    {
                        _logi.Add("Т-позер был беспощадно изнечтожен");
                        _score += 50;
                        _logi.Add("Начислено 50 очков");
                    }
                    else if (enamies.Symbol == 'Д')
                    {
                        _logi.Add("Далек был повержен");
                        _score += 75;
                        _logi.Add("Начислено 75 очков");
                    }
                }
            }
        }

        private void EbakaController()
        {
            foreach (var enamies in _units)
                if (enamies.Symbol == 'Т')
                {
                    if (enamies.Position.X - 1 == _patsan.Position.X && enamies.Position.Y == _patsan.Position.Y && enamies.Cooldown >= 3)
                        EbakaAttack(enamies);
                    else if (enamies.Position.X == _patsan.Position.X && enamies.Position.Y - 1 == _patsan.Position.Y && enamies.Cooldown >= 3)
                        EbakaAttack(enamies);
                    else if (enamies.Position.X + 1 == _patsan.Position.X && enamies.Position.Y == _patsan.Position.Y && enamies.Cooldown >= 3)
                        EbakaAttack(enamies);
                    else if (enamies.Position.X == _patsan.Position.X && enamies.Position.Y + 1 == _patsan.Position.Y && enamies.Cooldown >= 3)
                        EbakaAttack(enamies);
                    else
                        EnamiesWalking(enamies);

                    enamies.Cooldown += 1;
                }
        }

        private void EbakaAttack(Unit enamies)
        {
            Console.SetCursorPosition((int)_patsan.Position.X, (int)_patsan.Position.Y);
            Console.Write('*');
            Thread.Sleep(100);
            Console.SetCursorPosition((int)_patsan.Position.X, (int)_patsan.Position.Y);
            Console.Write(' ');
            _patsan.TakeDamage(enamies.Damage);
            enamies.Cooldown = 0;
            _logi.Add("Ты отхватил затрещину");
            _logi.Add("Здоровье пошатнулось на " + enamies.Damage);
        }

        private void HoboWithShotgunController()
        {
            foreach (var enamies in _units)
                if (enamies.Symbol == 'Д')
                {
                    if (DirectioOfShot(enamies, _pescerkaMap, 5) == (int)WalkingDirection.Up && enamies.Cooldown >= 6)
                        HoboWithShotgunAttack((int)WalkingDirection.Up, enamies);
                    else if (DirectioOfShot(enamies, _pescerkaMap, 5) == (int)WalkingDirection.Left && enamies.Cooldown >= 6)
                        HoboWithShotgunAttack((int)WalkingDirection.Left, enamies);
                    else if (DirectioOfShot(enamies, _pescerkaMap, 5) == (int)WalkingDirection.Down && enamies.Cooldown >= 6)
                        HoboWithShotgunAttack((int)WalkingDirection.Down, enamies);
                    else if (DirectioOfShot(enamies, _pescerkaMap, 5) == (int)WalkingDirection.Right && enamies.Cooldown >= 6)
                        HoboWithShotgunAttack((int)WalkingDirection.Right, enamies);
                    else
                        EnamiesWalking(enamies);

                    enamies.Cooldown += 1;
                }
        }

        private void HoboWithShotgunAttack(int direction, Unit enamies)
        {
            if (direction == 1 || direction == 3)
            {
                Bullet bullet = new Bullet('|', new Vector2(enamies.Position.X, enamies.Position.Y), direction, 10);
                _bullets.Add(bullet);
                enamies.Cooldown = 0;
            }
            else if (direction == 2 || direction == 4)
            {
                Bullet bullet = new Bullet('-', new Vector2(enamies.Position.X, enamies.Position.Y), direction, 10);
                _bullets.Add(bullet);
                enamies.Cooldown = 0;
            }
        }

        private void EnamiesWalking(Unit enamies)
        {
            Random rand = new Random();
            int walkingDirection = rand.Next(5);

            if (walkingDirection == (int)WalkingDirection.Up && IsRoadAhead((int)enamies.Position.X, (int)enamies.Position.Y - 1) && !IsUnitAhead((int)enamies.Position.X, (int)enamies.Position.Y - 1))
                enamies.Move(0, -1);
            else if (walkingDirection == (int)WalkingDirection.Left && IsRoadAhead((int)enamies.Position.X - 1, (int)enamies.Position.Y) && !IsUnitAhead((int)enamies.Position.X - 1, (int)enamies.Position.Y))
                enamies.Move(-1, 0);
            else if (walkingDirection == (int)WalkingDirection.Down && IsRoadAhead((int)enamies.Position.X, (int)enamies.Position.Y + 1) && !IsUnitAhead((int)enamies.Position.X, (int)enamies.Position.Y + 1))
                enamies.Move(0, 1);
            else if (walkingDirection == (int)WalkingDirection.Right && IsRoadAhead((int)enamies.Position.X + 1, (int)enamies.Position.Y) && !IsUnitAhead((int)enamies.Position.X + 1, (int)enamies.Position.Y))
                enamies.Move(1, 0);
        }

        private void RichesController()
        {
            foreach (var riches in _riches)
                if (riches.Position.X == _patsan.Position.X && riches.Position.Y == _patsan.Position.Y)
                {
                    _score += riches.GetScoreRiches(riches.TypeRiches);
                    _logi.Add(riches.GetDescriptionRiches(riches.TypeRiches));
                    _logi.Add("Начислено " + riches.GetScoreRiches(riches.TypeRiches)+ " очков");
                    riches.Found = 0;
                    break;
                }
            _riches.RemoveAll(_riches => _riches.Found == 0);
        }

        private void BulletController(List<Bullet> bullets, List<Unit> units, List<PescerkaWay> pescerkaWay)
        {
            foreach (var bullet in bullets)
                if (bullet.Health == (int)WalkingDirection.Up)
                {
                    bool isMoving = false;
                    foreach (var pescerka in pescerkaWay)
                    {
                        if (pescerka.Position.X == bullet.Position.X && pescerka.Position.Y == bullet.Position.Y - 1)
                        {
                            isMoving = true;                            
                            break;
                        }                                
                    }

                    if (isMoving)
                        bullet.Move(0, -1);
                    else
                        bullet.TakeDamage(bullet.Damage);

                    foreach (var unit in units)
                    {
                        if (unit.Position.X == bullet.Position.X && unit.Position.Y == bullet.Position.Y)
                        {
                            if (unit.Symbol == 'Ф')
                            {
                                _patsan.TakeDamage(bullet.Damage);
                                _logi.Add("Лазер прожег в тебе дырку");
                                _logi.Add("Здоровье пошатнулось на " + bullet.Damage);
                                bullet.TakeDamage(bullet.Damage);
                            }
                            if (unit.Symbol == 'Т' || unit.Symbol == 'Д')
                            {
                                unit.TakeDamage(bullet.Damage);
                                _logi.Add("Огонь по своим, так даже лучше");
                                bullet.TakeDamage(bullet.Damage);
                            }
                        }
                    }
                }
                else if (bullet.Health == (int)WalkingDirection.Left)
                {
                    bool isMoving = false;
                    foreach (var pescerka in pescerkaWay)
                    {
                        if (pescerka.Position.X == bullet.Position.X - 1 && pescerka.Position.Y == bullet.Position.Y)
                        {
                            isMoving = true;
                            break;
                        }
                    }

                    if (isMoving)
                        bullet.Move(-1, 0);
                    else
                        bullet.TakeDamage(bullet.Damage);

                    foreach (var unit in units)
                    {
                        if (unit.Position.X == bullet.Position.X && unit.Position.Y == bullet.Position.Y)
                        {
                            if (unit.Symbol == 'Ф')
                            {
                                _patsan.TakeDamage(bullet.Damage);
                                _logi.Add("Лазер прожег в тебе дырку");
                                _logi.Add("Здоровье пошатнулось на " + bullet.Damage);
                                bullet.TakeDamage(bullet.Damage);
                            }
                            if (unit.Symbol == 'Т' || unit.Symbol == 'Д')
                            {
                                unit.TakeDamage(bullet.Damage);
                                _logi.Add("Огонь по своим, так даже лучше");
                                bullet.TakeDamage(bullet.Damage);
                            }
                        }
                    }
                }
                else if (bullet.Health == (int)WalkingDirection.Down)
                {
                    bool isMoving = false;
                    foreach (var pescerka in pescerkaWay)
                    {
                        if (pescerka.Position.X == bullet.Position.X && pescerka.Position.Y == bullet.Position.Y + 1)
                        {
                            isMoving = true;
                            break;
                        }
                    }

                    if (isMoving)
                        bullet.Move(0, 1);
                    else
                        bullet.TakeDamage(bullet.Damage);

                    foreach (var unit in units)
                    {
                        if (unit.Position.X == bullet.Position.X && unit.Position.Y == bullet.Position.Y )
                        {
                            if (unit.Symbol == 'Ф')
                            {
                                _patsan.TakeDamage(bullet.Damage);
                                _logi.Add("Лазер прожег в тебе дырку");
                                _logi.Add("Здоровье пошатнулось на " + bullet.Damage);
                                bullet.TakeDamage(bullet.Damage);
                            }
                            if (unit.Symbol == 'Т' || unit.Symbol == 'Д')
                            {
                                unit.TakeDamage(bullet.Damage);
                                _logi.Add("Огонь по своим, так даже лучше");
                                bullet.TakeDamage(bullet.Damage);
                            }
                        }
                    }
                }
                else if (bullet.Health == (int)WalkingDirection.Right)
                {
                    bool isMoving = false;
                    foreach (var pescerka in pescerkaWay)
                    {
                        if (pescerka.Position.X == bullet.Position.X + 1 && pescerka.Position.Y == bullet.Position.Y)
                        {
                            isMoving = true;
                            break;
                        }
                    }

                    if (isMoving)
                        bullet.Move(1, 0);
                    else
                        bullet.TakeDamage(bullet.Damage);

                    foreach (var unit in units)
                    {
                        if (unit.Position.X == bullet.Position.X && unit.Position.Y == bullet.Position.Y)
                        {
                            if (unit.Symbol == 'Ф')
                            {
                                _patsan.TakeDamage(bullet.Damage);
                                _logi.Add("Лазер прожег в тебе дырку");
                                _logi.Add("Здоровье пошатнулось на " + bullet.Damage);
                                bullet.TakeDamage(bullet.Damage);
                            }
                            if (unit.Symbol == 'Т' || unit.Symbol == 'Д')
                            {
                                unit.TakeDamage(bullet.Damage);
                                _logi.Add("Огонь по своим, так даже лучше");
                                bullet.TakeDamage(bullet.Damage);
                            }
                        }
                    }
                }
        }

        private int DirectioOfShot(Unit enamies, List<PescerkaWay> pescerka, int fieldOfView)
        {
            int directioOfShot = 0;
            int distancePatsan = 0;
            int distanceWall = 0;

            int[] arrayDistanceWall = new int[6] { 0, 1, 1, 1, 1, 1 };

            for (int i = 1; i <= fieldOfView; i++)
            {
                if (enamies.Position.X == _patsan.Position.X && enamies.Position.Y - i == _patsan.Position.Y)
                {
                    directioOfShot = 1;
                    distancePatsan = i;
                }
                else if (enamies.Position.X - i == _patsan.Position.X && enamies.Position.Y == _patsan.Position.Y)
                {
                    directioOfShot = 2;
                    distancePatsan = i;
                }
                else if (enamies.Position.X == _patsan.Position.X && enamies.Position.Y + i == _patsan.Position.Y)
                {
                    directioOfShot = 3;
                    distancePatsan = i;
                }
                else if (enamies.Position.X + i == _patsan.Position.X && enamies.Position.Y == _patsan.Position.Y)
                {
                    directioOfShot = 4;
                    distancePatsan = i;
                }
            }

            for (int i = 1; i <= fieldOfView; i++)
            {
                if (directioOfShot == 1)
                    foreach (var way in pescerka)
                    {
                        if (enamies.Position.X == way.Position.X && enamies.Position.Y - i == way.Position.Y)
                            arrayDistanceWall[i] = 0;
                    }
                else if (directioOfShot == 2)
                    foreach (var way in pescerka)
                    {
                        if (enamies.Position.X - i == way.Position.X && enamies.Position.Y == way.Position.Y)
                            arrayDistanceWall[i] = 0;
                    }
                else if (directioOfShot == 3)
                    foreach (var way in pescerka)
                    {
                        if (enamies.Position.X == way.Position.X && enamies.Position.Y + i == way.Position.Y)
                            arrayDistanceWall[i] = 0;
                    }
                else if (directioOfShot == 4)
                    foreach (var way in pescerka)
                    {
                        if (enamies.Position.X + i == way.Position.X && enamies.Position.Y == way.Position.Y)
                            arrayDistanceWall[i] = 0;
                    }
            }

            for (int i = 0; i < arrayDistanceWall.Length; i++)
            {
                if (arrayDistanceWall[i] == 1)
                {
                    distanceWall = i;
                    break;
                }
            }

            if (distancePatsan < distanceWall)
                return directioOfShot;                
            else
                return 0;
        }

        public int GetScore()
        {
            return _score;
        }

        public bool GetIsRunningGame()
        {
            return isRunningGame;
        }

        public bool GetIsWinFloor()
        {
            return isWinFloor;
        }

        private bool IsRoadAhead(int x, int y)
        {
            bool isRoadAhead = false;
            foreach (var way in _pescerkaMap)
                if (way.Position.X == x && way.Position.Y == y)
                {
                    isRoadAhead = true;
                    break;
                }
            return isRoadAhead;
        }

        private bool IsUnitAhead(int x, int y)
        {
            bool isUnitAhead = false;
            foreach (var unit in _units)
                if (unit.Position.X == x && unit.Position.Y == y)
                {
                    isUnitAhead = true;
                    break;
                }
            return isUnitAhead;
        }
    }
}
