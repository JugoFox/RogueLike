
using RogueLike.GameObjects;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using static System.Formats.Asn1.AsnWriter;

namespace RogueLike.Core
{
    public class LevelUpdate
    {
        LevelGenerator level = new LevelGenerator();
        ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
        Drawing draw = new Drawing();
        private List<Unit> _enemies = new List<Unit> { };
        private Patsan _patsan;
        private char[,] _unitMap;
        private int[,] _pescerkaMap;
        private char[,] _richesMap;
        bool isRunningGame = true;
        private List<Bullet> _bullets = new List<Bullet> { };
        private List<string> _logi = new List<string> { };
        List<Riches> _riches = new List<Riches> { };
        private List<int> _score = new List<int> { };

        enum WalkingDirection
        {
            Up = 1,
            Left = 2,
            Down = 3,
            Right = 4,
            None = 0
        }

        public LevelUpdate(Patsan patsan, List<Unit> enemies, char[,] unitMap, int[,] pescerkaMap, List<string> logi, List<Bullet> bullets, char[,] richesMap, List<Riches> riches, List<int> score)
        {
            _patsan = patsan;
            _enemies = enemies;
            _unitMap = unitMap;
            _pescerkaMap = pescerkaMap;
            _logi = logi;
            _bullets = bullets;
            _richesMap = richesMap;
            _riches = riches;
            _score = score;
        }

        public void Update()
        {
            int score = 0;
            foreach (int s in _score)
                score += s;

            int countEbaka = 0;
            int countHoboWithShotgun = 0;
            foreach (var enamies in _enemies)
                if (enamies.Symbol == 'Т')
                    countEbaka++;
                else if (enamies.Symbol == 'Д')
                    countHoboWithShotgun++;


            PatsanController();
            if (countEbaka > 0)
                EbakaController();
            if (countHoboWithShotgun > 0)
                HoboWithShotgunController();
            if (_riches.Count > 0)
                RichesController();

            if (_pescerkaMap[_patsan.X, _patsan.Y] == 2)
            {
                _score.Add(572);
                Console.Clear();
                Console.WriteLine("Прощай душное подземелье");
                if (countEbaka <= 0)
                {
                    _score.Add(115);
                    Console.WriteLine("Все Т-позеры были наказаны, дополнотельные очки + 115");
                }
                if (countHoboWithShotgun <= 0)
                {
                    _score.Add(207);
                    Console.WriteLine("Уничтожена вся популяция далеков, дополнотельные очки + 207");
                }
                if (countEbaka <= 0)
                {
                    _score.Add(535);
                    Console.WriteLine("Подобраны все ништяки, дополнотельные очки + 535");
                }
                Console.WriteLine("Вы выбрались из подземелья. Итоговый счет :" + score);
                isRunningGame = false;
            }

            if (_patsan.Health <= 0)
            {
                Console.Clear();
                Console.WriteLine("Похоже это не твое. Ты помер");
                Console.WriteLine("Вы отошли в мир иной. Итоговый счет :" + score);
                isRunningGame = false;
            }

            if (keyInfo.Key == ConsoleKey.Escape)
                isRunningGame = false;

            if (isRunningGame)
            {
                foreach (var enamies in _enemies)
                    if (enamies.Health <= 0)
                        _unitMap[enamies.X, enamies.Y] = ' ';

                _enemies.RemoveAll(_enemies => _enemies.Health <= 0);
                _riches.RemoveAll(_riches => _riches.Found == 0);

                foreach (var riches in _riches)
                    _richesMap[riches.X, riches.Y] = '.';

                _unitMap[_patsan.X, _patsan.Y] = _patsan.Symbol;

                foreach (var enamies in _enemies)
                    _unitMap[enamies.X, enamies.Y] = enamies.Symbol;

                if (_bullets.Count > 0)
                    BulletMovement(_bullets, _enemies);

                draw.PrintRiches(_richesMap, level.Width, level.Height);
                draw.PrintUnit(_unitMap, level.Width, level.Height);
                draw.DrawLog(level.Width, level.Height, _patsan, _logi, _score);
            }
        }

        public bool GetIsRunningGame()
        {
            return isRunningGame;
        }

