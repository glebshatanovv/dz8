
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Song> songs = new List<Song>();

            Song song1 = new Song("Rosalia", "T-Fest");

            Song song2 = new Song("back to friend", "sombr", song1);

            Song song3 = new Song("Back Home", "Yeat", song2);

            Song song4 = new Song("Царица", "ANNA ASTI", song3);

            // Создание объекта без параметров
            Song mySong = new Song();

            songs.Add(song1);
            songs.Add(song2);
            songs.Add(song3);
            songs.Add(song4);

            Console.WriteLine("Список песен:");

            foreach (Song song in songs)
            {
                song.Print();
            }

            Console.WriteLine();

            Console.WriteLine("Сравнение первой и второй песни:");

            if (songs[0].Equals(songs[1]))
            {
                Console.WriteLine("Песни одинаковые");
            }
            else
            {
                Console.WriteLine("Песни разные");
            }
        }
    }

    class Song
    {
        string name;
        string author;
        Song prev;

        // Конструктор без параметров
        public Song()
        {
            name = "";
            author = "";
            prev = null;
        }

        // Конструктор с названием и автором
        public Song(string name, string author)
        {
            this.name = name;
            this.author = author;
            prev = null;
        }

        // Конструктор с названием, автором и предыдущей песней
        public Song(string name, string author, Song prev)
        {
            this.name = name;
            this.author = author;
            this.prev = prev;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetAuthor(string author)
        {
            this.author = author;
        }

        public void SetPrev(Song prev)
        {
            this.prev = prev;
        }

        public void Print()
        {
            Console.WriteLine(Title());
        }

        public string Title()
        {
            return name + " - " + author;
        }

        public override bool Equals(object d)
        {
            if (d == null || !(d is Song))
            {
                return false;
            }

            Song other = (Song)d;

            return name == other.name && author == other.author;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(name, author);
        }
    }
}
