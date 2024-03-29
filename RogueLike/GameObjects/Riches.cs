﻿using System.Numerics;

namespace RogueLike.GameObjects
{
    public class Riches
    {
        public char Symbol { get; }
        public Vector2 Position { get; }
        public int TypeRiches { get; }
        public int Found { get; set; }
        private string[] descriptionRiches = new string[]
        {
            "Кольцо всевластия. На внутреней стороне светятся буквы \"ЧО ПЯЛИШЬ?\"",
            "Зуб крота. Ну да, вот это улов",
            "Лунный камень. Что он вообще делает в пещере",
            "Томик Маркса. Щас мы возглавим в этой пещере рабочее движение",
            "Кем-то потерянный AirPods. Может быть тут рядом и второй есть вместе с кейсом",
            "Кросовки Nike. Ля что за тяги бархатные"
        };

        private int[] scoreRiches = new int[] { 500, 100, 200, 150, 400, 320 };

        public Riches(char symbol, Vector2 position, int typeRiches, int found)
        {
            Symbol = symbol;
            Position = position;
            TypeRiches = typeRiches;
            Found = found;
        }

        public int GetScoreRiches(int typeRiches)
        {
            return scoreRiches[typeRiches];
        }

        public string GetDescriptionRiches(int typeRiches)
        {
            return "Вы обнаружили " + descriptionRiches[typeRiches];
        }
    }
}