        private void PatsanController()
        {
            if (keyInfo.Key == ConsoleKey.W && _pescerkaMap[_patsan.X, _patsan.Y - 1] != 1 && _unitMap[_patsan.X, _patsan.Y - 1] == ' ')
            {
                _unitMap[_patsan.X, _patsan.Y] = ' ';
                Console.SetCursorPosition(_patsan.X, _patsan.Y);
                Console.Write(' ');
                _patsan.Move(0, -1);
            }
            else if (keyInfo.Key == ConsoleKey.A && _pescerkaMap[_patsan.X - 1, _patsan.Y] != 1 && _unitMap[_patsan.X - 1, _patsan.Y] == ' ')
            {
                _unitMap[_patsan.X, _patsan.Y] = ' ';
                Console.SetCursorPosition(_patsan.X, _patsan.Y);
                Console.Write(' ');
                _patsan.Move(-1, 0);
            }
            else if (keyInfo.Key == ConsoleKey.S && _pescerkaMap[_patsan.X, _patsan.Y + 1] != 1 && _unitMap[_patsan.X, _patsan.Y + 1] == ' ')
            {
                _unitMap[_patsan.X, _patsan.Y] = ' ';
                Console.SetCursorPosition(_patsan.X, _patsan.Y);
                Console.Write(' ');
                _patsan.Move(0, 1);
            }
            else if (keyInfo.Key == ConsoleKey.D && _pescerkaMap[_patsan.X + 1, _patsan.Y] != 1 && _unitMap[_patsan.X + 1, _patsan.Y] == ' ')
            {
                _unitMap[_patsan.X, _patsan.Y] = ' ';
                Console.SetCursorPosition(_patsan.X, _patsan.Y);
                Console.Write(' ');
                _patsan.Move(1, 0);
            }



            else if (keyInfo.Key == ConsoleKey.UpArrow && _pescerkaMap[_patsan.X, _patsan.Y - 1] != 1)
            {
                Console.SetCursorPosition(_patsan.X, _patsan.Y - 1);
                Console.Write('*');
                Thread.Sleep(100);
                Console.SetCursorPosition(_patsan.X, _patsan.Y - 1);
                Console.Write(' ');

                foreach (var enamies in _enemies)
                {
                    if (enamies.X == _patsan.X && enamies.Y == _patsan.Y - 1)
                    {
                        enamies.TakeDamage(_patsan.Damage);
                        if (enamies.Symbol == 'Т')
                        {
                            _logi.Add("Т-позер был беспощадно изнечтожен");
                            _score.Add(50);
                            _logi.Add("Начислено 50 очков");
                        }
                        else if (enamies.Symbol == 'Д')
                        {
                            _logi.Add("Далек был повержен");
                            _score.Add(50);
                            _logi.Add("Начислено 75 очков");
                        }
                    }

                }
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow && _pescerkaMap[_patsan.X - 1, _patsan.Y] != 1)
            {
                Console.SetCursorPosition(_patsan.X - 1, _patsan.Y);
                Console.Write('*');
                Thread.Sleep(100);
                Console.SetCursorPosition(_patsan.X - 1, _patsan.Y);
                Console.Write(' ');

                foreach (var enamies in _enemies)
                {
                    if (enamies.X == _patsan.X - 1 && enamies.Y == _patsan.Y)
                    {
                        enamies.TakeDamage(_patsan.Damage);
                        if (enamies.Symbol == 'Т')
                        {
                            _logi.Add("Т-позер был беспощадно изнечтожен");
                            _score.Add(50);
                            _logi.Add("Начислено 50 очков");
                        }
                        else if (enamies.Symbol == 'Д')
                        {
                            _logi.Add("Далек был повержен");
                            _score.Add(50);
                            _logi.Add("Начислено 75 очков");
                        }
                    }
                }
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && _pescerkaMap[_patsan.X, _patsan.Y + 1] != 1)
            {
                Console.SetCursorPosition(_patsan.X, _patsan.Y + 1);
                Console.Write('*');
                Thread.Sleep(100);
                Console.SetCursorPosition(_patsan.X, _patsan.Y + 1);
                Console.Write(' ');

                foreach (var enamies in _enemies)
                {
                    if (enamies.X == _patsan.X && enamies.Y == _patsan.Y + 1)
                    {
                        if (enamies.Symbol == 'Т')
                        {
                            _logi.Add("Т-позер был беспощадно изнечтожен");
                            _score.Add(50);
                            _logi.Add("Начислено 50 очков");
                        }
                        else if (enamies.Symbol == 'Д')
                        {
                            _logi.Add("Далек был повержен");
                            _score.Add(50);
                            _logi.Add("Начислено 75 очков");
                        }
                    }
                }
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow && _pescerkaMap[_patsan.X + 1, _patsan.Y] != 1)
            {
                Console.SetCursorPosition(_patsan.X + 1, _patsan.Y);
                Console.Write('*');
                Thread.Sleep(100);
                Console.SetCursorPosition(_patsan.X + 1, _patsan.Y);
                Console.Write(' ');

                foreach (var enamies in _enemies)
                {
                    if (enamies.X == _patsan.X + 1 && enamies.Y == _patsan.Y)
                    {
                        enamies.TakeDamage(_patsan.Damage);
                        if (enamies.Symbol == 'Т')
                        {
                            _logi.Add("Т-позер был беспощадно изнечтожен");
                            _score.Add(50);
                            _logi.Add("Начислено 50 очков");
                        }
                        else if (enamies.Symbol == 'Д')
                        {
                            _logi.Add("Далек был повержен");
                            _score.Add(50);
                            _logi.Add("Начислено 75 очков");
                        }
                    }
                }
            }
        }

        private void EbakaController()
        {
            foreach (var enamies in _enemies)
                if (enamies.Symbol == 'Т')
                {
                    if (GetIsFieldOfView(enamies, 2) && enamies.Cooldown >= 3)
                    {
                        if (enamies.X - 1 == _patsan.X && enamies.Y == _patsan.Y)
                        {
                            Console.SetCursorPosition(_patsan.X, _patsan.Y);
                            Console.Write('*');
                            Thread.Sleep(100);
                            Console.SetCursorPosition(_patsan.X, _patsan.Y);
                            Console.Write(' ');
                            _patsan.TakeDamage(enamies.Damage);
                            enamies.Cooldown = 0;
                            _logi.Add("Ты отхватил затрещину");
                            _logi.Add("Здоровье пошатнулось на " + enamies.Damage);
                        }
                        else if (enamies.X == _patsan.X && enamies.Y - 1 == _patsan.Y)
                        {
                            Console.SetCursorPosition(_patsan.X, _patsan.Y);
                            Console.Write('*');
                            Thread.Sleep(100);
                            Console.SetCursorPosition(_patsan.X, _patsan.Y);
                            Console.Write(' ');
                            _patsan.TakeDamage(enamies.Damage);
                            enamies.Cooldown = 0;
                            _logi.Add("Ты отхватил затрещину");
                            _logi.Add("Здоровье пошатнулось на " + enamies.Damage);

                        }
                        else if (enamies.X + 1 == _patsan.X && enamies.Y == _patsan.Y)
                        {
                            Console.SetCursorPosition(_patsan.X, _patsan.Y);
                            Console.Write('*');
                            Thread.Sleep(100);
                            Console.SetCursorPosition(_patsan.X, _patsan.Y);
                            Console.Write(' ');
                            _patsan.TakeDamage(enamies.Damage);
                            enamies.Cooldown = 0;
                            _logi.Add("Ты отхватил затрещину");
                            _logi.Add("Здоровье пошатнулось на " + enamies.Damage);
                        }
                        else if (enamies.X == _patsan.X && enamies.Y + 1 == _patsan.Y)
                        {
                            Console.SetCursorPosition(_patsan.X, _patsan.Y);
                            Console.Write('*');
                            Thread.Sleep(100);
                            Console.SetCursorPosition(_patsan.X, _patsan.Y);
                            Console.Write(' ');
                            _patsan.TakeDamage(enamies.Damage);
                            enamies.Cooldown = 0;
                            _logi.Add("Ты отхватил затрещину");
                            _logi.Add("Здоровье пошатнулось на " + enamies.Damage);
                        }
                    }
                    else
                        EnamiesWalking(enamies);

                    enamies.Cooldown += 1;
                }

        }

        private void RichesController()
        {
            foreach (var riches in _riches)
                if (riches.X == _patsan.X && riches.Y == _patsan.Y)
                {
                    _score.Add(riches.GetScoreRiches(riches.TypeRiches));
                    _logi.Add(riches.GetDescriptionRiches(riches.TypeRiches));
                    riches.Found = 0;
                    _richesMap[riches.X, riches.Y] = ' ';
                    Console.SetCursorPosition(riches.X, riches.Y);
                    Console.Write(' ');
                    break;
                }

        }

        private void HoboWithShotgunController()
        {
            foreach (var enamies in _enemies)
                if (enamies.Symbol == 'Д')
                {
                    bool foundPatsan = false;
                    bool isShoot = false;

                    if (GetIsFieldOfView(enamies, 5))
                    {
                        if (DirectioOfShot(enamies, 5) == (int)WalkingDirection.Up)
                        {
                            for (int i = 1; i <= 5; i++)
                            {
                                if (enamies.X == _patsan.X && enamies.Y - i == _patsan.Y)
                                    foundPatsan = true;
                                if (_pescerkaMap[enamies.X, enamies.Y - i] == 1)
                                    break;
                            }

                            if (foundPatsan)
                                isShoot = true;

                            if (isShoot && _bullets.Count == 0)
                            {
                                Bullet bullet = new Bullet('|', enamies.X, enamies.Y, 1, 10);
                                _bullets.Add(bullet);
                            }
                        }
                        else if (DirectioOfShot(enamies, 5) == (int)WalkingDirection.Left)
                        {
                            for (int i = 1; i <= 5; i++)
                            {
                                if (enamies.X - i == _patsan.X && enamies.Y == _patsan.Y)
                                    foundPatsan = true;
                                if (_pescerkaMap[enamies.X - i, enamies.Y] == 1)
                                    break;
                            }

                            if (foundPatsan)
                                isShoot = true;

                            if (isShoot && _bullets.Count == 0)
                            {
                                Bullet bullet = new Bullet('-', enamies.X, enamies.Y, 2, 10);
                                _bullets.Add(bullet);
                            }
                        }
                        else if (DirectioOfShot(enamies, 5) == (int)WalkingDirection.Down)
                        {
                            for (int i = 1; i <= 5; i++)
                            {
                                if (enamies.X == _patsan.X && enamies.Y + i == _patsan.Y)
                                    foundPatsan = true;
                                if (_pescerkaMap[enamies.X, enamies.Y + i] == 1)
                                    break;
                            }

                            if (foundPatsan)
                                isShoot = true;

                            if (isShoot && _bullets.Count == 0)
                            {
                                Bullet bullet = new Bullet('|', enamies.X, enamies.Y, 3, 10);
                                _bullets.Add(bullet);
                            }
                        }
                        else if (DirectioOfShot(enamies, 5) == (int)WalkingDirection.Right)
                        {
                            for (int i = 1; i <= 5; i++)
                            {
                                if (enamies.X + i == _patsan.X && enamies.Y == _patsan.Y)
                                    foundPatsan = true;
                                if (_pescerkaMap[enamies.X + i, enamies.Y] == 1)
                                    break;
                            }

                            if (foundPatsan)
                                isShoot = true;

                            if (isShoot && _bullets.Count == 0)
                            {
                                Bullet bullet = new Bullet('-', enamies.X, enamies.Y, 4, 10);
                                _bullets.Add(bullet);
                            }
                        }
                    }

                    EnamiesWalking(enamies);
                }

        }

        private void BulletMovement(List<Bullet> bullets, List<Unit> _enemies)
        {
            bool isClear = false;

            foreach (var bullet in bullets)
                if (bullet.Health == (int)WalkingDirection.Up)
                {
                    if (_pescerkaMap[bullet.X, bullet.Y - 1] == 0 && _unitMap[bullet.X, bullet.Y - 1] == ' ')
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        bullet.Move(0, -1);
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(bullet.Symbol);
                    }
                    else if (_unitMap[bullet.X, bullet.Y - 1] == 'Ф')
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        _patsan.TakeDamage(bullet.Damage);
                        isClear = true;
                        _logi.Add("Лазер прожег в тебе дырку");
                        _logi.Add("Здоровье пошатнулось на " + bullet.Damage);
                    }
                    else if (_unitMap[bullet.X, bullet.Y - 1] == 'Т' || _unitMap[bullet.X, bullet.Y - 1] == 'Д')
                    {
                        foreach (var enemies in _enemies)
                            if (enemies.X == bullet.X && enemies.Y == bullet.Y - 1)
                            {
                                Console.SetCursorPosition(bullet.X, bullet.Y);
                                Console.Write(' ');
                                enemies.TakeDamage(bullet.Damage);
                                isClear = true;
                                _logi.Add("Огонь по своим, так даже лучше");

                            }
                    }
                    else
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        isClear = true;
                    }
                }
                else if (bullet.Health == (int)WalkingDirection.Left)
                {
                    if (_pescerkaMap[bullet.X - 1, bullet.Y] == 0 && _unitMap[bullet.X - 1, bullet.Y] == ' ')
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        bullet.Move(-1, 0);
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(bullet.Symbol);
                    }
                    else if (_unitMap[bullet.X - 1, bullet.Y] == 'Ф')
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        _patsan.TakeDamage(bullet.Damage);
                        isClear = true;
                        _logi.Add("Лазер прожег в тебе дырку");
                        _logi.Add("Здоровье пошатнулось на " + bullet.Damage);
                    }
                    else if (_unitMap[bullet.X - 1, bullet.Y] == 'Т' || _unitMap[bullet.X - 1, bullet.Y] == 'Д')
                    {
                        foreach (var enemies in _enemies)
                            if (enemies.X == bullet.X - 1 && enemies.Y == bullet.Y)
                            {
                                Console.SetCursorPosition(bullet.X, bullet.Y);
                                Console.Write(' ');
                                enemies.TakeDamage(bullet.Damage);
                                isClear = true;
                                _logi.Add("Огонь по своим, так даже лучше");

                            }
                    }
                    else
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        isClear = true;
                    }
                }
                else if (bullet.Health == (int)WalkingDirection.Down)
                {
                    if (_pescerkaMap[bullet.X, bullet.Y + 1] == 0 && _unitMap[bullet.X, bullet.Y + 1] == ' ')
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        bullet.Move(0, 1);
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(bullet.Symbol);
                    }
                    else if (_unitMap[bullet.X, bullet.Y + 1] == 'Ф')
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        _patsan.TakeDamage(bullet.Damage);
                        isClear = true;
                        _logi.Add("Лазер прожег в тебе дырку");
                        _logi.Add("Здоровье пошатнулось на " + bullet.Damage);
                    }
                    else if (_unitMap[bullet.X, bullet.Y + 1] == 'Т' || _unitMap[bullet.X, bullet.Y + 1] == 'Д')
                    {
                        foreach (var enemies in _enemies)
                            if (enemies.X == bullet.X && enemies.Y == bullet.Y + 1)
                            {
                                Console.SetCursorPosition(bullet.X, bullet.Y);
                                Console.Write(' ');
                                enemies.TakeDamage(bullet.Damage);
                                isClear = true;
                                _logi.Add("Огонь по своим, так даже лучше");

                            }
                    }
                    else
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        isClear = true;
                    }
                }
                else if (bullet.Health == (int)WalkingDirection.Right)
                {

                    if (_pescerkaMap[bullet.X + 1, bullet.Y] == 0 && _unitMap[bullet.X + 1, bullet.Y] == ' ')
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        bullet.Move(1, 0);
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(bullet.Symbol);
                    }
                    else if (_unitMap[bullet.X + 1, bullet.Y] == 'Ф')
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        _patsan.TakeDamage(bullet.Damage);
                        isClear = true;
                        _logi.Add("Лазер прожег в тебе дырку");
                        _logi.Add("Здоровье пошатнулось на " + bullet.Damage);
                    }
                    else if (_unitMap[bullet.X + 1, bullet.Y] == 'Т' || _unitMap[bullet.X + 1, bullet.Y] == 'Д')
                    {
                        foreach (var enemies in _enemies)
                            if (enemies.X == bullet.X + 1 && enemies.Y == bullet.Y)
                            {
                                Console.SetCursorPosition(bullet.X, bullet.Y);
                                Console.Write(' ');
                                enemies.TakeDamage(bullet.Damage);
                                isClear = true;
                                _logi.Add("Огонь по своим, так даже лучше");

                            }
                    }
                    else
                    {
                        Console.SetCursorPosition(bullet.X, bullet.Y);
                        Console.Write(' ');
                        isClear = true;
                    }
                }


            if (isClear)
                bullets.Clear();
        }

        private void EnamiesWalking(Unit enamies)
        {
            Random rand = new Random();
            int walkingDirection = rand.Next(5);

            if (walkingDirection == (int)WalkingDirection.Up && _pescerkaMap[enamies.X, enamies.Y - 1] == 0 && _unitMap[enamies.X, enamies.Y - 1] == ' ')
            {
                _unitMap[enamies.X, enamies.Y] = ' ';
                Console.SetCursorPosition(enamies.X, enamies.Y);
                Console.Write(' ');
                enamies.Move(0, -1);
            }
            else if (walkingDirection == (int)WalkingDirection.Left && _pescerkaMap[enamies.X - 1, enamies.Y] == 0 && _unitMap[enamies.X - 1, enamies.Y] == ' ')
            {
                _unitMap[enamies.X, enamies.Y] = ' ';
                Console.SetCursorPosition(enamies.X, enamies.Y);
                Console.Write(' ');
                enamies.Move(-1, 0);
            }
            else if (walkingDirection == (int)WalkingDirection.Down && _pescerkaMap[enamies.X, enamies.Y + 1] == 0 && _unitMap[enamies.X, enamies.Y + 1] == ' ')
            {
                _unitMap[enamies.X, enamies.Y] = ' ';
                Console.SetCursorPosition(enamies.X, enamies.Y);
                Console.Write(' ');
                enamies.Move(0, 1);
            }
            else if (walkingDirection == (int)WalkingDirection.Right && _pescerkaMap[enamies.X + 1, enamies.Y] == 0 && _unitMap[enamies.X + 1, enamies.Y] == ' ')
            {
                _unitMap[enamies.X, enamies.Y] = ' ';
                Console.SetCursorPosition(enamies.X, enamies.Y);
                Console.Write(' ');
                enamies.Move(1, 0);
            }
        }

        private bool GetIsFieldOfView(Unit enamies, int fieldOfView)
        {
            bool isFieldOfView = false;

            for (int i = 1; i <= fieldOfView; i++)
                if (enamies.X - i == _patsan.X && enamies.Y == _patsan.Y)
                {
                    isFieldOfView = true;
                    break;
                }
                else if (enamies.X == _patsan.X && enamies.Y - i == _patsan.Y)
                {
                    isFieldOfView = true;
                    break;
                }
                else if (enamies.X + i == _patsan.X && enamies.Y == _patsan.Y)
                {
                    isFieldOfView = true;
                    break;
                }
                else if (enamies.X == _patsan.X && enamies.Y + i == _patsan.Y)
                {
                    isFieldOfView = true;
                    break;
                }
            return isFieldOfView;
        }

        private int DirectioOfShot(Unit enamies, int fieldOfView)
        {
            int directioOfShot = 0;

            for (int i = 1; i <= fieldOfView; i++)
                if (enamies.X == _patsan.X && enamies.Y - i == _patsan.Y)
                    directioOfShot = 1;
                else if (enamies.X - i == _patsan.X && enamies.Y == _patsan.Y)
                    directioOfShot = 2;
                else if (enamies.X == _patsan.X && enamies.Y + i == _patsan.Y)
                    directioOfShot = 3;
                else if (enamies.X + i == _patsan.X && enamies.Y == _patsan.Y)
                    directioOfShot = 4;
            return directioOfShot;
        }
    }
}
